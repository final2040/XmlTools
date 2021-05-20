using System;

namespace XmlTools.XmlMapper
{
    public class XmlMappingException : Exception
    {
        public string NodePath { get; protected set; }

        public XmlMappingException(string nodePath)
        {
            NodePath = nodePath;
        }

        public XmlMappingException(string nodePath, string message):base(message)
        {
            NodePath = nodePath;
        }

        public XmlMappingException(string nodePath, Exception innerException) : base("", innerException)
        {
            NodePath = nodePath;
        }

        public XmlMappingException(string nodePath, string message, Exception innerException): base(message, innerException)
        {
            NodePath = nodePath;
        }

        public override string Message { get => $"{Message}, {NodePath}."; }
        public override string ToString()
        {
            return $"{Message}" + Environment.NewLine +
                   $"NodePath: {NodePath}" + Environment.NewLine;
        }
    }
}