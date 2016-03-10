// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Person and enums Gender, PersonType

using System;

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes a person
    /// </summary>
    public class Person : Nameable
    {
        /// <summary>
        /// Age of the person
        /// </summary>
        public uint? Age { get; set; }

        /// <summary>
        /// Sex of the person
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Taxis list
        /// </summary>
        public Taxi[] Taxis { get; set; }

        /// <summary>
        /// Tickets list
        /// </summary>
        public Ticket[] Tickets { get; set; }

        /// <summary>
        /// Person accomodation
        /// </summary>
        public Occupancy Accomodation { get; set; }

        /// <summary>
        /// Person`s avatar
        /// </summary>
        public Uri Avatar { get; set; }

        /// <summary>
        /// Attractions tickets
        /// </summary>
        public AttractionTicket[] Attractions { get; set; }

        /// <summary>
        /// Child or Adult
        /// </summary>
        public PersonType Type { get; set; }
    }

    /// <summary>
    /// Describes the person`s gender
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Male
        /// </summary>
        Male,
        /// <summary>
        /// Female
        /// </summary>
        Female
    }

    /// <summary>
    /// Adults / Child
    /// </summary>
    public enum PersonType
    {
        /// <summary>
        /// Child
        /// </summary>
        Child,
        /// <summary>
        /// Adult
        /// </summary>
        Adult
    }
}