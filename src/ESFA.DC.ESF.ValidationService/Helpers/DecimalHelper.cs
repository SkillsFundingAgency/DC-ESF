﻿using System;
using System.Globalization;

namespace ESFA.DC.ESF.ValidationService.Helpers
{
    public class DecimalHelper
    {
        public static bool CheckDecimalLengthAndPrecision(
            decimal value,
            int integerPartLength,
            int floatingPointLength)
        {
            var stringValue = value.ToString(CultureInfo.InvariantCulture);
            if (stringValue.Contains("."))
            {
                return stringValue.Substring(0, stringValue.IndexOf('.') - 1).Length <= integerPartLength &&
                       stringValue.Substring(stringValue.IndexOf('.')).Length <= floatingPointLength;
            }

            return stringValue.Length <= integerPartLength;
        }
    }
}
