using System;
using System.Xml;

namespace XmlTools.XmlMapper
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class XmlMapperNamespaceAttribute : Attribute
    {
        public string Name { get; }
        public string Uri { get; }

        public XmlMapperNamespaceAttribute(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class XmlPathAttribute : Attribute
    {
        private readonly string _xpath;
        public string Xpath => _xpath.TrimEnd('/');
        public bool Required { get; set; } = false;

        public XmlPathAttribute() : this("/")
        {

        }

        public XmlPathAttribute(string xpath)
        {
            _xpath = xpath;
        }

        public string GetValue(string xml)
        {
            return GetValue(xml, string.Empty);
        }

        public string GetValue(string xml, string parent)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            var node = document.SelectSingleNode(parent + Xpath);

            if (node == null)
            {
                throw new XmlMappingNodeNotFoundException(parent + Xpath);
            }
            if (Required && string.IsNullOrWhiteSpace(node.InnerText))
            {
                throw new XmlMappingRequiredException(parent + Xpath);
            }

            return node.InnerText;
        }

        public string SetValue(object value, string destinationXml)
        {
            return SetValue(value, destinationXml, string.Empty);
        }

        public string SetValue(object value, string destinationXml, string parent)
        {
            var document = new XmlDocument();
            document.LoadXml(destinationXml);
            var node = document.SelectSingleNode(parent + Xpath);
            if (node == null)
            {
                throw new XmlMappingNodeNotFoundException(parent + Xpath);
            }
            if (Required && value == null)
            {
                throw new XmlMappingRequiredException(parent + Xpath);
            }

            node.InnerText = value.ToString();
            return document.OuterXml;
        }
    }
}