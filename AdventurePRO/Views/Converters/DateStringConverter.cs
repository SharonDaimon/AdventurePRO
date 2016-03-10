// Author: Ekaterina Kuznetsova
// Date: 08.03.2016
// This file contains DateStringConverter

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace AdventurePRO.Views.Converters
{
    internal class DateStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
