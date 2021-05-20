using System;

namespace XmlTools.XmlMapper
{
    public class XmlMappingRequiredException : XmlMappingException
    {
        public override string Message { get => $"Required Node: '{NodePath}' is empty."; }

        public XmlMappingRequiredException(string nodePath) : base(nodePath)
        {
        }

        public XmlMappingRequiredException(string nodePath, string message) : base(nodePath, message)
        {
        }

        public XmlMappingRequiredException(string nodePath, Exception innerException) : base(nodePath, innerException)
        {
        }

        public XmlMappingRequiredException(string nodePath, string message, Exception innerException) : base(nodePath, message, innerException)
        {
        }
    }
}