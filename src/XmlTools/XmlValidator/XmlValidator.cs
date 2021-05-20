using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XmlTools.XmlValidator
{
    public class XmlValidator
    {
        private readonly XmlSchemaSet _schemaSet = new XmlSchemaSet();
        private XmlValidationResult _result;

        public XmlValidator(Uri schemaUri)
        {
            _schemaSet.Add("", schemaUri.LocalPath);
        }

        public XmlValidationResult Validate(string xml)
        {
            InitResult();
            var document = XDocument.Parse(xml);
            document.Validate(_schemaSet, ValidationEventHandler);
            return _result;
        }

        public XmlValidationResult Validate(Uri uri)
        {
            InitResult();
            var reader = XmlReader.Create(uri.LocalPath);
            var document = XDocument.Load(reader);
            document.Validate(_schemaSet, ValidationEventHandler);

            return _result;
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