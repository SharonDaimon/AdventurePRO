// Author: Ekaterina Kuznetsova
// Date: 06.03.2016
// This file contains AcquirableToCostStringConverter

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using AdventurePRO.Model;

namespace AdventurePRO.Views.Converters
{
    class AcquirableToCostStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Acquirable ac = (Acquirable)value;
            return string.Format("{0:0.00} {1}", ac.Cost, ac.Currency);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
