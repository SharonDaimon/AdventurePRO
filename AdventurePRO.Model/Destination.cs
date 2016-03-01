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
        /// The code of the place
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// The location of the place
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Weather forecasts list
        /// </summary>
        public Weather[] Weather { get; set; }
    }
}