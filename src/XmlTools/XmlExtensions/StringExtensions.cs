using System.Xml;

namespace XmlTools.XmlExtensions
{
    public static class StringExtensions
    {
        public static XmlDocument CreateXmlDocument(this string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            return xmlDocument;
        }
    }
}