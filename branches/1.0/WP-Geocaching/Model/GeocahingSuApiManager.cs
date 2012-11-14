﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Globalization;
using WP_Geocaching.Model.Utils;
using WP_Geocaching.Model.DataBase;
using System.Text.RegularExpressions;
using Microsoft.Phone;
using WP_Geocaching.Model;

namespace WP_Geocaching.Model
{
    /// <summary>
    /// IApiManager implementation for Geocaching.su
    /// </summary>
    public class GeocahingSuApiManager : IApiManager
    {
        private static GeocahingSuApiManager instance;
        private static readonly Encoding CP1251Encoding = new CP1251Encoding();
        private const string LinkPattern = "\\s*(?i)href\\s*=\\s*(\"([^\"]*\")|'[^']*'|([^'\">\\s]+))";
        private const string InfoUrl = "http://pda.geocaching.su/cache.php?cid={0}";
        private const string NotebookUrl = "http://pda.geocaching.su/note.php?cid={0}&mode=0";
        private const string PhotosUrl = "http://pda.geocaching.su/pict.php?cid={0}&mode=0";
        private const string DownloadUrl =
            "http://www.geocaching.su/pages/1031.ajax.php?exactly=1&lngmax={0}&lngmin={1}&latmax={2}&latmin={3}&cacheId={4}&geocaching=f1fadbc82d0156ae0f81f7d5e0b26bda";
        private int id;

        public HashSet<Cache> CacheList { get; set; }

        private GeocahingSuApiManager()
        {
            var random = new Random();
            id = random.Next(100000000);
            CacheList = new HashSet<Cache>();
        }

        public static GeocahingSuApiManager Instance
        {
            get
            {
                return instance ?? (instance = new GeocahingSuApiManager());
            }
        }

        public void UpdateCacheList(Action<List<Cache>> processCacheList, double lngmax, double lngmin, double latmax, double latmin)
        {
            var sUrl = String.Format(CultureInfo.InvariantCulture, DownloadUrl, lngmax, lngmin, latmax, latmin, id);
            var client = new WebClient
                             {
                                 Encoding = CP1251Encoding
                             };
            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null) return;
                var caches = XElement.Parse(e.Result);
                var parser = new CacheParser();
                var downloadedCaches = parser.Parse(caches);

                foreach (var p in downloadedCaches)
                {
                    if (!CacheList.Contains(p))
                    {
                        CacheList.Add(p);
                    }
                }

                if (processCacheList == null) return;
                var list = (from cache in CacheList
                            where ((cache.Location.Latitude <= latmax) &&
                                   (cache.Location.Latitude >= latmin) &&
                                   (cache.Location.Longitude <= lngmax) &&
                                   (cache.Location.Longitude >= lngmin))
                            select cache).ToList<Cache>();
                processCacheList(list);
            };
            client.DownloadStringAsync(new Uri(sUrl));
        }

        /// <summary>
        /// Downloads data at the url by cacheId
        /// </summary>
        /// <param name="processData">processes downloaded result</param>
        private static void DownloadAndProcessData(string url, Action<string> processData, int cacheId)
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
            webClient.Encoding = CP1251Encoding;
            webClient.DownloadStringAsync(new Uri(String.Format(url, cacheId), UriKind.Absolute));
        }

        /// <summary>
        /// Downloads cache info by cacheId
        /// </summary>
        /// <param name="processCacheInfo">processes downloaded result</param>
        public void DownloadAndProcessInfo(Action<string> processCacheInfo, int cacheId)
        {
            DownloadAndProcessData(InfoUrl, processCacheInfo, cacheId);
        }

        /// <summary>
        /// Downloads cache notebook by cacheId
        /// </summary>
        /// <param name="processCacheInfo">processes downloaded result</param>
        public void DownloadAndProcessNotebook(Action<string> processCacheNotebook, int cacheId)
        {
            DownloadAndProcessData(NotebookUrl, processCacheNotebook, cacheId);
        }

        public Cache GetCacheById(int cacheId)
        {
            foreach (var p in CacheList.Where(p => p.Id == cacheId))
            {
                return p;
            }
            var db = new CacheDataBase();
            return new Cache(db.GetCache(cacheId));
        }

        #region Photo Downloading

        private List<String> photoUrls;
        private List<String> photoNames;
        private List<ImageSource> images;
        private int cacheId;

        public void ProcessPhotos(int cacheId, Action<List<string>> processAction)
        {
            var db = new CacheDataBase();
            var helper = new FileStorageHelper();

            if (this.cacheId != cacheId)
            {
                ResetPhotoCacheData(cacheId);
            }

            if ((db.GetCache(cacheId) != null) && helper.IsPhotosExist(cacheId))
            {
                ProcessPhotosFromIsolatedStorage(cacheId, processAction);
            }
            else
            {
                ProcessPhotosFromWeb(cacheId, processAction);
            }
        }   

        private void ProcessPhotosFromWeb(int cacheId, Action<List<string>> processAction)
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
                InitializeImages(photoUrls.Count);

                if (processAction != null)
                {
                    processAction(photoUrls);
                }
            };

            webClient.AllowReadStreamBuffering = true;
            webClient.Encoding = CP1251Encoding;
            webClient.DownloadStringAsync(new Uri(String.Format(PhotosUrl, cacheId), UriKind.Absolute));
        }

        private void ProcessPhotosFromIsolatedStorage(int cacheId, Action<List<string>> processAction)
        {
            var helper = new FileStorageHelper();

            if (photoNames == null)
            {
                photoNames = helper.GetPhotoNames(cacheId);
            }

            InitializeImages(photoNames.Count);

            if (processAction != null)
            {
                processAction(photoNames);
            }
        }

        private void ResetPhotoCacheData(int cacheId)
        {
            this.cacheId = cacheId;
            photoUrls = photoNames = null;
            images = null;
        }

        private void ResetPhotoUrls(string htmlPhotoUrls)
        {
            if (photoUrls != null)
            {
                return;
            }

            photoUrls = new List<string>();
            var urls = Regex.Matches(htmlPhotoUrls, LinkPattern);

            for (var i = 0; i < urls.Count; i++)
            {
                var url = urls[i].Value.Substring(7, urls[i].Value.Length - 8);

                if (url.EndsWith(".jpg"))
                {
                    photoUrls.Add(url);
                }
            }
        }

        private void InitializeImages(int count)
        {
            if (images != null)
            {
                return;
            }

            images = new List<ImageSource>();

            for (var i = 0; i < count; i++)
            {
                images.Add(null);
            }
        }

        public void SaveAndProcessPhoto(string photoIdentifier, Action<ImageSource, int> processAction, int index)
        {
            if (!IsUrl(photoIdentifier)) //parallel threads error
            {
                return;
            }

            var helper = new FileStorageHelper();
            var photoUri = new Uri(photoIdentifier);
            var fileName = (photoUri.AbsolutePath.Substring(photoUri.AbsolutePath.LastIndexOf("/"))).Substring(1);

            if (helper.IsOnePhotoExists(cacheId, fileName))
            {
                return;
            }

            if (images.Count <= index) // theoretically impossible
            {
                return;
            }

            if (images[index] == null)
            {

                var webClient = new WebClient();
                webClient.OpenReadCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        return;
                    }

                    var writableBitmap = PictureDecoder.DecodeJpeg(e.Result);
                    helper.SavePhoto(cacheId, fileName, writableBitmap);
                    ProcessPhoto(writableBitmap, processAction, index);

                };
                webClient.OpenReadAsync(photoUri);
            }
            else
            {
                var image = (WriteableBitmap) images[index];
                helper.SavePhoto(cacheId, fileName, image);
                ProcessPhoto(image, processAction, index);
            }
        }

        public void LoadAndProcessPhoto(string photoIdentifier, Action<ImageSource, int> processAction, int index)
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

                    if ((new CacheDataBase()).GetCache(cacheId) != null)
                    {
                        var fileName = (photoUri.AbsolutePath.Substring(photoUri.AbsolutePath.LastIndexOf("/"))).Substring(1);
                        helper.SavePhoto(cacheId, fileName, writableBitmap);
                    }

                    ProcessPhoto(writableBitmap, processAction, index);
                };

                webClient.OpenReadAsync(photoUri);
            }
            else
            {
                ProcessPhoto(helper.GetPhoto(cacheId, photoIdentifier), processAction, index);
            }
        }

        private bool IsUrl(string data)
        {
            return data != null && data.Contains("http://");
        }

        private void ProcessPhoto(ImageSource image, Action<ImageSource, int> processAction, int index)
        {
            if (images[index] == null)
            {
                images[index] = image;
            }

            processAction(image, index);
        }

        public void ProcessPhoto(Action<ImageSource> processAction, int index)
        {
            if (images == null)
            {
                processAction(null);
                return;
            }

            var count = images.Count;
            if (count == 0) return;
            index = index % count;

            if (index < 0)
            {
                index += count;
            }

            processAction(images[index]);
        }

        public void DeletePhotos(int cacheId)
        {
            var helper = new FileStorageHelper();
            helper.DeletePhotos(cacheId);
            ResetPhotoDataAfterDeleting();
        }

        private void ResetPhotoDataAfterDeleting()
        {
            photoUrls = photoNames = null;
        }

        #endregion
    }
}