// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Acquirable

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes an acquirable product
    /// </summary>
    public abstract class Acquirable : Nameable
    {
        /// <summary>
        /// The price of the product
        /// </summary>
        public float Cost { get; set; }
    }
}