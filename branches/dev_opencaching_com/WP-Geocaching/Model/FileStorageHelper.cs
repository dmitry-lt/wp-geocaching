using System;
using System.Linq;
using System.Windows.Media;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace WP_Geocaching.Model
{
    public class FileStorageHelper
    {
        private const string FolderPath = "{0}\\{1}";
        private const string FilePath = "{0}\\{1}\\{2}";

        private string GetFolderPath(Cache cache)
        {
            return String.Format(FolderPath, cache.CacheProvider, cache.Id);
        }

        private string GetFilePath(Cache cache, string photoUrl)
        {
            var fileName = photoUrl.Substring(1 + photoUrl.LastIndexOf("/", StringComparison.Ordinal));
            return String.Format(FilePath, cache.CacheProvider, cache.Id, fileName);
        }

        public ImageSource GetImage(string imagePath)
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!store.FileExists(imagePath)) return null;
                    using (var isfs = store.OpenFile(imagePath, FileMode.Open))
                    {
                        if (isfs.Length > 0)
                        {
                            var image = new BitmapImage();
                            image.SetSource(isfs);
                            isfs.Close();
                            return image;
                        }
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                //ToDo do something
                return null;
            }
        }

        public ImageSource GetPhoto(Cache cache, string photoUrl)
        {
            return GetImage(GetFilePath(cache, photoUrl));
        }

        public bool IsOnePhotoExists(Cache cache, string photoUrl)
        {
            var imagePath = GetFilePath(cache, photoUrl);

            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return fileStore.FileExists(imagePath);
            }
        }

        public bool IsPhotosExist(Cache cache)
        {
            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return fileStore.DirectoryExists(GetFolderPath(cache));
            }
        }

        public void SavePhoto(Cache cache, string photoUrl, WriteableBitmap bitmap)
        {
            CreateCacheDirectories(cache);
            var newFilePath = GetFilePath(cache, photoUrl);

            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (fileStore.FileExists(newFilePath))
                {
                    fileStore.DeleteFile(newFilePath);
                }
                var myFileStream = fileStore.CreateFile(newFilePath);

                //85 - quality, maybe this parameter should be added in settings
                bitmap.SaveJpeg(myFileStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 85);
                myFileStream.Close();
            }
        }

        private void CreateCacheDirectories(Cache cache)
        {
            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (fileStore.DirectoryExists(GetFolderPath(cache)))
                {
                    return;
                }

                fileStore.CreateDirectory(GetFolderPath(cache));
            }
        }

        public List<string> GetPhotoNames(Cache cache)
        {
            if (!IsPhotosExist(cache))
            {
                return null;
            }

            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var filePattern = String.Format(FilePath, cache, "*.*");
                return fileStore.GetFileNames(filePattern).ToList();
            }
        }

        public void DeletePhotos(Cache cache)
        {
            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var photos = GetPhotoNames(cache);

                if (photos == null)
                {
                    return;
                }

                foreach (var p in photos)
                {
                    fileStore.DeleteFile(GetFilePath(cache, p));
                }

                fileStore.DeleteDirectory(GetFolderPath(cache));
            }
        }
    }
}
