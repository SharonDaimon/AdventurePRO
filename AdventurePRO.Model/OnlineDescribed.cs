// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class OnlineDescribed

using System;

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes smth. described online
    /// </summary>
    public abstract class OnlineDescribed : Nameable
    {
        /// <summary>
        /// The descriotion
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The rating
        /// </summary>
        public float? Rating { get; set; }

        /// <summary>
        /// Photos list
        /// </summary>
        public virtual Uri[] Photos { get; set; }

        /// <summary>
        /// Web - site
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Comments list
        /// </summary>
        public virtual Comment[] Comments { get; set; }
    }
}