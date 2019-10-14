using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WallpapersBing.Model
{
    class WebImageSource
    {
        string _url;
        public WebImageSource(string url)
        {
            _url = url;
        }

        public images GetImages()
        {
            using var client = new HttpClient();
            string rawData = client.GetStringAsync(_url).GetAwaiter().GetResult();
            XmlDeserializer<images> deserializer = new XmlDeserializer<images>(rawData, "images");
            var res = deserializer.Deserialize();
            return res;
        }
        

    }
}
