﻿using System;
using System.Globalization;
using System.Windows.Controls;

namespace CarPrice.Validation
{
    internal sealed class MileageEnginePowerVolumeValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(value as string, out var maybeMileage)) return new(false, "Значение в поле должно быть числом");

            if (maybeMileage < 0) return new(false, "Значение в поле должно быть положительным числом");

            return new(true, null);
        }
    }
}
