using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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
        public ObservableCollection<ThreePhotos> Previews { get; set; }

        public InfoPivotViewModel()
        {
            Previews = new ObservableCollection<ThreePhotos>();
        }

        public void LoadNotebookPivotItem(WebBrowser notebookBrowser)
        {
            CacheDataBase db = new CacheDataBase();
            DbCacheItem item = db.GetCache(Cache.Id);
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
            CacheDataBase db = new CacheDataBase();
            DbCacheItem item = db.GetCache(Cache.Id);
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
            GeocahingSuApiManager.Instance.DownloadPhotos(Cache.Id, ProcessUriList);
        }

        private void ProcessNotebook(string notebook)
        {
            CacheDataBase db = new CacheDataBase();
            Notebook = notebook;
            if (db.GetCache(Cache.Id) != null)
            {
                db.UpdateCacheNotebook(notebook, Cache.Id);
            }
            notebookBrowser.NavigateToString(notebook);
        }

        private void ProcessCacheInfo(string info)
        {
            CacheDataBase db = new CacheDataBase();
            Details = info;
            if (db.GetCache(Cache.Id) != null)
            {
                db.UpdateCacheInfo(info, Cache.Id);
            }
            detailsBrowser.NavigateToString(info);
        }

        private int PhotoCount;

        public void ProcessUriList(List<string> uriList)
        {
            if (Previews.Count == 0)
            {
                PhotoCount = uriList.Count;
                for (int i = 0; i < PhotoCount; i++)
                {
                    GeocahingSuApiManager.Instance.LoadPhoto(uriList[i], ProcaessPhoto);
                }
            }
        }

        private ThreePhotos photos;

        private void ProcaessPhoto(ImageSource photo)
        {
            if (photos == null)
            {
                photos = new ThreePhotos();
                photos.Add(photo);
                CheckAndSetThreePhotos();
                return;
            }
            if (photos.isFull())
            {
                Previews.Add(photos);
                photos = new ThreePhotos();
                photos.Add(photo);
                CheckAndSetThreePhotos();
                return;
            }
            photos.Add(photo);
            CheckAndSetThreePhotos();
        }
        private void CheckAndSetThreePhotos()
        {
            if ((Previews.Count) * 3 + photos.Count == PhotoCount)
            {
                Previews.Add(photos);
            }
        }
    }
}
