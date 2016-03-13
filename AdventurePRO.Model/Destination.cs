// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Destination

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes some place
    /// </summary>
    public class Destination : Nameable
    {
        /// <summary>
        /// The location of the place
        /// </summary>
        public virtual Location Location { get; set; }

        /// <summary>
        /// The country of the place
        /// </summary>
        public virtual Country Country { get; set; }
    }
}