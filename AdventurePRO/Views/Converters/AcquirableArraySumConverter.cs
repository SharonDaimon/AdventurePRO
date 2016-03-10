// Author: Ekaterina Kuznetsova
// Date: 06.03.2016
// This file contains AcquirableArraySumConverter

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using AdventurePRO.Model;

namespace AdventurePRO.Views.Converters
{
    internal class AcquirableArraySumConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ac = (Acquirable[])values[0];
            string currency = (string)values[1];
            

            float sum = (from a in ac select StaticCurrencyConverter.Convert(a.Cost, a.Currency, currency)).Sum();
            return string.Format("{0:0.00} {1}", sum, currency);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
