using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model.Dialogs;

namespace WP_Geocaching.ViewModel
{
    public class InfoPivotViewModel : BaseViewModel
    {
        private WebBrowser _logbookBrowser;
        private WebBrowser _infoBrowser;

        private Visibility _noPhotosMessageVisibility = Visibility.Visible;
        private Visibility _noLogbookMessageVisibility = Visibility.Visible;
        private Visibility _noInfoMessageVisibility = Visibility.Visible;

        private ObservableCollection<Photo> _previews;
        private readonly Action _closeAction;
        private ConfirmDeleteDialog _confirmDeleteDialog;

        public string Info { get; set; }
        public string Logbook { get; set; }

        private Cache _cache;
        public Cache Cache
        {
            get { return _cache; }
            set
            {
                _cache = value;

                var db = new CacheDataBase();
                var dbCache = db.GetCache(Cache.Id, Cache.CacheProvider);
                if (null == dbCache || null == dbCache.Description || null == dbCache.Logbook)
                {
                    // TODO: photos
                    ApiManager.Instance.FetchCacheDetails(LoadAndSaveCacheInfo, LoadAndSaveLogbook, null, Cache);
                }

                NotifyPropertyChanged("Cache");
            }
        }

        // invoke this event to hide photos tab
        public event EventHandler HidePhotos;

        public ObservableCollection<Photo> Previews 
        { 
            get
            {
                return _previews;
            }

            set 
            { 
                _previews = value;
                if (value.Count != 0)
                {
                    NoPhotosMessageVisibility = Visibility.Collapsed;
                }
                NotifyPropertyChanged("Previews");
            }
        }

        public Visibility NoPhotosMessageVisibility
        {
            get
            {
                return _noPhotosMessageVisibility;
            }
            set
            {
                _noPhotosMessageVisibility = value;
                NotifyPropertyChanged("NoPhotosMessageVisibility");
            }
        }

        public Visibility NoLogbookMessageVisibility
        {
            get
            {
                return _noLogbookMessageVisibility;
            }
            set
            {
                _noLogbookMessageVisibility = value;
                NotifyPropertyChanged("NoLogbookMessageVisibility");
            }
        }

        public Visibility NoInfoMessageVisibility
        {
            get
            {
                return _noInfoMessageVisibility;
            }
            set
            {
                _noInfoMessageVisibility = value;
                NotifyPropertyChanged("NoInfoMessageVisibility");
            }
        }

        public InfoPivotViewModel(Action closeAction)
        {
            _closeAction = closeAction;

            Previews = new ObservableCollection<Photo>();
        }

        public void LoadLogbookPivotItem(WebBrowser logbookBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id, Cache.CacheProvider);
            _logbookBrowser = logbookBrowser;

            if ((item != null) && (item.Logbook != null))
            {
                Logbook = item.Logbook;
                NoLogbookMessageVisibility = Visibility.Collapsed;
                logbookBrowser.NavigateToString(item.Logbook);
            }
            else if (Logbook != null)
            {
                LoadAndSaveLogbook(Logbook);
            }
        }

        public void DownloadAndSaveLogbook()
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id, Cache.CacheProvider);

            if (item.Logbook != null)
            {
                return;
            }

            if (Logbook != null)
            {
                LoadAndSaveLogbook(Logbook);
            }
        }

        public void LoadDetailsPivotItem(WebBrowser detailsBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id, Cache.CacheProvider);
            _infoBrowser = detailsBrowser;

            if ((item != null) && (item.Description != null))
            {
                Info = item.Description;
                NoInfoMessageVisibility = Visibility.Collapsed;
                detailsBrowser.NavigateToString(item.Description);
            }
            else if (Info != null)
            {
                LoadAndSaveCacheInfo(Info);
            }
        }

        public void DownloadAndSaveCacheInfo()
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id, Cache.CacheProvider);

            if (item.Description != null)
            {
                return;
            }

            if (Info != null)
            {
                LoadAndSaveCacheInfo(Info);
            }
        }

        public void LoadPreviews()
        {
            ApiManager.Instance.LoadPhotos(Cache.Id, LoadPhotos);
        }

        public void DownloadAndSavePhotos()
        {
            ApiManager.Instance.SavePhotos(Cache.Id, SavePhotos);
        }

        private void SavePhotos(ObservableCollection<Photo> photos)
        {
            ProcessPhotos(photos);
        }

        private void LoadAndSaveLogbook(string logbook)
        {
            var db = new CacheDataBase();
            Logbook = logbook;
            if (logbook != "")
            {
                NoLogbookMessageVisibility = Visibility.Collapsed;
            }

            if (db.GetCache(Cache.Id, Cache.CacheProvider) != null)
            {
                db.UpdateCacheLogbook(logbook, Cache);
            }

            if (_logbookBrowser != null)
            {
                _logbookBrowser.NavigateToString(logbook);
            }
        }

        private void LoadAndSaveCacheInfo(string info)
        {
            var db = new CacheDataBase();
            Info = info;
            if (info != "")
            {
                NoInfoMessageVisibility = Visibility.Collapsed;
            }

            if (db.GetCache(Cache.Id, Cache.CacheProvider) != null)
            {
                db.UpdateCacheInfo(info, Cache);
            }

            if (_infoBrowser != null)
            {
                _infoBrowser.NavigateToString(info);
            }
        }

        public void LoadPhotos(ObservableCollection<Photo> photos)
        {
            ProcessPhotos(photos);
        }

        private void ProcessPhotos(ObservableCollection<Photo> photos)
        {
            if (Previews == null)
            {
                return;
            }

            if (photos == null)
            {
                //todo save photos
                return;
            }

            if (photos.Count == Previews.Count)
            {
                //todo save photos
            }

            if (Previews.Count == 0)
            {
                Previews = photos;
            }
        }

        public void ShowConfirmDeleteDialog(Dispatcher dispatcher)
        {
            if (_confirmDeleteDialog == null)
            {
                _confirmDeleteDialog = new ConfirmDeleteDialog(Cache, _closeAction, dispatcher);
            }
            _confirmDeleteDialog.Show();
        }
    }
}
