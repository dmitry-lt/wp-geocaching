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
using Microsoft.Xna.Framework.Media;


namespace WP_Geocaching.Model
{
    public class FileStorageHelper
    {
        private const string filePath = "{0}\\{1}";
        private IsolatedStorageFile fileStore;

        public void SaveImage(int cacheId, string fileName, WriteableBitmap bitmap)
        {
            string newFilePath = String.Format(filePath, cacheId, fileName.Substring(1));
            fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            fileStore.CreateDirectory(cacheId.ToString());
            if (fileStore.FileExists(newFilePath))
            {
                fileStore.DeleteFile(newFilePath);
            }
            IsolatedStorageFileStream myFileStream = fileStore.CreateFile(newFilePath);

            //85 - quality, maybe this parameter should be added in settings
            Extensions.SaveJpeg(bitmap, myFileStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 85);
            myFileStream.Close();
        }

        public bool IsFileExists(int cacheId, string fileName)
        {
            string newFilePath = String.Format(filePath, cacheId, fileName.Substring(1));
            fileStore = IsolatedStorageFile.GetUserStoreForApplication();
            return fileStore.FileExists(newFilePath);
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
    }
}
