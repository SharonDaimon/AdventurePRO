// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class DbItem

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes an object can be stored in a database
    /// </summary>
    public abstract class DbItem
    {
        /// <summary>
        /// An object ID
        /// </summary>
        public int ID { get; set; }
    }
}