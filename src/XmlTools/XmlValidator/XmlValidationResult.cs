using System;
using System.Collections.Generic;
using System.Linq;

namespace XmlTools.XmlValidator
{
    public class XmlValidationResult
    {
        public bool IsValid => ValidationErrors.Count <= 0;

        public IList<string> ValidationErrors { get; set; } = new List<string>();

        public void AddError(string message)
        {
            ValidationErrors.Add(message);
        }

        public override string ToString()
        {
            var delimiter = Environment.NewLine;
            if (!IsValid)
            {
                return ValidationErrors.Aggregate((a, b) => a + delimiter + b);
            }
            return string.Empty;
        }
    }
}