using NUnit.Framework;
using System;
using System.Xml;
using XmlTools.XmlMapper;

namespace UnitTests.XmlMapper
{
    public class XmlMapperTests
    {


        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Map_ShouldMapNodesToProperties()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var result = mapper.Map<Example>(XmlSample.ValidXml);

            // assert
            Assert.That(result.SchoolName, Is.EqualTo("Abraham Lincoln"));
            Assert.That(result.SchoolState, Is.EqualTo("Alabama"));
            Assert.That(result.SchoolNumber, Is.EqualTo(32));
        }

        [Test]
        public void Map_whenClassIneritsMappedClass_ShouldMapNodesToProperties()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var result = mapper.Map<InheritedExample>(XmlSample.ValidXml);

            // assert
            Assert.That(result.SchoolName, Is.EqualTo("Abraham Lincoln"));
            Assert.That(result.SchoolState, Is.EqualTo("Alabama"));
            Assert.That(result.SchoolNumber, Is.EqualTo(32));
            Assert.That(result.SomeProperty, Is.EqualTo("asdfd"));
        }

        [Test]
        public void Map_ShouldMapNodesWithNamespaceToProperties()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var result = mapper.Map<NamespacesExample>(XmlSample.XmlWithNamespaces);

            // assert
            Assert.That(result.Age, Is.EqualTo(25));
            Assert.That(result.City, Is.EqualTo("Los alamos"));
            Assert.That(result.FirstName, Is.EqualTo("Jorge"));
            Assert.That(result.LastName, Is.EqualTo("Gomez"));
            Assert.That(result.Number, Is.EqualTo("15"));
            Assert.That(result.Street, Is.EqualTo("Nort 11"));
        }

        [Test]
        public void Map_ShouldConvertNodeContentToPropertyType()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var result = mapper.Map<Example>(XmlSample.ValidXml);

            // assert
            Assert.That(result.SchoolNumber, Is.EqualTo(32));
        }

        [Test]
        public void Map_WhenNodeIsEmptyAndPropertyIsPrimitive_ShouldAssignDefaultValueToProperty()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var result = mapper.Map<Example>(XmlSample.ValidXml);

            // assert
            Assert.That(result.PrimitiveNode, Is.EqualTo(default(int)));
        }

        [Test]
        public void Map_WhenDoesntMatchPropertyType_ShouldThrowInvalidTypeException()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = Assert.Throws<XmlMappingInvalidFormatException>(() => mapper.Map<Example>(XmlSample.InvalidXml));

            // assert
            Assert.That(actual.NodePath, Is.EqualTo("/example/schoolNumber"));
        }

        [Test]
        public void Map_WhenNodeIsEmptyAndRequiredOptionIsTrue_ShouldThrowException()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = Assert.Throws<XmlMappingRequiredException>(() => mapper.Map<Example>(XmlSample.EmptyNodeXml));

            // assert
            Assert.That(actual.NodePath, Is.EqualTo("/example/school"));
        }

        [Test]
        public void Map_WhenNodeIsEmptyAndRequiredOptionIsFalse_ShouldSetEmptyProperty()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var result = mapper.Map<Example>(XmlSample.EmptyNodeNotRequiredXml);

            // assert
            Assert.That(result.SchoolState, Is.Empty);
        }


        [Test]
        public void Map_WhenNodeDoesntExists_ShouldThrowNodeNotFoundException()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = Assert.Throws<XmlMappingNodeNotFoundException>(() => mapper.Map<Example>(XmlSample.MissingNodeXml));

            // assert
            Assert.That(actual.NodePath, Is.EqualTo("/example/schoolNumber"));
        }

        [Test]
        public void Map_WhenNestedObjectIsDetectedShouldMapNestedObject()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = mapper.Map<Example>(XmlSample.ValidXml);

            // assert
            Assert.That(actual.Alumn.FirstName, Is.EqualTo("Juan"));
            Assert.That(actual.Alumn.LastName, Is.EqualTo("Perez"));
            Assert.That(actual.Alumn.Age, Is.EqualTo(19));
            Assert.That(actual.Parent.FirstName, Is.EqualTo("Rosa"));
            Assert.That(actual.Parent.LastName, Is.EqualTo("Perez"));
            Assert.That(actual.Parent.Age, Is.EqualTo(43));
        }

        [Test]
        public void Map_WhenNestedObjectIsDetectedShouldMapNestedRecursively()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = mapper.Map<Example>(XmlSample.ValidXml);

            // assert
            Assert.That(actual.Alumn.Address.Street, Is.EqualTo("1st Street"));
            Assert.That(actual.Alumn.Address.Number, Is.EqualTo("25A"));
            Assert.That(actual.Alumn.Address.Phone.Code, Is.EqualTo("01"));
            Assert.That(actual.Alumn.Address.Phone.Number, Is.EqualTo("555-45258"));
            Assert.That(actual.Parent.Address.Street, Is.EqualTo("2nd Street"));
            Assert.That(actual.Parent.Address.Number, Is.EqualTo("45"));
            Assert.That(actual.Parent.Address.Phone.Code, Is.EqualTo("01"));
            Assert.That(actual.Parent.Address.Phone.Number, Is.EqualTo("555-458780"));
        }

        [Test]
        public void Map_WhenPropertyIsDateShouldMapDate()
        {
            // arrange
            var mapper = new Mapper();


            // act
            var actual = mapper.Map<Example>(XmlSample.ValidXml);

            // assert
            Assert.That(actual.InscriptionDate, Is.EqualTo(new DateTime(2021, 05, 20)));
        }

        [Test]
        public void MapToXml_ShouldWriteMapValuesToXml()
        {
            // arrange
            var mapper = new Mapper();
            var sample = Example.CreateSample();

            // act
            var actual = mapper.Map(sample, XmlSample.ValidXml);

            // assert
            Assert.That(GetNodeValue(actual, "/example/school"), Is.EqualTo(sample.SchoolName));
            Assert.That(GetNodeValue(actual, "/example/schoolNumber"), Is.EqualTo(sample.SchoolNumber.ToString()));
            Assert.That(GetNodeValue(actual, "/example/schoolState"), Is.EqualTo(sample.SchoolState));
        }

        [Test]
        public void MapToXml_WhenClassIneritsMappedClass_ShouldWriteMapValuesToXml()
        {
            // arrange
            var mapper = new Mapper();
            var sample = InheritedExample.CreateSample();

            // act
            var actual = mapper.Map(sample, XmlSample.ValidXml);

            // assert
            Assert.That(GetNodeValue(actual, "/example/school"), Is.EqualTo(sample.SchoolName));
            Assert.That(GetNodeValue(actual, "/example/schoolNumber"), Is.EqualTo(sample.SchoolNumber.ToString()));
            Assert.That(GetNodeValue(actual, "/example/schoolState"), Is.EqualTo(sample.SchoolState));
            Assert.That(GetNodeValue(actual, "/example/someProperty"), Is.EqualTo(sample.SomeProperty));
        }

        [Test]
        public void MapToXml_ShouldWriteNestedObjectRecursively()
        {
            // arrange
            var mapper = new Mapper();

            var sample = Example.CreateSample();

            // act
            var actual = mapper.Map(sample, XmlSample.ValidXml);

            // assert
            Assert.That(GetNodeValue(actual, "/example/alumn/firstName"), Is.EqualTo(sample.Alumn.FirstName));
            Assert.That(GetNodeValue(actual, "/example/alumn/lastName"), Is.EqualTo(sample.Alumn.LastName));
            Assert.That(GetNodeValue(actual, "/example/alumn/age"), Is.EqualTo(sample.Alumn.Age.ToString()));
            Assert.That(GetNodeValue(actual, "/example/alumn/address/street"), Is.EqualTo(sample.Alumn.Address.Street));
            Assert.That(GetNodeValue(actual, "/example/alumn/address/number"), Is.EqualTo(sample.Alumn.Address.Number));
            Assert.That(GetNodeValue(actual, "/example/alumn/address/phone/number"), Is.EqualTo(sample.Alumn.Address.Phone.Number));
            Assert.That(GetNodeValue(actual, "/example/alumn/address/phone/countryCode"), Is.EqualTo(sample.Alumn.Address.Phone.Code));
            Assert.That(GetNodeValue(actual, "/example/parent/firstName"), Is.EqualTo(sample.Parent.FirstName));
            Assert.That(GetNodeValue(actual, "/example/parent/lastName"), Is.EqualTo(sample.Parent.LastName));
            Assert.That(GetNodeValue(actual, "/example/parent/age"), Is.EqualTo(sample.Parent.Age.ToString()));
            Assert.That(GetNodeValue(actual, "/example/parent/address/street"), Is.EqualTo(sample.Parent.Address.Street));
            Assert.That(GetNodeValue(actual, "/example/parent/address/number"), Is.EqualTo(sample.Parent.Address.Number));
            Assert.That(GetNodeValue(actual, "/example/parent/address/phone/number"), Is.EqualTo(sample.Parent.Address.Phone.Number));
            Assert.That(GetNodeValue(actual, "/example/parent/address/phone/countryCode"), Is.EqualTo(sample.Parent.Address.Phone.Code));
        }

        [Test]
        public void Map_ShouldPropertiesWithNamespaceToXml()
        {
            // arrange
            var mapper = new Mapper();
            var expected = NamespacesExample.CreateSample();


            // act
            var actual = mapper.Map(expected, XmlSample.XmlWithNamespaces);

            // assert
            Assert.That(GetNodeValue(actual, "/p:example/p:file/firstName"), Is.EqualTo(expected.FirstName));
            Assert.That(GetNodeValue(actual, "/p:example/p:file/lastName"), Is.EqualTo(expected.LastName));
            Assert.That(GetNodeValue(actual, "/p:example/p:file/age"), Is.EqualTo(expected.Age.ToString()));
            Assert.That(GetNodeValue(actual, "/p:example/p:file/i:adrress/street"), Is.EqualTo(expected.Street));
            Assert.That(GetNodeValue(actual, "/p:example/p:file/i:adrress/number"), Is.EqualTo(expected.Number));
            Assert.That(GetNodeValue(actual, "/p:example/p:file/i:adrress/city"), Is.EqualTo(expected.City));
        }

        public string GetNodeValue(string xml, string xPath)
        {
            var document = new XmlDocument();
            var nsManager = new XmlNamespaceManager(document.NameTable);
            nsManager.AddNamespace("p", "http://exampleNamespace.com/");
            nsManager.AddNamespace("i", "http://exampleNamespace2");

            document.LoadXml(xml);
            return document.SelectSingleNode(xPath, nsManager)?.InnerText;
        }
    }
}
