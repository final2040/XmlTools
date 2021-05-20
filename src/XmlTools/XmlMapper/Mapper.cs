using System;

namespace XmlTools.XmlMapper
{
    public class Mapper
    {
        public Mapper()
        {
        }

        public string Map<T>(T source, string destinationXml)
        {
            return new Class(source).WriteXml(destinationXml);
        }

        public T Map<T>(string xml) where T : class
        {
            var destination = (T)Activator.CreateInstance(typeof(T));
            return new Class(destination).ReadXml(xml) as T;
        }

    }
}
