using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace WallpapersBing.Model
{
    class XmlDeserializer<T>
    {
        string _xmlData;
        string _rootAttribute;

        public XmlDeserializer(string xmlData, string rootAttribute)
        {
            _xmlData = xmlData;
            _rootAttribute = rootAttribute;
        }

        public T Deserialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(_rootAttribute));
            using StringReader stringReader = new StringReader(_xmlData);
            return (T)serializer.Deserialize(stringReader);
        }

    }
}
