using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static System.Char;
using System.Windows.Controls;

namespace CarPrice.Validation
{
    internal sealed class EngineVolumeValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var valueForParse = value as string;

            if (!string.IsNullOrEmpty(valueForParse) && !IsDouble(ref valueForParse)) return new(false, "Значение в поле должно быть числом");

            return new(true, null);
        }

        private bool IsDouble(ref string value)
        {
            var valueSplitDot = value.Split('.');

            if (valueSplitDot.Length != 2) return value.All(c => IsDigit(c));

            return valueSplitDot.First().All(c => IsDigit(c)) && valueSplitDot.Last().All(c => IsDigit(c));
        }
    }
}
