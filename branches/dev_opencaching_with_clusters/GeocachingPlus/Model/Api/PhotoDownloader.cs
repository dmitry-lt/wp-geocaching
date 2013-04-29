using System;
using System.Net;
using System.Windows.Media.Imaging;
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
                            try
                            {
                                photo = PictureDecoder.DecodeJpeg(e.Result);
                            }
                            catch (Exception ex)
                            {
                                // TODO: log
                            }
                        }
                        processPhoto(photo);
                    };
            webClient.OpenReadAsync(new Uri(photoUrl));
        }
    }
}
