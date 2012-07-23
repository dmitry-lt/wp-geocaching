﻿using System;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.DataBase;

namespace WP_Geocaching.ViewModel
{
    public class InfoPivotViewModel : BaseViewModel
    {
        private WebBrowser notebookBrowser;
        private WebBrowser detailsBrowser;

        public string Details { get; set; }
        public string Notebook { get; set; }
        public Cache Cache { get; set; }
        public ObservableCollection<Photo> Previews { get; set; }

        public InfoPivotViewModel()
        {
            Previews = new ObservableCollection<Photo>();
        }

        public void LoadNotebookPivotItem(WebBrowser notebookBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);
            this.notebookBrowser = notebookBrowser;

            if ((item != null) && (item.Notebook != null))
            {
                notebookBrowser.NavigateToString(item.Notebook);
            }
            else
            {
                GeocahingSuApiManager.Instance.DownloadAndProcessNotebook(ProcessNotebook, Cache.Id);
            }
        }

        public void LoadDetailsPivotItem(WebBrowser detailsBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);
            this.detailsBrowser = detailsBrowser;

            if ((item != null) && (item.Details != null))
            {
                detailsBrowser.NavigateToString(item.Details);
            }
            else
            {
                GeocahingSuApiManager.Instance.DownloadAndProcessInfo(ProcessCacheInfo, Cache.Id);
            }
        }

        public void LoadPreviews()
        {
            GeocahingSuApiManager.Instance.ProcessPhotos(Cache.Id, LoadPhotos);
        }

        public void DownloadAndSavePhotos()
        {
            GeocahingSuApiManager.Instance.ProcessPhotos(Cache.Id, SavePhotos);
        }

        public void DeletePhotos()
        {
            GeocahingSuApiManager.Instance.DeletePhotos(Cache.Id);
        }

        private void SavePhotos(List<string> uriList)
        {
            ProcessPhotos(uriList, GeocahingSuApiManager.Instance.SaveAndProcessPhoto);
        }

        private void ProcessNotebook(string notebook)
        {
            var db = new CacheDataBase();
            Notebook = notebook;

            if (db.GetCache(Cache.Id) != null)
            {
                db.UpdateCacheNotebook(notebook, Cache.Id);
            }

            notebookBrowser.NavigateToString(notebook);
        }

        private void ProcessCacheInfo(string info)
        {
            var db = new CacheDataBase();
            Details = info;

            if (db.GetCache(Cache.Id) != null)
            {
                db.UpdateCacheInfo(info, Cache.Id);
            }

            detailsBrowser.NavigateToString(info);
        }

        public void LoadPhotos(List<string> uriList)
        {
            ProcessPhotos(uriList, GeocahingSuApiManager.Instance.LoadAndProcessPhoto);
        }

        private void ProcessPhotos(List<string> uriList, Action<string, Action<ImageSource, int>, int> processPhotos)
        {
            if (uriList == null || Previews == null)
            {
                return;
            }

            if (Previews.Count == 0)
            {
                FillPreviews(uriList.Count);
            }

            for (var i = 0; i < uriList.Count; i++)
            {
                processPhotos(uriList[i], AddPhotoToPreviews, i);
            }
        }

        private void AddPhotoToPreviews(ImageSource photo, int index)
        {
            Previews[index].PhotoSource = photo;
            Previews[index].Index = index;
        }

        private void FillPreviews(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Previews.Add(new Photo());
            }
        }
    }
}