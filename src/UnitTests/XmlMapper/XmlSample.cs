using System;
using System.IO;

namespace UnitTests.XmlMapper
{
    class XmlSample
    {
        public static XmlSample ValidXml = new XmlSample("ValidXml.xml");
        public static XmlSample InvalidXml = new XmlSample("InvalidXml.xml");
        public static XmlSample XmlWithNamespaces = new XmlSample("xmlWithNamespaces.xml");
        public static XmlSample EmptyNodeXml = new XmlSample("EmptyNode.xml");
        public static XmlSample EmptyNodeNotRequiredXml = new XmlSample("OptionalEmptyNode.xml");
        public static XmlSample MissingNodeXml = new XmlSample("MissingNode.xml");

        private readonly string _filePath;
        public static implicit operator string(XmlSample xmlSample) => xmlSample.ToString();

        private XmlSample(string fileName)
        {
            var fileUri = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "\\XmlMapper\\Samples\\" + fileName);
            _filePath = fileUri.LocalPath;
        }

        public string Read()
        {
            return File.ReadAllText(_filePath);
        }

        public override string ToString()
        {
            return Read();
        }
    }
}