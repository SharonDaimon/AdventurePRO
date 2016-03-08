﻿// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Adventure

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes an adventure
    /// </summary>
    public class Adventure : DbItem
    {
        /// <summary>
        /// Full cost of the adventure
        /// </summary>
        public float FullCost
        {
            get
            {
                return (from a in Tickets.Cast<Acquirable>()
                        .Concat(Taxis)
                        .Concat(from h in Hotels from o in h.Occupancies select o)
                        .Concat(from a in Attractions from t in a.Tickets select t)
                        select StaticCurrencyConverter.Convert(a.Cost, a.Currency, this.Currency))
                         .Sum();
            }
        }

        /// <summary>
        /// The currency in which the full price is calculated
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Adventure start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Adventure finish date
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Adventure members list
        /// </summary>
        public Person[] Persons { get; set; }

        /// <summary>
        /// Adventure hotels list
        /// </summary>
        public Hotel[] Hotels { get; set; }

        /// <summary>
        /// Adventure taxis list
        /// </summary>
        public Taxi[] Taxis { get; set; }

        /// <summary>
        /// Adventure tickects list
        /// </summary>
        public Ticket[] Tickets { get; set; }

        /// <summary>
        /// The destination of the adventure
        /// </summary>
        public Destination Destination { get; set; }

        /// <summary>
        /// The home place of adventurers
        /// </summary>
        public Destination Home { get; set; }

        /// <summary>
        /// All adventure attractions list
        /// </summary>
        public Attraction[] Attractions { get; set; }

        /// <summary>
        /// Weather forecasts list
        /// </summary>
        public Weather[] Weather { get; set; }
    }
}