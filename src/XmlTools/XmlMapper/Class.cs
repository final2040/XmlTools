using System;
using System.Collections.Generic;

namespace XmlTools.XmlMapper
{
    internal class Class
    {
        private readonly string _parentNode = string.Empty;
        private readonly IList<Property> _properties = new List<Property>();
        private object _context;

        public Class(object context, string parentNode)
        {
            _context = context;
            _parentNode = parentNode;
            ReadProperties();
        }

        public Class(object context):this(context, String.Empty)
        {
            
        }

        public void ReadProperties()
        {
            var type = _context.GetType();
            foreach (var property in type.GetProperties())
            {
                _properties.Add(new Property(property, _context, _parentNode));
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
                    new Class(instance, property.XmlPath).ReadXml(SourceXml);
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
                    destinationXml = new Class(property.GetValue(), property.XmlPath).WriteXml(destinationXml);
                }
                else if(!property.IsNull)
                {
                    destinationXml = property.WriteTo(destinationXml);
                }
            }

            return destinationXml;
        }
    }
}