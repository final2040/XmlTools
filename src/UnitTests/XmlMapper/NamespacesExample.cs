using XmlTools.XmlMapper;

namespace UnitTests.XmlMapper
{
    [XmlMapperNamespace("p", "http://exampleNamespace.com/")]
    [XmlMapperNamespace("i", "http://exampleNamespace2")]
    class NamespacesExample
    {
        
        [XmlPath("/p:example/p:file/firstName")] 
        public string FirstName { get; set; }

        [XmlPath("/p:example/p:file/lastName")]
        public string LastName { get; set; }

        [XmlPath("/p:example/p:file/age")]
        public int Age { get; set; }

        [XmlPath("/p:example/p:file/i:adrress/street")]
        public string Street { get; set; }

        [XmlPath("/p:example/p:file/i:adrress/number")]
        public string Number { get; set; }

        [XmlPath("/p:example/p:file/i:adrress/city")]
        public string City { get; set; }

        public static NamespacesExample CreateSample()
        {
            return new NamespacesExample()
            {
                FirstName = "Raul",
                LastName = "Dominguez",
                Age = 25,
                City = "Nuevo Mexico",
                Number = "25A",
                Street = "Tunas"
            };
        }
    }
}