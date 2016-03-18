// Author: Ekaterina Kuznetsova
// Date: 08.03.2016
// This file contains AttractionsTicketsConverter

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using AdventurePRO.Model;

namespace AdventurePRO.Views.Converters
{
    internal class AttractionsTicketsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var attractions = value as Attraction[];
            if (attractions != null)
            {
                return (from a in attractions from t in a.Tickets select t).ToArray();
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
