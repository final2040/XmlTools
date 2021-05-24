using System.Text;
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

        public static string Beautify(this XmlDocument document)
        {
            StringBuilder builder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(builder, settings))
            {
                document.Save(writer);
            }
            return builder.ToString();
        }
    }
}