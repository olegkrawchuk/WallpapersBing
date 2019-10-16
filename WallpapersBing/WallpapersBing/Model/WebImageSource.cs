using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WallpapersBing.Model
{
    class WebImageSource : IDisposable
    {
        HttpClient httpClient = new HttpClient();
        string _url;

        public WebImageSource(string url)
        {
            _url = url;
        }

        public images GetAllImages()
        {
            //using var client = new HttpClient();
            string rawData = httpClient.GetStringAsync(_url).GetAwaiter().GetResult();
            XmlDeserializer<images> deserializer = new XmlDeserializer<images>(rawData, "images");
            var res = deserializer.Deserialize();
            return res;
        }
        
        public byte[] DownloadImage(string url)
        {
            var responce = httpClient.GetAsync(url).GetAwaiter().GetResult();
            var content = responce.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            responce.Dispose();

            return content;
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
