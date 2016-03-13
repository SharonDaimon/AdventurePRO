// Author: Kristina Enikeeva
// Date: 11.03.2016
// This file contains attractions search results class SeatwaveEvent

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;

namespace AdventurePRO.Model.APIs.Results
{
    /// <summary>
    /// Describes an Event
    /// </summary>
    [Serializable]
    [XmlRoot("Event")]
    public class SeatwaveEvent
    {
        private const string date_format = "yyyy-MM-ddTHH\\:mm\\:ss";

        /// <summary>
        /// The event id
        /// </summary>
        [XmlElement("Id")]
        public string Id { get; set; }

        /// <summary>
        /// Date and time of the event
        /// </summary>
        [XmlIgnore]
        public DateTime Date { get; set; }

        /// <summary>
        /// The date in string representation. Using for xml serialization
        /// </summary>
        [XmlElement("Date")]
        public string DateString
        {
            get { return Date.ToString(date_format); }
            set { this.Date = DateTime.ParseExact(value, date_format, CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// The name of the artist, performer, sports team etc.
        /// </summary>
        [XmlElement("EventGroupName")]
        public string GroupName { get; set; }

        /// <summary>
        /// Artist or performer image URL
        /// </summary>
        [XmlElement("EventGroupImageURL")]
        public string GroupImageURL { get; set; }

        /// <summary>
        ///  Name of the venue
        /// </summary>
        [XmlAttribute("VenueName")]
        public string VenueName { get; set; }

        /// <summary>
        /// Id of the venue
        /// </summary>
        [XmlAttribute("VenueID")]
        public string VenueId { get; set; }

        /// <summary>
        /// URL of the Seatwave event page. This will redirect via an affiliate platform when one has been specified
        /// </summary>
        [XmlAttribute("SwURL")]
        public string EventSwURL { get; set; }

        /// <summary>
        /// The number of available tickets
        /// </summary>
        [XmlAttribute("TicketCount")]
        public uint CountOfTickets { get; set; }

        /// <summary>
        /// The mininum price price of a ticket
        /// </summary>
        [XmlAttribute("MinPrice")]
        public float Price { get; set; }

        /// <summary>
        /// The minimum price currency (standard 3 letter code)
        /// </summary>
        [XmlAttribute("Currency")]
        public string Currency { get; set; }


    }
}
