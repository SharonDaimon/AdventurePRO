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
            try {
                var attractions = value as Attraction[];
                if (attractions != null)
                {
                    if (attractions.Any(a => a == null))
                    {
                        return string.Empty;
                    }
                    if (attractions.Any(a => a.Tickets == null))
                    {
                        return string.Empty;
                    }
                    if (attractions.Any(a => a.Tickets.Any(t => t == null)))
                    {
                        return string.Empty;
                    }

                    var attr_t = from a in attractions where a != null && a.Tickets.Length > 0 from t in a.Tickets select t;
                    if (attr_t != null)
                    {
                        return attr_t.ToArray();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
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
