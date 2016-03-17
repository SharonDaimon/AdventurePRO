// Author: Kristina Enikeeva
// Дата: 12.03.2016
// This file contains qpx access logic

namespace AdventurePRO.Model.APIs.Results
{
    /// <summary>
    /// Describes a qpx trip
    /// </summary>
    public class QPXTrip : Acquirable
    {
        /// <summary>
        /// Ticket to destination
        /// </summary>
        public Ticket There { get; set; }

        /// <summary>
        /// Ticket back
        /// </summary>
        public Ticket Back { get; set; }
    }
}
