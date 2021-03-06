﻿// Author: Ekaterina Kuznetsova
// Date: 08.03.2016
// This file contains  AdventureCostStringConverter

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using AdventurePRO.Model;

namespace AdventurePRO.Views.Converters
{
    internal class AdventureCostStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.Adventure adventure = value as Model.Adventure;
            if (adventure != null)
            {
                return string.Format("{0:0.00} {1}", adventure.FullCost, adventure.Currency);
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
