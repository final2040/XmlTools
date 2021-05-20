using System;

namespace XmlTools.XmlMapper
{
    public class XmlMappingNodeNotFoundException : XmlMappingException
    {
        public override string Message { get => $"Couldn't find nodePath: '{NodePath}'."; }

        public XmlMappingNodeNotFoundException(string nodePath) : base(nodePath)
        {
        }

        public XmlMappingNodeNotFoundException(string nodePath, string message) : base(nodePath, message)
        {
        }

        public XmlMappingNodeNotFoundException(string nodePath, Exception innerException) : base(nodePath, innerException)
        {
        }

        public XmlMappingNodeNotFoundException(string nodePath, string message, Exception innerException) : base(nodePath, message, innerException)
        {
        }
    }
}