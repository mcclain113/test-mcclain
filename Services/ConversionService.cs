using System;
using System.Text.RegularExpressions;
using IS_Proj_HIT.Entities.Enum;

namespace IS_Proj_HIT.Services
{
    public static class ConversionService
    {
        /// <summary>
        ///     Converts the source unit to the target 
        /// </summary>
        /// <param name="src">Current unit of the value</param>
        /// <param name="tar">Unit to convert to</param>
        /// <param name="val">Value</param>
        /// <returns>Converted value</returns>
        public static decimal? ConvertLength(LengthUnit src, LengthUnit tar, decimal? val)
        {
            if (src == tar) return val;
            switch (src)
            {
                case LengthUnit.Inches when tar      == LengthUnit.Feet:        return val * 0.0833333333M;
                case LengthUnit.Inches when tar      == LengthUnit.Centimeters: return val * 2.54M;
                case LengthUnit.Inches when tar      == LengthUnit.Meters:      return val * 0.0254M;
                case LengthUnit.Feet when tar        == LengthUnit.Inches:      return val * 12M;
                case LengthUnit.Feet when tar        == LengthUnit.Centimeters: return val * 30.48M;
                case LengthUnit.Feet when tar        == LengthUnit.Meters:      return val * 0.3048M;
                case LengthUnit.Centimeters when tar == LengthUnit.Inches:      return val * 0.3937007874M;
                case LengthUnit.Centimeters when tar == LengthUnit.Feet:        return val * 0.032808399M;
                case LengthUnit.Centimeters when tar == LengthUnit.Meters:      return val * 0.01M;
                case LengthUnit.Meters when tar      == LengthUnit.Inches:      return val * 39.3700787402M;
                case LengthUnit.Meters when tar      == LengthUnit.Feet:        return val * 3.280839895M;
                case LengthUnit.Meters when tar      == LengthUnit.Centimeters: return val * 100M;
                default:
                    throw new ArgumentOutOfRangeException(nameof(src), src, null);
            }
        }

        /// <summary>
        ///     Converts the source unit to the target 
        /// </summary>
        /// <param name="src">Current unit of the value</param>
        /// <param name="tar">Unit to convert to</param>
        /// <param name="val">Value</param>
        /// <returns>Converted value</returns>
        public static decimal? ConvertWeight(WeightUnit src, WeightUnit tar, decimal? val)
        {
            if (src == tar) return val;
            switch (src)
            {
                case WeightUnit.Pounds when tar    == WeightUnit.Grams:     return val * 453.59237M;
                case WeightUnit.Pounds when tar    == WeightUnit.Kilograms: return val * 0.45359237M;
                case WeightUnit.Grams when tar     == WeightUnit.Pounds:    return val * 0.0022046226M;
                case WeightUnit.Grams when tar     == WeightUnit.Kilograms: return val * 0.001M;
                case WeightUnit.Kilograms when tar == WeightUnit.Pounds:    return val * 2.2046226218M;
                case WeightUnit.Kilograms when tar == WeightUnit.Grams:     return val * 1000M;
                default:
                    throw new ArgumentOutOfRangeException(nameof(src), src, null);
            }
        }

        /// <summary>
        ///     Converts the source unit to the target 
        /// </summary>
        /// <param name="src">Current unit of the value</param>
        /// <param name="tar">Unit to convert to</param>
        /// <param name="val">Value</param>
        /// <returns>Converted value</returns>
        public static decimal? ConvertTemp(TempUnit src, TempUnit tar, decimal? val)
        {
            if (src == tar) return val;
            switch (src)
            {
                case TempUnit.Fahrenheit when tar == TempUnit.Celsius:    return (val - 32) / 1.8M;
                case TempUnit.Celsius when tar    == TempUnit.Fahrenheit: return val * 1.8M + 32;
                default:
                    throw new ArgumentOutOfRangeException(nameof(src), src, null);
            }
        }

        /// <summary>
        ///     Converts a date into an int representing how old the person born on that date is
        /// </summary>
        /// <param name="dateOfBirth">Date of birth</param>
        /// <returns>Years old</returns>
        public static int ConvertDobToCurrentAge(DateTime dateOfBirth)
        {
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear) {
                age--;
            }

            return age;
        }

        public static string ConvertTableName(string tableName)
        {
            var result = Regex.Replace(tableName, @"(\p{Lu})", " $1").TrimStart();
            return result;
        }
    }
}