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
        public virtual float TotalPrice
        {
            get
            {
                return (from oc in Occupancies
                        select oc.Cost)
                        .Sum();
            }
        }
        /// <summary>
        /// Rooms list
        /// </summary>
        public virtual Occupancy[] Occupancies { get; set; }

        /// <summary>
        /// Hotel location
        /// </summary>
        public virtual Location Location { get; set; }

        /// <summary>
        /// Count of hotel stars
        /// </summary>
        public uint Stars { get; set; }
    }
}