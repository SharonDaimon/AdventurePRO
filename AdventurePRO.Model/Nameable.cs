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
        public string Name { get; set; }

        /// <summary>
        /// The code
        /// </summary>
        public string Code { get; set; }
    }
}