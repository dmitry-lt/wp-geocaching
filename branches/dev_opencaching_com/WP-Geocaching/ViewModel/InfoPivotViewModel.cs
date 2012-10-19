using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
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

        private bool _noPhotosMessageVisible = true;
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

        private const string NoImageUriDark = "/Resources/Images/NoPhotoWhite.png";
        private const string NoImageUriLight = "/Resources/Images/NoPhotoBlack.png";

        private BitmapImage GetNoImageBitmap()
        {
            var darkBackgroundVisibility = (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];

            if (darkBackgroundVisibility == Visibility.Visible)
            {
                return new BitmapImage(new Uri(NoImageUriDark, UriKind.RelativeOrAbsolute));
            }
            else
            {
                return new BitmapImage(new Uri(NoImageUriLight, UriKind.RelativeOrAbsolute));
            }

        }

        private void ProcessPhotoUrls(List<string> photoUrls)
        {
            _photoUrls = photoUrls;

            if (null == _photoUrls || !_photoUrls.Any())
            {
                // TODO: all photos are loaded
            }
            else
            {
                NoPhotosMessageVisible = false;

                // TODO: download photos
                var photoDownloader = new PhotoDownloader();

                Previews = new ObservableCollection<Photo>();

                for (var i = 0; i < photoUrls.Count(); i++)
                {
                    var photo = new Photo(GetNoImageBitmap(), photoUrls[i], true);
                    Previews.Add(photo);

                    // TODO: all photos are loaded handler
                    photoDownloader.DownloadPhoto(
                        b => 
                        { 
                            photo.PhotoSource = b;
                            photo.IsPlaceholder = false;
                    
                        }
                        , photoUrls[i]);
                }
            }

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
                    ApiManager.Instance.FetchCacheDetails(ProcessInfo, ProcessLogbook, ProcessPhotoUrls, Cache);
                }
                else
                {
                    Info = dbCache.Description;
                    Logbook = dbCache.Logbook;
                    var helper = new FileStorageHelper();
                    foreach (var source in helper.GetPhotos(_cache))
                    {
                        Previews.Add(source);
                    }
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
                NotifyPropertyChanged("Previews");
            }
        }

        public bool NoPhotosMessageVisible
        {
            get
            {
                return _noPhotosMessageVisible;
            }
            set
            {
                _noPhotosMessageVisible = value;
                NotifyPropertyChanged("NoPhotosMessageVisible");
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
