// Author: Ekaterina Kuznetsova
// Date: 08.03.2016
// This file contains CombiningTwoConverters converter

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace AdventurePRO.Views.Converters
{
    internal class CombiningTwoConverters : IValueConverter
    {
        /*
        *   Code below is taken from
        *   http://stackoverflow.com/questions/1594357/wpf-how-to-use-2-converters-in-1-binding
        */

        public IValueConverter Converter1 { get; set; }
        public IValueConverter Converter2 { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object convertedValue = Converter1.Convert(value, targetType, parameter, culture);
            return Converter2.Convert(convertedValue, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
