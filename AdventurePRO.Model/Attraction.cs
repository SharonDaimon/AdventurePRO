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
        public Location Location { get; set; }

        /// <summary>
        /// The location ticket
        /// </summary>
        public AttractionTicket[] Tickets { get; set; }

        /// <summary>
        /// The attraction order href
        /// </summary>
        public string OrderHref { get; set; }
    }

    /// <summary>
    /// Describes the attraction ticket
    /// </summary>
    public class AttractionTicket : Acquirable
    {
        /// <summary>
        /// Ticket owner
        /// </summary>
        public Person Owner { get; set; }

        /// <summary>
        /// An attraction of the ticket
        /// </summary>
        public Attraction Attraction { get; set; }
    }
}