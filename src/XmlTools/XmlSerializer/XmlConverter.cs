using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace XmlTools.XmlSerializer
{
    public class XmlConverter
    {
        public T FromXml<T>(string xml) where T : class
        {
            var buffer = Encoding.ASCII.GetBytes(xml);
            using (var stream = new MemoryStream(buffer))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return serializer.Deserialize(stream) as T;
            }
        }

        public string ToXml<T>(T obj) where T : class
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                return Encoding.ASCII.GetString(stream.ToArray());
            }
        }

        public string ToXml<T>(T obj, params XmlQualifiedName[] xmlQualifiedNames) where T : class
        {
            using (var stream = new MemoryStream())
            {
                var namespaces = new XmlSerializerNamespaces(xmlQualifiedNames);
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                serializer.Serialize(stream, obj, namespaces);
                return Encoding.ASCII.GetString(stream.ToArray());
            }
        }
    }
}
