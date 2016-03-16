// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Taxi
using System;

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes a taxi
    /// </summary>
    public class Taxi : Acquirable
    {
        /// <summary>
        /// Passengers list
        /// </summary>
        public virtual Person[] Passengers { get; set; }

        /// <summary>
        /// The start point of this taxi
        /// </summary>
        public virtual Location From { get; set; }

        /// <summary>
        /// The finish point of this taxi
        /// </summary>
        public virtual Location To { get; set; }

        /// <summary>
        /// Then does this taxi commute
        /// </summary>
        public virtual DateTime When { get; set; }
    }
}