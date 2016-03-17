// Author: Kristina Enikeeva
// Date: 10.03.2016
// This file contains hotels search results class HotelRoom

using System.Collections.Generic;

namespace AdventurePRO.Model.APIs.Results
{
    /// <summary>
    /// Single hotel room in hotel
    /// </summary>
    public class HotelRoom : Acquirable
    {
        /// <summary>
        /// Rate key for hotelbeds
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Count of rooms in the hotel room
        /// </summary>
        public uint RoomsCount { get; set; }

        /// <summary>
        /// Number of adults
        /// </summary>
        public uint AdultsNumber { get; set; }

        /// <summary>
        /// Number of children
        /// </summary>
        public uint ChildrenNumber { get; set; }

        /// <summary>
        /// Children ages
        /// </summary>
        public IEnumerable<uint> ChildrenAges { get; set; }

       /// <summary>
       /// The room hotel
       /// </summary>
        public Hotel Hotel { get; set; }
    }
}
