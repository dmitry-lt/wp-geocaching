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

        public void ProcessUriList(List<string> uriList)
        {
            for (int i = 0; i < uriList.Count; i++)
            {
                GeocahingSuApiManager.Instance.LoadPhoto(uriList[i], ProcaessPhoto);
            }
        }

        ThreePhotos photos;
        private void ProcaessPhoto(PreviewImage photo)
        {
            if (photos == null)
            {
                photos = new ThreePhotos();
                photos.Add(photo);
                return;
            }
            if (photos.isFull())
            {
                Previews.Add(photos);
                photos = new ThreePhotos();
                return;
            }
            photos.Add(photo);
        }
    }
}
