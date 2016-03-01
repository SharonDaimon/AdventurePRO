// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Nameable

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes smth. nameable
    /// </summary>
    public abstract class Nameable : DbItem
    {
        /// <summary>
        /// The name
        /// </summary>
        public int Name { get; set; }
    }
}