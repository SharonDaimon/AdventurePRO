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
        public virtual Gender Gender { get; set; }

        /// <summary>
        /// Taxis list
        /// </summary>
        public virtual Taxi[] Taxis { get; set; }

        /// <summary>
        /// Tickets list
        /// </summary>
        public virtual Ticket[] Tickets { get; set; }

        /// <summary>
        /// Person accomodation
        /// </summary>
        public virtual Occupancy Accomodation { get; set; }

        /// <summary>
        /// Person`s avatar
        /// </summary>
        public virtual Uri Avatar { get; set; }

        /// <summary>
        /// Attractions tickets
        /// </summary>
        public virtual AttractionTicket[] Attractions { get; set; }
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
        Children,
        /// <summary>
        /// Adult
        /// </summary>
        Adult
    }
}