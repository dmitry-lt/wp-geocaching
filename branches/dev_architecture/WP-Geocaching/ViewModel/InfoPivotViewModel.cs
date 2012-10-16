using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using System.Collections.ObjectModel;
using WP_Geocaching.Model;
using WP_Geocaching.Model.Api;
using WP_Geocaching.Model.Api.OpenCachingCom;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model.Dialogs;

namespace WP_Geocaching.ViewModel
{
    public class InfoPivotViewModel : BaseViewModel
    {
        private WebBrowser notebookBrowser;
        private WebBrowser infoBrowser;

        private Visibility noPhotosMessageVisibility = Visibility.Visible;
        private Visibility noNotebookMessageVisibility = Visibility.Visible;
        private Visibility noInfoMessageVisibility = Visibility.Visible;

        private ObservableCollection<Photo> previews;
        private Action closeAction;
        private ConfirmDeleteDialog confirmDeleteDialog;

        public string Info { get; set; }
        public string Notebook { get; set; }

        private Cache _cache;
        public Cache Cache
        {
            get { return _cache; }
            set
            {
                _cache = value;
                NotifyPropertyChanged("Cache");
            
                // TODO: refactor
                if (_cache is OpenCachingComCache)
                {
                    HidePhotos(this, new EventArgs());
                }
            }
        }

        public event EventHandler HidePhotos;

        public ObservableCollection<Photo> Previews 
        { 
            get
            {
                return previews;
            }

            set 
            { 
                previews = value;
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
                return noPhotosMessageVisibility;
            }
            set
            {
                noPhotosMessageVisibility = value;
                NotifyPropertyChanged("NoPhotosMessageVisibility");
            }
        }

        public Visibility NoNotebookMessageVisibility
        {
            get
            {
                return noNotebookMessageVisibility;
            }
            set
            {
                noNotebookMessageVisibility = value;
                NotifyPropertyChanged("NoNotebookMessageVisibility");
            }
        }

        public Visibility NoInfoMessageVisibility
        {
            get
            {
                return noInfoMessageVisibility;
            }
            set
            {
                noInfoMessageVisibility = value;
                NotifyPropertyChanged("NoInfoMessageVisibility");
            }
        }

        public InfoPivotViewModel(Action closeAction)
        {
            this.closeAction = closeAction;

            Previews = new ObservableCollection<Photo>();
        }

        public void LoadNotebookPivotItem(WebBrowser notebookBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);
            this.notebookBrowser = notebookBrowser;

            if ((item != null) && (item.Notebook != null))
            {
                Notebook = item.Notebook;
                NoNotebookMessageVisibility = Visibility.Collapsed;
                notebookBrowser.NavigateToString(item.Notebook);
            }
            else if (Notebook != null)
            {
                LoadAndSaveNotebook(Notebook);
            }
            else
            {
                ApiManager.Instance.DownloadAndProcessNotebook(LoadAndSaveNotebook, Cache);
            }
        }

        public void DownloadAndSaveNotebook()
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);

            if (item.Notebook != null)
            {
                return;
            }

            if (Notebook != null)
            {
                LoadAndSaveNotebook(Notebook);
            }
            else
            {
                ApiManager.Instance.DownloadAndProcessNotebook(LoadAndSaveNotebook, Cache);
            }
        }

        public void LoadDetailsPivotItem(WebBrowser detailsBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);
            infoBrowser = detailsBrowser;

            if ((item != null) && (item.Details != null))
            {
                Info = item.Details;
                NoInfoMessageVisibility = Visibility.Collapsed;
                detailsBrowser.NavigateToString(item.Details);
            }
            else if (Info != null)
            {
                LoadAndSaveCacheInfo(Info);
            }
            else
            {
                ApiManager.Instance.DownloadAndProcessInfo(LoadAndSaveCacheInfo, Cache);
            }
        }

        public void DownloadAndSaveCacheInfo()
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);

            if (item.Details != null)
            {
                return;
            }

            if (Info != null)
            {
                LoadAndSaveCacheInfo(Info);
            }
            else
            {
                ApiManager.Instance.DownloadAndProcessInfo(LoadAndSaveCacheInfo, Cache);
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

        private void LoadAndSaveNotebook(string notebook)
        {
            var db = new CacheDataBase();
            Notebook = notebook;
            if (notebook != "")
            {
                NoNotebookMessageVisibility = Visibility.Collapsed;
            }

            if (db.GetCache(Cache.Id) != null)
            {
                db.UpdateCacheNotebook(notebook, Cache.Id);
            }

            if (notebookBrowser != null)
            {
                notebookBrowser.NavigateToString(notebook);
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

            if (db.GetCache(Cache.Id) != null)
            {
                db.UpdateCacheInfo(info, Cache.Id);
            }

            if (infoBrowser != null)
            {
                infoBrowser.NavigateToString(info);
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
            if (confirmDeleteDialog == null)
            {
                confirmDeleteDialog = new ConfirmDeleteDialog(Cache, closeAction, dispatcher);
            }
            confirmDeleteDialog.Show();
        }
    }
}
