// Author: Anastasia Mukalled
// Дата: 06.03.2016
// This file contains the description of the application data model 
// class StaticCurrencyConverter and interface ICurrencyConverter

namespace AdventurePRO.Model
{
    /// <summary>
    /// Contains an ICurrencyConverter object
    /// </summary>
    public static class StaticCurrencyConverter
    {
        /// <summary>
        /// The ICurrencyConverter object
        /// </summary>
        public static ICurencyConverter Converter;

        /// <summary>
        /// Converts "cost" in "from" currency to "to" currency 
        /// </summary>
        /// <param name="cost"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Converted value</returns>
        public static float Convert(float cost, string from, string to)
        {
            return Converter.Convert(cost, from, to);
        }
    }

    /// <summary>
    /// Converts cost from one currency to another
    /// </summary>
    public interface ICurencyConverter
    {
        /// <summary>
        /// Must return an exchange rate 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>An exchange rate</returns>
        float this[string from, string to] { get; }

        /// <summary>
        /// Must convert "cost" in "from" currency to "to" currency 
        /// </summary>
        /// <param name="cost"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Converted value</returns>
        float Convert(float cost, string from, string to);
    }
}
