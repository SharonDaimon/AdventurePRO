// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Occupancy
using System;

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes a hotel room
    /// </summary>
    public class Occupancy : Acquirable
    {
        /// <summary>
        /// Check out date
        /// </summary>
        public virtual DateTime CheckOut { get; set; }

        /// <summary>
        /// Check in date
        /// </summary>
        public virtual DateTime CheckIn { get; set; }

        /// <summary>
        /// Total number of days in this room
        /// </summary>
        public virtual uint DaysAmount
        {
            get
            {
                return (uint)((CheckOut - CheckIn).TotalDays + 0.5);
            }
        }
        
        /// <summary>
        /// Room capacity
        /// </summary>
        public uint? Capacity { get; set; }

        /// <summary>
        /// Determines if the room has a bathroom
        /// </summary>
        public bool? HasBathrom { get; set; }

        /// <summary>
        /// Count of rooms
        /// </summary>
        public uint? RoomsCount { get; set; }

        /// <summary>
        /// Room guests list
        /// </summary>
        public virtual Person[] Guests { get; set; }

        /// <summary>
        /// Type of the room
        /// </summary>
        public string Type { get; set; }
    }
}