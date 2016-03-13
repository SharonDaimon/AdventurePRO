// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model classes Attraction and AttractionTicket
namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes the tourist attraction
    /// </summary>
    public class Attraction : OnlineDescribed
    {
        /// <summary>
        /// The attraction location
        /// </summary>
        public virtual Location Location { get; set; }

        /// <summary>
        /// The location ticket
        /// </summary>
        public virtual AttractionTicket[] Tickets { get; set; }
    }

    /// <summary>
    /// Describes the attraction ticket
    /// </summary>
    public class AttractionTicket : Acquirable
    {
        /// <summary>
        /// Ticket owner
        /// </summary>
        public virtual Person Owner { get; set; }

        /// <summary>
        /// An attraction of the ticket
        /// </summary>
        public virtual Attraction Attraction { get; set; }
    }
}