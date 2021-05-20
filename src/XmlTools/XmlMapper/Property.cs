using System;
using System.Diagnostics;
using System.Reflection;

namespace XmlTools.XmlMapper
{
    internal class Property
    {
        private readonly string _parentNode;
        private readonly PropertyInfo _property;
        private readonly object _context;
        private XmlPathAttribute _xmlPath;

        public bool IsUserClass => _property.PropertyType.IsClass && !_property.PropertyType.FullName.StartsWith("System.");

        public string XmlPath => _xmlPath.Xpath;

        public bool IsNull => GetValue() == null;

        public Property(PropertyInfo property, object context, string parentNode):this(property, context)
        {
            _parentNode = parentNode;
        }

        public Property(PropertyInfo property, object context)
        {
            Debug.Assert(property != null);
            Debug.Assert(context != null);

            _property = property;
            _context = context;
            _xmlPath = _property.GetCustomAttribute(typeof(XmlPathAttribute)) as XmlPathAttribute;
        }

        public string WriteTo(string xml)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(xml));

            return _xmlPath != null ? _xmlPath.SetValue(GetValue(), xml, _parentNode) : xml;
        }

        public void ReadFrom(string xml)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(xml));
            try
            {
                if (_xmlPath != null)
                {
                    SetValue(Convert.ChangeType(_xmlPath.GetValue(xml, _parentNode), GetPropertyType()));
                }
            }
            catch (FormatException e)
            {
                throw new XmlMappingInvalidFormatException(_xmlPath.Xpath, e);
            }
        }
        
        public object GetValue()
        {
            return _property.GetValue(_context);
        }

        public Type GetPropertyType()
        {
            return _property.PropertyType;
        }

        public void SetValue(object value)
        {
            _property.SetValue(_context, value);
        }

        public string GetNodePath()
        {
            return _xmlPath.Xpath;
        }
    }
}