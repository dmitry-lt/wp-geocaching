using System;
using System.Net;
using System.Windows.Media.Imaging;
using ImageTools;
using ImageTools.IO.Png;
using Microsoft.Phone;

namespace GeocachingPlus.Model.Api
{
    public class PhotoDownloader
    {
        public void DownloadPhoto(Action<WriteableBitmap> processPhoto, string photoUrl)
        {
            if (null == processPhoto)
            {
                return;
            }

            var webClient = new WebClient();
            webClient.OpenReadCompleted += 
                (sender, e) =>
                    {
                        WriteableBitmap photo = null;
                        if (e.Error == null)
                        {
                            photo = PictureDecoder.DecodeJpeg(e.Result);
                        }
                        processPhoto(photo);
                    };
            webClient.OpenReadAsync(new Uri(photoUrl));
        }

        public void DownloadPng(Action<WriteableBitmap> processImage, string photoUrl)
        {
            if (null == processImage)
            {
                return;
            }

            var webClient = new WebClient();
            webClient.OpenReadCompleted +=
                (sender, e) =>
                {
                    if (e.Error == null)
                    {
                        var pngDecoder = new PngDecoder();
                        var ei = new ExtendedImage();
                        pngDecoder.Decode(ei, e.Result);
                        var writeableBitmap = ei.ToBitmap();
                        processImage(writeableBitmap);
                    }
                    else
                    {
                        processImage(null);
                    }
                };
            webClient.OpenReadAsync(new Uri(photoUrl));
        }

    }
}
