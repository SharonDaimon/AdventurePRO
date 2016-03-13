// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Ticket
using System;

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes the transport ticket
    /// </summary>
    public class Ticket : Acquirable
    {
        /// <summary>
        /// Departure point
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Arrival point
        /// </summary>
        public string To { get; set; }
        
        /// <summary>
        /// The owner of the ticket
        /// </summary>
        public virtual Person Owner { get; set; }

        /// <summary>
        /// The type of a transport
        /// </summary>
        public string TransportType { get; set; }

        /// <summary>
        /// Arrival time
        /// </summary>
        public virtual DateTime Arrival { get; set; }

        /// <summary>
        /// Departure time
        /// </summary>
        public virtual DateTime Departure { get; set; }
    }
}