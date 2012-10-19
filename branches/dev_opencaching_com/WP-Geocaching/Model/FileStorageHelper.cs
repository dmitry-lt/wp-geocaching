﻿using System;
using System.Linq;
using System.Windows.Media;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;


namespace WP_Geocaching.Model
{
    public class FileStorageHelper
    {
        private const string FilePath = "{0}\\{1}";

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

        public ImageSource GetPhoto(string cacheId, string fileName)
        {
            return GetImage(String.Format(FilePath, cacheId, fileName));
        }

        public bool IsOnePhotoExists(string cacheId, string fileName)
        {
            var imagePath = String.Format(FilePath, cacheId, fileName);

            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return fileStore.FileExists(imagePath);
            }
        }

        public bool IsPhotosExist(string cacheId)
        {
            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return fileStore.DirectoryExists(cacheId);
            }
        }

        public void SavePhoto(string cacheId, string fileName, WriteableBitmap bitmap)
        {
            CreateCacheDirectories(cacheId);
            var newFilePath = String.Format(FilePath, cacheId, fileName);

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

        private void CreateCacheDirectories(string cacheId)
        {
            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (fileStore.DirectoryExists(cacheId))
                {
                    return;
                }

                fileStore.CreateDirectory(cacheId);
            }
        }

        public List<string> GetPhotoNames(string cacheId)
        {
            if (!IsPhotosExist(cacheId))
            {
                return null;
            }

            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var filePattern = String.Format(FilePath, cacheId, "*.*");
                return fileStore.GetFileNames(filePattern).ToList();
            }
        }

        public void DeletePhotos(string cacheId)
        {
            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var photos = GetPhotoNames(cacheId);

                if (photos == null)
                {
                    return;
                }

                foreach (var p in photos)
                {
                    fileStore.DeleteFile(String.Format(FilePath, cacheId, p));
                }

                fileStore.DeleteDirectory(cacheId.ToString());
            }
        }
    }
}