using System;
using System.Linq;
using NUnit.Framework;
using XmlTools.XmlValidator;

namespace UnitTests.XmlValidator
{
    [TestFixture()]
    public class XmlValidationResultTests
    {
        [Test()]
        public void IsValid_Should_ReturnTrueIfNoErrors()
        {
            // arrange
            var validationResult = new XmlValidationResult();

            // act
            var actual = validationResult.IsValid;
            
            // assert
            Assert.That(actual, Is.True);
        }

        [Test()]
        public void IsValid_Should_ReturnFalseIfOneOrMoreErrors()
        {
            // arrange
            var validationResult = new XmlValidationResult();
            

            // act
            validationResult.ValidationErrors.Add("Test Error");
            var actual = validationResult.IsValid;

            // assert
            Assert.That(actual, Is.False);
        }

        [Test()]
        public void AddError_Should_AddAnError()
        {
            // arrange
            var validationResult = new XmlValidationResult();

            // act
            validationResult.AddError("Test Error");
            var actual = validationResult.ValidationErrors;

            // assert
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.Any(p => p == "Test Error"), Is.True);
        }

        [Test()]
        public void ToString_Should_ReturnLineBreakSeparatedErrors()
        {
            // arrange
            var validationResult = new XmlValidationResult();
            var error1 = "Error 1";
            var error2 = "Error 2";
            var expected = error1 + Environment.NewLine + error2;

            //act
            validationResult.AddError(error1);
            validationResult.AddError(error2);
            var actual = validationResult.ToString();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}