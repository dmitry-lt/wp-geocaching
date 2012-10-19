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
        private readonly WebBrowser _logbookBrowser;
        private readonly WebBrowser _infoBrowser;

        private Visibility _noPhotosMessageVisibility = Visibility.Visible;
        private bool _noLogbookMessageVisibile = true;
        private bool _noInfoMessageVisible = true;

        private ObservableCollection<Photo> _previews;
        private readonly Action _closeAction;
        private ConfirmDeleteDialog _confirmDeleteDialog;

        private string _info;
        public string Info
        {
            get { return _info;  }
            set 
            { 
                _info = value;
                NoInfoMessageVisible = value == null;
                _infoBrowser.NavigateToString(_info);
            }
        }

        private string _logbook;
        public string Logbook
        {
            get { return _logbook; }
            set
            {
                _logbook = value;
                NoLogbookMessageVisible = value == null;
                _logbookBrowser.NavigateToString(_logbook);
            }
        }

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
                    ApiManager.Instance.FetchCacheDetails(s => Info = s, s => Logbook = s, null, Cache);
                }
                else
                {
                    Info = dbCache.Description;
                    Logbook = dbCache.Logbook;
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

        public bool NoLogbookMessageVisible
        {
            get
            {
                return _noLogbookMessageVisibile;
            }
            set
            {
                _noLogbookMessageVisibile = value;
                NotifyPropertyChanged("NoLogbookMessageVisible");
            }
        }

        public bool NoInfoMessageVisible
        {
            get
            {
                return _noInfoMessageVisible;
            }
            set
            {
                _noInfoMessageVisible = value;
                NotifyPropertyChanged("NoInfoMessageVisible");
            }
        }

        public InfoPivotViewModel(Action closeAction, WebBrowser infoBrowser, WebBrowser logbookBrowser)
        {
            _closeAction = closeAction;
            _logbookBrowser = logbookBrowser;
            _infoBrowser = infoBrowser;

            Previews = new ObservableCollection<Photo>();
        }

        public void LoadLogbookPivotItem(WebBrowser logbookBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id, Cache.CacheProvider);

            if ((item != null) && (item.Logbook != null))
            {
                Logbook = item.Logbook;
                NoLogbookMessageVisible = false;
                logbookBrowser.NavigateToString(item.Logbook);
            }
            else if (Logbook != null)
            {
                LoadAndSaveLogbook(Logbook);
            }
        }

        public void LoadDetailsPivotItem(WebBrowser detailsBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id, Cache.CacheProvider);

            if ((item != null) && (item.Description != null))
            {
                Info = item.Description;
                NoInfoMessageVisible = false;
                detailsBrowser.NavigateToString(item.Description);
            }
            else if (Info != null)
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
                NoLogbookMessageVisible = false;
            }

            if (db.GetCache(Cache.Id, Cache.CacheProvider) != null)
            {
                db.UpdateCacheLogbook(logbook, Cache);
            }
        }

        private void LoadAndSaveCacheInfo(string info)
        {
            var db = new CacheDataBase();
            Info = info;
            if (info != "")
            {
                NoInfoMessageVisible = false;
            }

            if (db.GetCache(Cache.Id, Cache.CacheProvider) != null)
            {
                db.UpdateCacheInfo(info, Cache);
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
