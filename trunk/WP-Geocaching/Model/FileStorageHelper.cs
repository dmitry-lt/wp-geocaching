using System;
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
        private const string filePath = "{0}\\{1}";
        private const string previewSubdirectory = "{0}/preview";
        private const string fullsizeSubdirectory = "{0}/follsize";
        
        public void SaveImage(string imagePath, WriteableBitmap bitmap)
        {
            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (fileStore.FileExists(imagePath))
            {
                fileStore.DeleteFile(imagePath);
            }
            var myFileStream = fileStore.CreateFile(imagePath);

            //85 - quality, maybe this parameter should be added in settings
            bitmap.SaveJpeg(myFileStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 85);
            myFileStream.Close();
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

        public ImageSource GetPreviewImage(int cacheId, string fileName)
        {
            return GetImage(String.Format(filePath, String.Format(previewSubdirectory, cacheId), fileName));
        }

        public ImageSource GetFullsizeImage(int cacheId, string fileName)
        {
            return GetImage(String.Format(filePath, String.Format(fullsizeSubdirectory, cacheId), fileName));
        }

        public bool IsPreviewExists(int cacheId, string fileName)
        {
            string imagePath = String.Format(filePath, String.Format(previewSubdirectory, cacheId), fileName.Substring(1));
            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            return fileStore.FileExists(imagePath);
        }

        public bool IsFullsizeExists(int cacheId, string fileName)
        {
            var imagePath = String.Format(filePath, String.Format(fullsizeSubdirectory, cacheId), fileName.Substring(1));
            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            return fileStore.FileExists(imagePath);
        }

        public bool IsPhotosExists(int cacheId)
        {
            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            return fileStore.DirectoryExists(cacheId.ToString());
        }

        public void SavePreviewImage(int cacheId, string fileName, WriteableBitmap bitmap)
        {
            CreateCacheDirectories(cacheId);
            var newFilePath = String.Format(filePath, String.Format(previewSubdirectory, cacheId), fileName.Substring(1));
            SaveImage(newFilePath, bitmap);
        }

        public void SaveFullsizeImage(int cacheId, string fileName, WriteableBitmap bitmap)
        {
            CreateCacheDirectories(cacheId);
            var newFilePath = String.Format(filePath, String.Format(fullsizeSubdirectory, cacheId), fileName.Substring(1));
            SaveImage(newFilePath, bitmap);
        }

        private void CreateCacheDirectories(int cacheId)
        {
            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (fileStore.DirectoryExists(cacheId.ToString())) return;
            fileStore.CreateDirectory(cacheId.ToString());
            fileStore.CreateDirectory(String.Format(previewSubdirectory, cacheId));
            fileStore.CreateDirectory(String.Format(fullsizeSubdirectory, cacheId));
        }

        public void SavePictureInMediaLibrary(int cacheId, string fileName)
        {
            var path = String.Format(filePath, cacheId, fileName.Substring(1));

            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            var myFileStream = fileStore.CreateFile(path);
            myFileStream = fileStore.OpenFile(path, FileMode.Open, FileAccess.Read);

            //Add the JPEG file to the photos library on the device.
            var library = new MediaLibrary();
            var pic = library.SavePicture(fileName.Substring(1), myFileStream);

            myFileStream.Close();
        }

        public List<string> GetPreviewNames(int cacheId)
        {
            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            var filePattern = String.Format(previewSubdirectory, cacheId) + "\\*.*";
            return fileStore.GetFileNames(filePattern).ToList();
        }

        public List<string> GetFullsizeNames(int cacheId)
        {
            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            var filePattern = String.Format(fullsizeSubdirectory, cacheId) + "\\*.*";
            return fileStore.GetFileNames(filePattern).ToList();
        }
    }
}
