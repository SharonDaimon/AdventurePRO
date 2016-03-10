// Author: Ekaterina Kuznetsova
// Date: 07.03.2016
// This file contains RatingConverter

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace AdventurePRO.Views.Converters
{
    class RatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? string.Format("{0:0.0}", (float)value) : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
