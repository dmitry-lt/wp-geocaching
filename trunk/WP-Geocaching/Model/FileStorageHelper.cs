using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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
        private IsolatedStorageFile fileStore;

        public void SaveImage(string imagePath, WriteableBitmap bitmap)
        {
            fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (fileStore.FileExists(imagePath))
            {
                fileStore.DeleteFile(imagePath);
            }
            IsolatedStorageFileStream myFileStream = fileStore.CreateFile(imagePath);

            //85 - quality, maybe this parameter should be added in settings
            Extensions.SaveJpeg(bitmap, myFileStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 85);
            myFileStream.Close();
        }

        public ImageSource GetImage(string imagePath)
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store == null) return null;
                    if (!store.FileExists(imagePath)) return null;
                    using (IsolatedStorageFileStream isfs = store.OpenFile(imagePath, FileMode.Open))
                    {
                        if (isfs.Length > 0)
                        {
                            BitmapImage image = new BitmapImage();
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
            fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            return fileStore.FileExists(imagePath);
        }

        public bool IsFullsizeExists(int cacheId, string fileName)
        {
            string imagePath = String.Format(filePath, String.Format(fullsizeSubdirectory, cacheId), fileName.Substring(1));
            fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            return fileStore.FileExists(imagePath);
        }

        public bool IsPhotosExists(int cacheId)
        {
            fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            return fileStore.DirectoryExists(cacheId.ToString());
        }

        public void SavePreviewImage(int cacheId, string fileName, WriteableBitmap bitmap)
        {
            CreateCacheDirectories(cacheId);
            string newFilePath = String.Format(filePath, String.Format(previewSubdirectory, cacheId), fileName.Substring(1));
            SaveImage(newFilePath, bitmap);
        }

        public void SaveFullsizeImage(int cacheId, string fileName, WriteableBitmap bitmap)
        {
            CreateCacheDirectories(cacheId);
            string newFilePath = String.Format(filePath, String.Format(fullsizeSubdirectory, cacheId), fileName.Substring(1));
            SaveImage(newFilePath, bitmap);
        }

        private void CreateCacheDirectories(int cacheId)
        {
            fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (!fileStore.DirectoryExists(cacheId.ToString()))
            {
                fileStore.CreateDirectory(cacheId.ToString());
                fileStore.CreateDirectory(String.Format(previewSubdirectory, cacheId));
                fileStore.CreateDirectory(String.Format(fullsizeSubdirectory, cacheId));
            }
        }

        public void SavePictureInMediaLibrary(int cacheId, string fileName)
        {
            string FilePath = String.Format(filePath, cacheId, fileName.Substring(1));

            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            var myFileStream = fileStore.CreateFile(FilePath);
            myFileStream = fileStore.OpenFile(FilePath, FileMode.Open, FileAccess.Read);

            //Add the JPEG file to the photos library on the device.
            MediaLibrary library = new MediaLibrary();
            Picture pic = library.SavePicture(fileName.Substring(1), myFileStream);

            myFileStream.Close();
        }

        public List<string> GetPreviewNames(int cacheId)
        {
            List<string> previewNames = new List<string>();
            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            var filePattern = String.Format(previewSubdirectory, cacheId) + "\\*.*";
            foreach (var name in fileStore.GetFileNames(filePattern))
            {
                previewNames.Add(name);
            }
            return previewNames;
        }

        public List<string> GetFullsizeNames(int cacheId)
        {
            List<string> fullsizeNames = new List<string>();
            var fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            var filePattern = String.Format(fullsizeSubdirectory, cacheId) + "\\*.*";
            foreach (var name in fileStore.GetFileNames(filePattern))
            {
                fullsizeNames.Add(name);
            }
            return fullsizeNames;
        }
    }
}
