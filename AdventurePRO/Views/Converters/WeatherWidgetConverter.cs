// Author: Ekaterina Kuznetsova
// Date: 06.03.2016
// This file contains WeatherWidgetConverter

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using AdventurePRO.Model;

namespace AdventurePRO.Views.Converters
{
    class WeatherWidgetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Weather[] weather = (Weather[])values[0];
            int val = (int)((double)values[1]);
            return weather[val];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
