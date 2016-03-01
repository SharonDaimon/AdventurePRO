// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Weather
using System;

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes the weather forecast
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// Air temperature
        /// </summary>
        public float Temperature { get; set; }

        /// <summary>
        /// For which location does this forecast right 
        /// </summary>
        public Location Region { get; set; }

        /// <summary>
        /// Forecast date
        /// </summary>
        public DateTime Date { get; set; }
    }
}