// Author: Kristina Enikeeva
// Date: 11.03.2016
// This file contains attractions search results class SeatwaveVenue

namespace AdventurePRO.Model.APIs.Results
{
    /// <summary>
    /// Describes an adventure Venue
    /// </summary>
    public class SeatwaveVenue : Nameable
    {
        /// <summary>
        /// An id of the venue
        /// </summary>
        public string VenueId { get; set; }

        /// <summary>
        /// The location of the venue
        /// </summary>
        public Location Location { get; set; }
    }
}
