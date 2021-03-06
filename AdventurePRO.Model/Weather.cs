﻿// Author: Anastasia Mukalled
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
        /// Unit of temperature measurement
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// For which location does this forecast right 
        /// </summary>
        public virtual Location Region { get; set; }

        /// <summary>
        /// Forecast date
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// The type of the weather
        /// </summary>
        public virtual Type WeatherType { get; set; }

        /// <summary>
        /// Weather Type
        /// </summary>
        public enum Type { Sunny, Cloudy, Partly_Cloudy, Rainy, Stormy, Thunderstorm }
    }
}