using System;
using System.Linq;
using System.Globalization;
using System.Windows.Controls;
using static System.String;
using static System.Char;

namespace CarPrice.Validation
{
    internal sealed class CompanyModelValidation : ValidationRule
    {
        private static bool IsSpaceOrLatinOrDigit(char c) => c is ' ' or >= 'a' and <= 'z' or >= '0' and <= '9';

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var maybeCompanyModel = value as string;

            return maybeCompanyModel switch
            {
                string when IsNullOrWhiteSpace(maybeCompanyModel) => new(false, "Значение в поле не может быть пустой"),
                string when maybeCompanyModel.Length <= 2 => new(false, "Минимальная длина 3 символа"),
                string when !maybeCompanyModel.All(c => IsSpaceOrLatinOrDigit(c)) => new(false, "Некорректные символы"),
                _ => new(true, null)
            };
        }
    }
}
