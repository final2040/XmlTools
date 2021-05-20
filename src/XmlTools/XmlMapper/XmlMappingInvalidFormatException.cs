using System;

namespace XmlTools.XmlMapper
{
    public class XmlMappingInvalidFormatException : XmlMappingException
    {
       public override string Message { get => $"The content of the nodePath: '{NodePath}' doesn't match with property type"; }

       public XmlMappingInvalidFormatException(string nodePath) : base(nodePath)
       {
       }

       public XmlMappingInvalidFormatException(string nodePath, string message) : base(nodePath, message)
       {
       }

       public XmlMappingInvalidFormatException(string nodePath, Exception innerException) : base(nodePath, innerException)
       {
       }

       public XmlMappingInvalidFormatException(string nodePath, string message, Exception innerException) : base(nodePath, message, innerException)
       {
       }
    }
}