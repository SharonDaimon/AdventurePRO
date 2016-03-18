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
            if (values == null || values.Length <= 1)
            {
                return null;
            }

            var weather = values[0] as Weather[];
            int val = (int)(values[1] as double?);

            if (weather != null)
            {
                return weather[val];
            }
            else
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
