using XmlTools.XmlSerializer;

namespace XmlTools.XmlExtensions
{

    public static class ObjectExtensions
    {
        private static readonly XmlConverter XmlConverter = new XmlConverter();

        public static string ToXml(this object obj)
        {
            return XmlConverter.ToXml(obj);
        }
    }
}
