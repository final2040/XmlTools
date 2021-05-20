using System.Xml;

namespace XmlTools.XmlExtensions
{
    public static class XmlDocumentExtensions
    {
        public static string GetValue(this XmlDocument document, string xPath)
        {
            return document.GetNode(xPath)?.InnerText;
        }

        public static XmlNode GetNode(this XmlDocument document, string xPath)
        {
            return document.SelectSingleNode(xPath);
        }
    }
}