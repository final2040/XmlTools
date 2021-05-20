using System;
using System.Collections.Generic;
using System.Reflection;

namespace XmlTools.XmlMapper
{
    internal class Class
    {
        private readonly string _parentNode = string.Empty;
        private readonly IList<Property> _properties = new List<Property>();
        private Dictionary<string, string> _namespaces = new Dictionary<string, string>();

        private object _context;

        public Class(object context, string parentNode)
        {
            _context = context;
            _parentNode = parentNode;
            GetNamespaces();
            ReadProperties();
        }

        private void GetNamespaces()
        {

            var namespaceAttributes = _context.GetType().GetCustomAttributes(typeof(XmlMapperNamespaceAttribute), false);

            foreach (XmlMapperNamespaceAttribute attribute in namespaceAttributes)
            {
                _namespaces.Add(attribute.Name, attribute.Uri);
            }
        }

        public Class(object context) : this(context, String.Empty)
        {

        }

        public void ReadProperties()
        {
            var type = _context.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty))
            {

                _properties.Add(new Property(property, _context, _parentNode, _namespaces));

            }
        }


        public object ReadXml(string SourceXml)
        {
            foreach (var property in _properties)
            {
                if (property.IsUserClass)
                {
                    var instance = Activator.CreateInstance(property.GetPropertyType());
                    property.SetValue(instance);
                    new Class(instance, _parentNode + property.XmlPath).ReadXml(SourceXml);
                }
                else
                {
                    property.ReadFrom(SourceXml);
                }
            }
            return _context;
        }

        public string WriteXml(string destinationXml)
        {
            foreach (var property in _properties)
            {
                if (property.IsUserClass && !property.IsNull)
                {
                    destinationXml = new Class(property.GetValue(), _parentNode + property.XmlPath).WriteXml(destinationXml);
                }
                else if (!property.IsNull)
                {
                    destinationXml = property.WriteTo(destinationXml);
                }
            }

            return destinationXml;
        }
    }
}