using NUnit.Framework;
using System;
using System.IO;

namespace UnitTests.XmlValidator
{
    [TestFixture()]
    public class XmlValidatorTests
    {
        private string _applicationPath;
        private Uri _validTestXmlUri;
        private Uri _testXmlSchemaUri;
        private Uri _invalidTestXmlUri;

        [SetUp()]
        public void Setup()
        {
            _applicationPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            _validTestXmlUri = new Uri(_applicationPath + "\\XmlValidator\\XML\\TestXml.xml");
            _invalidTestXmlUri = new Uri(_applicationPath + "\\XmlValidator\\XML\\InvalidTestXml.xml");
            _testXmlSchemaUri = new Uri(_applicationPath + "\\XmlValidator\\xml\\TestXml.xsd");
        }


        [Test()]
        public void Validate_ShouldReturnValidResultIfXmlIsValid()
        {
            // arrange

            var xmlValidator = new global::XmlTools.XmlValidator.XmlValidator(_testXmlSchemaUri);

            // act
            var actual = xmlValidator.Validate(_validTestXmlUri);

            // assert
            Assert.That(actual.IsValid, Is.True);
        }

        [Test()]
        public void Validate_ShouldReturnInValidResultIfXmlIsInvalid()
        {
            // arrange
            var xmlValidator = new global::XmlTools.XmlValidator.XmlValidator(_testXmlSchemaUri);

            // act
            var actual = xmlValidator.Validate(_invalidTestXmlUri);

            // assert
            Assert.That(actual.IsValid, Is.False);
        }

        [Test()]
        public void Validate_ShouldReturnAddValidationErrorsToValidationResult()
        {
            // arrange
            var xmlValidator = new global::XmlTools.XmlValidator.XmlValidator(_testXmlSchemaUri);

            // act
            var actual = xmlValidator.Validate(_invalidTestXmlUri);

            // assert
            Assert.That(actual.ValidationErrors.Count, Is.EqualTo(2));
        }

        [Test()]
        public void ValidateString_ShouldReturnValidResultIfXmlIsValid()
        {
            // arrange

            var xmlValidator = new global::XmlTools.XmlValidator.XmlValidator(_testXmlSchemaUri);
            var xmlText = File.ReadAllText(_validTestXmlUri.LocalPath);

            // act
            var actual = xmlValidator.Validate(xmlText);

            // assert
            Assert.That(actual.IsValid, Is.True);
        }

        [Test()]
        public void ValidateString_ShouldReturnInValidResultIfXmlIsInvalid()
        {
            // arrange
            var xmlValidator = new global::XmlTools.XmlValidator.XmlValidator(_testXmlSchemaUri);
            var xmlText = File.ReadAllText(_invalidTestXmlUri.LocalPath);

            // act
            var actual = xmlValidator.Validate(xmlText);

            // assert
            Assert.That(actual.IsValid, Is.False);
        }

        [Test()]
        public void ValidateString_ShouldReturnAddValidationErrorsToValidationResult()
        {
            // arrange
            var xmlValidator = new global::XmlTools.XmlValidator.XmlValidator(_testXmlSchemaUri);
            var xmlText = File.ReadAllText(_invalidTestXmlUri.LocalPath);

            // act
            var actual = xmlValidator.Validate(xmlText);

            // assert
            Assert.That(actual.ValidationErrors.Count, Is.EqualTo(2));
        }
    }
}