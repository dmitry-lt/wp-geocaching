using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;
using GeocachingPlus.Model;
using GeocachingPlus.Model.Api;
using GeocachingPlus.Model.DataBase;
using GeocachingPlus.Model.Dialogs;

namespace GeocachingPlus.ViewModel
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

        private string _hint;
        public string Hint
        {
            get { return _hint; }
            set
            {
                _hint = value;
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

        private bool _hintLoaded;
        private void ProcessHint(string hint)
        {
            Hint = hint;
            _hintLoaded = true;
            CheckFullyLoaded();
        }

        private class PhotoInfo
        {
            public string Url;
            public bool Loaded;
        }

        private readonly List<PhotoInfo> _photoInfos = new List<PhotoInfo>();

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

        private bool _photoUrlLoaded;
        private void ProcessPhotoUrls(List<string> photoUrls)
        {
            if (null != photoUrls)
            {
                foreach (var photoUrl in photoUrls)
                {
                    _photoInfos.Add(new PhotoInfo() {Url = photoUrl});
                }
            }

            if (null == _photoInfos || !_photoInfos.Any())
            {
                CheckFullyLoaded();
            }
            else
            {
                NoPhotosMessageVisible = false;

                var photoDownloader = new PhotoDownloader();

                Previews = new ObservableCollection<Photo>();

                for (var i = 0; i < _photoInfos.Count(); i++)
                {
                    var photoInfo = _photoInfos[i];
                    var photo = new Photo(GetNoImageBitmap(), photoInfo.Url, true);
                    Previews.Add(photo);

                    photoDownloader.DownloadPhoto(
                        b => 
                        { 
                            photo.PhotoSource = b;
                            photo.IsPlaceholder = false;
                            photoInfo.Loaded = true;
                            CheckFullyLoaded();
                        }
                        , photoInfo.Url);
                }
            }

            _photoUrlLoaded = true;
            CheckFullyLoaded();
        }

        private readonly object _lock = new object();
        private void CheckFullyLoaded()
        {
            lock (_lock)
            {
                if (_infoLoaded && _logbookLoaded && _photoUrlLoaded && _photoInfos.All(p => p.Loaded) && _hintLoaded)
                {
                    CacheFullyLoaded(this, new EventArgs());
                }
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
                if (null == dbCache || null == dbCache.HtmlDescription || null == dbCache.HtmlLogbook)
                {
                    ApiManager.Instance.FetchCacheDetails(ProcessInfo, ProcessLogbook, ProcessPhotoUrls, ProcessHint, Cache);
                }
                else
                {
                    Info = dbCache.HtmlDescription;
                    Logbook = dbCache.HtmlLogbook;
                    var helper = new FileStorageHelper();
                    foreach (var source in helper.GetPhotos(_cache))
                    {
                        NoPhotosMessageVisible = false;
                        Previews.Add(source);
                    }
                    CacheFullyLoaded(this, new EventArgs());
                }

                RaisePropertyChanged(() => Cache);
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
                RaisePropertyChanged(() => Previews);
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
                RaisePropertyChanged(() => NoPhotosMessageVisible);
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
                RaisePropertyChanged(() => NoLogbookMessageVisible);
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
                RaisePropertyChanged(() => NoInfoMessageVisible);
            }
        }

        private bool _isCacheFullyLoaded;
        public bool IsCacheFullyLoaded
        {
            get
            {
                return _isCacheFullyLoaded;
            }
            set
            {
                _isCacheFullyLoaded = value;
                RaisePropertyChanged(() => IsCacheFullyLoaded);
            }
        }

        public InfoPivotViewModel(Action closeAction, WebBrowser infoBrowser, WebBrowser logbookBrowser)
        {
            _closeAction = closeAction;
            _logbookBrowser = logbookBrowser;
            _infoBrowser = infoBrowser;

            Previews = new ObservableCollection<Photo>();

            CacheFullyLoaded += (s,e) => IsCacheFullyLoaded = true;
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
