using System;
using System.Collections.Specialized;
using System.Windows;
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
        private Visibility noPhotosMessageVisibility = Visibility.Visible;
        private Visibility noNotebookMessageVisibility = Visibility.Visible;
        private Visibility noInfoMessageVisibility = Visibility.Visible;

        public string Details { get; set; }
        public string Notebook { get; set; }
        public Cache Cache { get; set; }
        public ObservableCollection<Photo> Previews { get; set; }

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

        public InfoPivotViewModel()
        {
            Previews = new ObservableCollection<Photo>();
            Previews.CollectionChanged += Previews_CollectionChanged;
        }

        private void Previews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems.Count != 0)
            {
                NoPhotosMessageVisibility = Visibility.Collapsed;
            }
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
                GeocahingSuApiManager.Instance.DownloadAndProcessNotebook(LoadAndSaveNotebook, Cache.Id);
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
                GeocahingSuApiManager.Instance.DownloadAndProcessNotebook(LoadAndSaveNotebook, Cache.Id);
            }
        }

        public void LoadDetailsPivotItem(WebBrowser detailsBrowser)
        {
            var db = new CacheDataBase();
            var item = db.GetCache(Cache.Id);
            this.detailsBrowser = detailsBrowser;

            if ((item != null) && (item.Details != null))
            {
                Details = item.Details;
                NoInfoMessageVisibility = Visibility.Collapsed;
                detailsBrowser.NavigateToString(item.Details);
            }
            else if (Details != null)
            {
                LoadAndSaveCacheInfo(Details);
            }
            else
            {
                GeocahingSuApiManager.Instance.DownloadAndProcessInfo(LoadAndSaveCacheInfo, Cache.Id);
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

            if (Details != null)
            {
                LoadAndSaveCacheInfo(Details);
            }
            else
            {
                GeocahingSuApiManager.Instance.DownloadAndProcessInfo(LoadAndSaveCacheInfo, Cache.Id);
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
            Details = info;
            if (info != "")
            {
                NoInfoMessageVisibility = Visibility.Collapsed;
            }

            if (db.GetCache(Cache.Id) != null)
            {
                db.UpdateCacheInfo(info, Cache.Id);
            }

            if (detailsBrowser != null)
            {
                detailsBrowser.NavigateToString(info);
            }
        }

        public void LoadPhotos(List<string> uriList)
        {
            ProcessPhotos(uriList, GeocahingSuApiManager.Instance.LoadAndProcessPhoto);
        }

        private void ProcessPhotos(List<string> uriList, Action<string, Action<ImageSource, int>, int> processPhotos)
        {
            if (Previews == null)
            {
                return;
            }

            if (uriList == null)
            {
                //todo save photos
                return;
            }

            if (uriList.Count == Previews.Count)
            {
                //todo save potos
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
