using System;
using System.Xml;
using NUnit.Framework;
using XmlTools.XmlMapper;

namespace UnitTests.XmlMapper
{
    public class XmlMapperTests
    {
        private Destination _source;

        public const string Xml =
            @"
                <file>
	                <firstName>Gabriel</firstName>
	                <lastName>Molina</lastName>
	                <address>Alfredo Vargas #36</address>
                    <age>22</age>
                    <city>Mexico</city>
                    <country></country>
                    <state>Mexico</state>
                    <number>1</number>
                    <dateOfBirth>24-09-2002</dateOfBirth>
                    <nested>
                        <age>12</age>
                        <lastName>Ferrer</lastName>
                    </nested>
                </file>";

        public const string BadXml =
            @"
                <file>
	                <firstName>Gabriel</firstName>
	                <lastName>Molina</lastName>
	                <address>Alfredo Vargas #36</address>
                    <age>22asd</age>
                    <city></city>
                    <country></country>
                    <number>1</number>
                    <dateOfBirth>24-09-2002</dateOfBirth>
                    <nested>
                        <age>12</age>
                        <lastName>Ferrer</lastName>
                    </nested>
                </file>";

        public const string BadXml1 =
            @"
                <file>
	                <firstName>Gabriel</firstName>
	                <lastName>Molina</lastName>
	                <address>Alfredo Vargas #36</address>
                    <age>22</age>
                    <city>mexico</city>
                    <country></country>
                    <number>1</number>
                    <dateOfBirth>24-09-2002</dateOfBirth>
                    <nested>
                        <age>12</age>
                        <lastName>Ferrer</lastName>
                    </nested>
                </file>";

        [SetUp]
        public void Setup()
        {
            _source = new Destination()
            {
                Age = 12,
                BirthDay = new DateTime(1989, 05, 12),
                City = "Kansas city",
                Country = "United States",
                Name = "Richard",
                NestedObject = new Nested()
                {
                    Number = 43,
                    NestedObject = new Nested2()
                    {
                        LastName = "Mendez"
                    }
                },
                State = "Kansas",
                Nested2 = new Nested3()
                {
                    Age = 34,
                    LastName = "Lavariega"
                }
            };
        }

        [Test]
        public void Map_ShouldMapNodesToProperties()
        {
            // arrange
            var mapper = new Mapper();
          

            // act
            var result = mapper.Map<Destination>(Xml);

            // assert
            Assert.That(result.Name, Is.EqualTo("Gabriel"));
        }

        [Test]
        public void Map_ShouldConvertNodeContentToPropertyType()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var result = mapper.Map<Destination>(Xml);

            // assert
            Assert.That(result.Age, Is.EqualTo(22));
        }

        [Test]
        public void Map_WhenDoesntMatchPropertyType_ShouldThrowInvalidTypeException()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = Assert.Throws<XmlMappingInvalidFormatException>(() => mapper.Map<Destination>(BadXml));

            // assert
            Assert.That(actual.NodePath, Is.EqualTo("/file/age"));
        }

        [Test]
        public void Map_WhenNodeIsEmptyAndRequiredOptionIsTrue_ShouldThrowException()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = Assert.Throws<XmlMappingInvalidFormatException>(() => mapper.Map<Destination>(BadXml));

            // assert
            Assert.That(actual.NodePath, Is.EqualTo("/file/age"));
        }

        [Test]
        public void Map_WhenNodeIsEmptyAndRequiredOptionIsFalse_ShouldSetEmptyProperty()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var result = mapper.Map<Destination>(Xml);

            // assert
            Assert.That(result.Country, Is.Empty);
        }


        [Test]
        public void Map_WhenNodeDoesntExists_ShouldThrowNodeNotFoundException()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = Assert.Throws<XmlMappingNodeNotFoundException>(() => mapper.Map<Destination>(BadXml1));

            // assert
            Assert.That(actual.NodePath, Is.EqualTo("/file/state"));
        }

        [Test]
        public void Map_WhenNestedObjectIsDetectedShouldMapNestedObject()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = mapper.Map<Destination>(Xml);

            // assert
            Assert.That(actual.NestedObject.Number, Is.EqualTo(1));
        }

        [Test]
        public void Map_WhenNestedObjectIsDetectedShouldMapNestedRecursively()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = mapper.Map<Destination>(Xml);

            // assert
            Assert.That(actual.NestedObject.NestedObject.LastName, Is.EqualTo("Molina"));
        }

        [Test]
        public void Map_WhenPropertyIsDateShouldMapDate()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = mapper.Map<Destination>(Xml);

            // assert
            Assert.That(actual.BirthDay, Is.EqualTo(new DateTime(2002,09,24)));
        }

        [Test]
        public void MapToXml_ShouldWriteMapValuesToXml()
        {
            // arrange
            var mapper = new Mapper();
            

            // act
            var actual = mapper.Map(_source, Xml);

            // assert
            Assert.That(GetNodeValue(actual, "/file/firstName"), Is.EqualTo(_source.Name));
            Assert.That(GetNodeValue(actual, "/file/age"), Is.EqualTo(_source.Age.ToString()));
            Assert.That(GetNodeValue(actual, "/file/city"), Is.EqualTo(_source.City));
            Assert.That(GetNodeValue(actual, "/file/country"), Is.EqualTo(_source.Country));
            Assert.That(GetNodeValue(actual, "/file/state"), Is.EqualTo(_source.State));
        }

        [Test]
        public void MapToXml_ShouldWriteNestedObjectRecursively()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = mapper.Map(_source, Xml);

            // assert
            Assert.That(GetNodeValue(actual, "/file/number"), Is.EqualTo(_source.NestedObject.Number.ToString()));
            Assert.That(GetNodeValue(actual, "/file/lastName"), Is.EqualTo(_source.NestedObject.NestedObject.LastName));
            Assert.That(GetNodeValue(actual, "/file/nested/age"), Is.EqualTo(_source.Nested2.Age.ToString()));
            Assert.That(GetNodeValue(actual, "/file/nested/lastName"), Is.EqualTo(_source.Nested2.LastName));

        }

        public string GetNodeValue(string xml, string xPath)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            return document.SelectSingleNode(xPath)?.InnerText;
        }
    }

    class Destination
    {
        [XmlPath("/file/firstName")]
        public string Name { get; set; }

        [XmlPath("/file/age")]
        public int Age { get; set; }

        [XmlPath("/file/city", Required = true)]
        public string City { get; set; }

        [XmlPath("/file/country")]
        public string Country { get; set; }

        
        [XmlPath("/file/state")]
        public string State { get; set; }
        [XmlPath("/file/dateOfBirth")]
        public DateTime BirthDay { get; set; }

        public string NotMapped { get; set; }
        [XmlPath()]
        public Nested NestedObject { get; set; }

        [XmlPath("/file/nested")]
        public Nested3 Nested2 { get; set; }
    }
    
    public class Nested
    {
        [XmlPath("/file/number")]
        public int Number { get; set; }
        [XmlPath()]
        public Nested2 NestedObject { get; set; }
    }

    public class Nested2
    {
        [XmlPath("/file/lastName")]
        public string LastName { get; set; }
    }
    public class Nested3
    {
        [XmlPath("/lastName")]
        public string LastName { get; set; }

        [XmlPath("/age")]
        public int Age { get; set; }
    }
}
