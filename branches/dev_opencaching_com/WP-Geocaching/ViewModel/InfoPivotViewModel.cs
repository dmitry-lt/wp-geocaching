using System;
using System.Collections.Generic;
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

        private bool _infoLoaded;
        private void ProcessInfo(string info)
        {
            Info = info;
            _infoLoaded = true;
            CheckFullyLoaded();
        }

        private bool _logbookLoaded;
        private void ProcessLogbook(string logbook)
        {
            Logbook = logbook;
            _logbookLoaded = true;
            CheckFullyLoaded();
        }

        private List<string> _photoUrls;
        private bool _photoUrlsLoaded;

        private void ProcessPhotoUrls(List<string> photoUrls)
        {
            _photoUrls = photoUrls;

            // TODO: download photos

            _photoUrlsLoaded = true;
            CheckFullyLoaded();
        }

        private void CheckFullyLoaded()
        {
            if (_infoLoaded && _logbookLoaded && _photoUrlsLoaded)
            {
                CacheFullyLoaded(this, new EventArgs());
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
                    ApiManager.Instance.FetchCacheDetails(ProcessInfo, ProcessLogbook, ProcessPhotoUrls, Cache);
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
        public event EventHandler CacheFullyLoaded;

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

        public void LoadPreviews()
        {
            ApiManager.Instance.LoadPhotos(Cache, ProcessPhotos);
        }

        public void DownloadAndSavePhotos()
        {
            ApiManager.Instance.SavePhotos(Cache, ProcessPhotos);
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
