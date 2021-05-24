using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XmlTools.XmlValidator
{
    public class XmlValidator
    {
        private readonly XmlSchemaSet _schemaSet = new XmlSchemaSet();
        private XmlValidationResult _result;

        public XmlValidator(Uri schemaUri) : this(schemaUri, String.Empty)
        {

        }
        public XmlValidator(Uri schemaUri, string targetNamespace)
        {
            _schemaSet.Add(targetNamespace, schemaUri.LocalPath);
        }

        public XmlValidationResult Validate(string xml)
        {
            InitResult();
            var reader = XmlReader.Create(new StringReader(xml));
            Validate(reader);
            return _result;
        }

        public XmlValidationResult Validate(Uri uri)
        {
            InitResult();
            var reader = XmlReader.Create(uri.LocalPath);
            Validate(reader);

            return _result;
        }

        private void Validate(XmlReader reader)
        {
            var document = XDocument.Load(reader);
            document.Validate(_schemaSet, ValidationEventHandler);
        }

        private void InitResult()
        {
            _result = new XmlValidationResult();
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error) _result.AddError(e.Message);
        }
    }
}