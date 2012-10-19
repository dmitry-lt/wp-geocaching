using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Globalization;
using WP_Geocaching.Model.Utils;
using WP_Geocaching.Model.DataBase;
using System.Text.RegularExpressions;
using Microsoft.Phone;

namespace WP_Geocaching.Model.Api.GeocachingSu
{
    /// <summary>
    /// IApiManager implementation for Geocaching.su
    /// </summary>
    public class GeocahingSuApiManager : IApiManager
    {
        private static readonly Encoding Cp1251Encoding = new CP1251Encoding();
        private const string LinkPattern = "\\s*(?i)href\\s*=\\s*(\"([^\"]*\")|'[^']*'|([^'\">\\s]+))";
        private const string InfoUrl = "http://pda.geocaching.su/cache.php?cid={0}";
        private const string LogbookUrl = "http://pda.geocaching.su/note.php?cid={0}&mode=0";
        private const string PhotosUrl = "http://pda.geocaching.su/pict.php?cid={0}&mode=0";
        private const string DownloadUrl =
            "http://www.geocaching.su/pages/1031.ajax.php?exactly=1&lngmax={0}&lngmin={1}&latmax={2}&latmin={3}&cacheId={4}&geocaching=f1fadbc82d0156ae0f81f7d5e0b26bda";
        private readonly int _id;

        public HashSet<Cache> Caches { get; private set; }

        internal GeocahingSuApiManager()
        {
            var random = new Random();
            _id = random.Next(100000000);
            Caches = new HashSet<Cache>();
        }

        public void FetchCaches(Action<List<Cache>> processCaches, double lngmax, double lngmin, double latmax, double latmin)
        {
            var sUrl = String.Format(CultureInfo.InvariantCulture, DownloadUrl, lngmax, lngmin, latmax, latmin, _id);
            var client = new WebClient
                             {
                                 Encoding = Cp1251Encoding
                             };
            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;
                var caches = XElement.Parse(e.Result);
                var parser = new GeocachingSuCacheParser();
                var downloadedCaches = parser.Parse(caches);

                foreach (var p in downloadedCaches)
                {
                    if (!Caches.Contains(p))
                    {
                        Caches.Add(p);
                    }
                }

                if (processCaches == null) return;
                var list = (from cache in Caches
                            where ((cache.Location.Latitude <= latmax) &&
                                   (cache.Location.Latitude >= latmin) &&
                                   (cache.Location.Longitude <= lngmax) &&
                                   (cache.Location.Longitude >= lngmin))
                            select cache).ToList<Cache>();
                processCaches(list);
            };
            client.DownloadStringAsync(new Uri(sUrl));
        }

        public void FetchCacheDetails(Action<string> processDescription, Action<string> processLogbook, Action<List<string>> processPhotoUrls, Cache cache)
        {
            if (null != processDescription)
            {
                DownloadAndProcessDescription(processDescription, cache);
            }

            if (null != processLogbook)
            {
                DownloadAndProcessLogbook(processLogbook, cache);
            }

            if (null != processPhotoUrls)
            {
                DownloadAndProcessPhotoUrls(processPhotoUrls, cache);
            }
        }

        /// <summary>
        /// Downloads data at the url by cacheId
        /// </summary>
        /// <param name="processData">processes downloaded result</param>
        private static void DownloadAndProcessData(string url, Action<string> processData, string cacheId)
        {
            var webClient = new WebClient();

            webClient.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null && processData != null)
                {
                    processData(e.Result);
                }
            };

            webClient.AllowReadStreamBuffering = true;
            webClient.Encoding = Cp1251Encoding;
            webClient.DownloadStringAsync(new Uri(String.Format(url, cacheId), UriKind.Absolute));
        }

        /// <summary>
        /// Downloads cache info
        /// </summary>
        /// <param name="processCacheInfo">processes downloaded result</param>
        /// <param name="cache"> </param>
        private void DownloadAndProcessDescription(Action<string> processCacheInfo, Cache cache)
        {
            DownloadAndProcessData(InfoUrl, processCacheInfo, cache.Id);
        }

        /// <summary>
        /// Downloads cache logbook
        /// </summary>
        /// <param name="processCacheLogbook"> </param>
        /// <param name="cache"> </param>
        private void DownloadAndProcessLogbook(Action<string> processCacheLogbook, Cache cache)
        {
            DownloadAndProcessData(LogbookUrl, processCacheLogbook, cache.Id);
        }

        private void DownloadAndProcessPhotoUrls(Action<List<string>> processPhotoUrls, Cache cache)
        {
            if (null == processPhotoUrls)
            {
                return;
            }

            var webClient = new WebClient();

            webClient.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    return;
                }

                processPhotoUrls(ParsePhotoUrls(e.Result));
            };

            webClient.AllowReadStreamBuffering = true;
            webClient.Encoding = Cp1251Encoding;
            webClient.DownloadStringAsync(new Uri(String.Format(PhotosUrl, cache.Id), UriKind.Absolute));
        }

        public Cache GetCache(string cacheId, CacheProvider cacheProvider)
        {
            return Caches.FirstOrDefault(p => p.Id == cacheId);
        }

        #region Photo Downloading

        private List<String> _photoUrls;
        private List<String> _photoNames;
        private ObservableCollection<Photo> _images;
        private Cache _cache;

        public void SavePhotos(Cache cache, Action<ObservableCollection<Photo>> processAction)
        {
            ProcessPhotos(cache, processAction, SaveAndProcessPhoto);
        }

        public void ProcessPhotos(Cache cache, Action<ObservableCollection<Photo>> processAction, Action<string, int> processIdentifier)
        {
            var db = new CacheDataBase();
            var helper = new FileStorageHelper();

            if (null != _cache && _cache.Id != cache.Id)
            {
                ResetPhotoCacheData(cache);
            }

            if ((db.GetCache(cache.Id, CacheProvider.GeocachingSu) != null) && helper.IsPhotosExist(cache))
            {
                ProcessPhotosFromIsolatedStorage(cache, processAction, processIdentifier);
            }
            else
            {
                ProcessPhotosFromWeb(cache, processAction, processIdentifier);
            }
        }

        private void ProcessPhotosFromWeb(Cache cache, Action<ObservableCollection<Photo>> processAction, Action<string, int> processIdentifier)
        {
            var webClient = new WebClient();

            webClient.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    if (processAction != null)
                    {
                        processAction(null);
                    }
                    return;
                }

                ResetPhotoUrls(e.Result);
                InitializeImages(_photoUrls.Count);

                if (processAction == null)
                {
                    return;
                }

                processAction(_images);

                if (processIdentifier == null)
                {
                    return;
                }

                for (var i = 0; i < _photoUrls.Count; i++)
                {
                    processIdentifier(_photoUrls[i], i);
                }
            };

            webClient.AllowReadStreamBuffering = true;
            webClient.Encoding = Cp1251Encoding;
            webClient.DownloadStringAsync(new Uri(String.Format(PhotosUrl, cache), UriKind.Absolute));
        }

        private void ProcessPhotosFromIsolatedStorage(Cache cache, Action<ObservableCollection<Photo>> processAction, Action<string, int> processIdentifier)
        {
            var helper = new FileStorageHelper();

            if (_photoNames == null)
            {
                _photoNames = helper.GetPhotoNames(cache);
            }

            InitializeImages(_photoNames.Count);

            if (processAction == null)
            {
                return;
            }

            processAction(_images);

            if (processIdentifier == null)
            {
                return;
            }

            for (var i = 0; i < _photoNames.Count; i++)
            {
                processIdentifier(_photoNames[i], i);
            }
        }

        private void ResetPhotoCacheData(Cache cache)
        {
            this._cache = cache;
            _photoUrls = _photoNames = null;
            _images = null;
        }

        private void ResetPhotoUrls(string htmlPhotoUrls)
        {
            if (_photoUrls != null)
            {
                return;
            }

            _photoUrls = new List<string>();
            var urls = Regex.Matches(htmlPhotoUrls, LinkPattern);

            for (var i = 0; i < urls.Count; i++)
            {
                var url = urls[i].Value.Substring(7, urls[i].Value.Length - 8);

                if (url.EndsWith(".jpg"))
                {
                    _photoUrls.Add(url);
                }
            }
        }

        private List<string> ParsePhotoUrls(string html)
        {
            var photoUrls = new List<string>();
            var urls = Regex.Matches(html, LinkPattern);

            for (var i = 0; i < urls.Count; i++)
            {
                var url = urls[i].Value.Substring(7, urls[i].Value.Length - 8);

                if (url.EndsWith(".jpg"))
                {
                    photoUrls.Add(url);
                }
            }

            return photoUrls;
        }

        private void InitializeImages(int count)
        {
            if (_images != null)
            {
                return;
            }

            _images = new ObservableCollection<Photo>();

            for (var i = 0; i < count; i++)
            {
                _images.Add(null);
            }
        }

        public void SaveAndProcessPhoto(string photoIdentifier, int index)
        {
            if (!IsUrl(photoIdentifier)) //parallel threads error
            {
                return;
            }

            var helper = new FileStorageHelper();
            var photoUri = new Uri(photoIdentifier);
            var fileName = (photoUri.AbsolutePath.Substring(photoUri.AbsolutePath.LastIndexOf("/"))).Substring(1);

            if (helper.IsOnePhotoExists(_cache, fileName))
            {
                return;
            }

            if (_images.Count <= index) // theoretically impossible
            {
                return;
            }

            if (_images[index] == null)
            {

                var webClient = new WebClient();
                webClient.OpenReadCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        return;
                    }

                    var writableBitmap = PictureDecoder.DecodeJpeg(e.Result);
                    helper.SavePhoto(_cache, fileName, writableBitmap);
                    _images[index] = new Photo(writableBitmap, fileName, false);
                };
                webClient.OpenReadAsync(photoUri);
            }
        }

        public void LoadAndProcessPhoto(string photoIdentifier, int index)
        {
            if (photoIdentifier == null) // theoretically impossible
            {
                return;
            }

            var helper = new FileStorageHelper();          

            if (IsUrl(photoIdentifier))
            {
                var photoUri = new Uri(photoIdentifier);
                var webClient = new WebClient();
                webClient.OpenReadCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        return;
                    }

                    var writableBitmap = PictureDecoder.DecodeJpeg(e.Result);

                    var fileName = (photoUri.AbsolutePath.Substring(photoUri.AbsolutePath.LastIndexOf("/"))).Substring(1);
                    if ((new CacheDataBase()).GetCache(_cache.Id, CacheProvider.GeocachingSu) != null)
                    {
                        helper.SavePhoto(_cache, fileName, writableBitmap);
                    }

                    _images[index] = new Photo(writableBitmap, fileName, false);
                };

                webClient.OpenReadAsync(photoUri);
            }
            else
            {
                _images[index] = helper.GetPhoto(_cache, photoIdentifier);
            }
        }

        private bool IsUrl(string data)
        {
            return data != null && data.Contains("http://");
        }

        public void DeletePhotos(Cache cache)
        {
            var helper = new FileStorageHelper();
            helper.DeletePhotos(cache);
            ResetPhotoDataAfterDeleting();
        }

        private void ResetPhotoDataAfterDeleting()
        {
            _photoUrls = _photoNames = null;
        }

        #endregion
    }
}
