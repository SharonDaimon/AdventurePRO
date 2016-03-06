// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Hotel

using System.Linq;

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes a hotel
    /// </summary>
    public class Hotel : OnlineDescribed
    {
        /// <summary>
        /// The total price of all rooms
        /// </summary>
        public float TotalPrice
        {
            get
            {
                return (from oc in Occupancies
                        select StaticCurrencyConverter.Convert(oc.Cost, oc.Currency, this.Currency))
                        .Sum();
            }
        }
        /// <summary>
        /// Rooms list
        /// </summary>
        public Occupancy[] Occupancies { get; set; }

        /// <summary>
        /// Hotel location
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Count of hotel stars
        /// </summary>
        public uint Stars { get; set; }
    }
}