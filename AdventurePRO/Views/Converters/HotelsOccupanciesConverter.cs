// Author: Ekaterina Kuznetsova
// Date: 08.03.2016
// This file contains HotelsOccupanciesConverter

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using AdventurePRO.Model;

namespace AdventurePRO.Views.Converters
{
    internal class HotelsOccupanciesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Hotel[] hotels = value as Hotel[];

            if (hotels != null)
            {
                var occupancies = from h in hotels
                                  from occupancy in h.Occupancies
                                  select occupancy;

                return occupancies.ToArray();
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
