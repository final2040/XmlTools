using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml;

namespace XmlTools.XmlMapper
{
    internal class Property
    {
        private readonly string _parentNode;
        private readonly PropertyInfo _property;
        private readonly object _context;
        private XmlPathAttribute _xmlPath;
        private Dictionary<string, string> _namespaces;

        public bool IsUserClass => _property.PropertyType.IsClass && !_property.PropertyType.FullName.StartsWith("System.");

        public string XmlPath => _xmlPath.Xpath;

        public bool IsNull => GetValue() == null;

        public Property(PropertyInfo property, object context, string parentNode)
        {
            _parentNode = parentNode;
        }

        public Property(PropertyInfo property, object context, string parentNode, Dictionary<string, string> namespaces) : this(property, context, parentNode)
        {
            Debug.Assert(property != null);
            Debug.Assert(context != null);

            _property = property;
            _context = context;
            _xmlPath = _property.GetCustomAttribute(typeof(XmlPathAttribute)) as XmlPathAttribute;
            _namespaces = namespaces;
        }

        public string WriteTo(string xml)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(xml));

            return _xmlPath != null ? WriteXml(xml) : xml;
        }

        private string WriteXml(string destinationXml)
        {
            var document = new XmlDocument();
            document.LoadXml(destinationXml);
            var xmlnsManager = new XmlNamespaceManager(document.NameTable);
            foreach (var ns in _namespaces)
            {
                xmlnsManager.AddNamespace(ns.Key, ns.Value);
            }

            var node = document.SelectSingleNode(_parentNode + _xmlPath.Xpath, xmlnsManager);
            if (node == null)
            {
                throw new XmlMappingNodeNotFoundException(_parentNode + _xmlPath.Xpath);
            }
            if (_xmlPath.Required && GetValue() == null)
            {
                throw new XmlMappingRequiredException(_parentNode + _xmlPath.Xpath);
            }

            node.InnerText = GetValue().ToString();
            return document.OuterXml;
        }

        public void ReadFrom(string xml)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(xml));

            try
            {
                if (_xmlPath != null)
                {
                    var nodeContent = ReadXml(xml);
                    var propertyType = GetPropertyType();
                    if (string.IsNullOrWhiteSpace(nodeContent) && propertyType.IsValueType)
                    {
                        SetValue(Activator.CreateInstance(propertyType));
                    }
                    else
                    {
                        SetValue(Convert.ChangeType(nodeContent, propertyType));
                    }
                    
                }
            }
            catch (FormatException e)
            {
                throw new XmlMappingInvalidFormatException(_xmlPath.Xpath, e);
            }
        }

        private string ReadXml(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);

            var xmlnsManager = new XmlNamespaceManager(document.NameTable);
            foreach (var ns in _namespaces)
            {
                xmlnsManager.AddNamespace(ns.Key, ns.Value);
            }

            var node = document.SelectSingleNode(_parentNode + _xmlPath.Xpath, xmlnsManager);

            if (node == null && _xmlPath.Required)
            {
                throw new XmlMappingNodeNotFoundException(_parentNode + _xmlPath.Xpath);
            }
            if (_xmlPath.Required && string.IsNullOrWhiteSpace(node.InnerText))
            {
                throw new XmlMappingRequiredException(_parentNode + _xmlPath.Xpath);
            }

            if (node == null && !_xmlPath.Required)
            {
                return string.Empty;
            }

            return node.InnerText;
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