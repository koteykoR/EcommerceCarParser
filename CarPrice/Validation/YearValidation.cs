using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using static System.String;
using static System.Char;

namespace CarPrice.Validation
{
    internal sealed class YearValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var maybeYear = value as string;

            return maybeYear switch
            {
                string when IsNullOrWhiteSpace(maybeYear) => new(false, "Значение в поле не может быть пустой"),
                string when maybeYear.Length != 4 => new(false, "Необходима 4 символа"),
                string when !maybeYear.All(c => IsDigit(c)) => new(false, "Некорректные символы"),
                _ => new(true, null)
            };
        }
    }
}
