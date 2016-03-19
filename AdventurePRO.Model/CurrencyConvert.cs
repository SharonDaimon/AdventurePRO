// Author: Anastasia Mukalled
// Дата: 06.03.2016
// This file contains the description of the application data model 
// class StaticCurrencyConverter and interface ICurrencyConverter

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventurePRO.Model
{
    /// <summary>
    /// Contains an ICurrencyConverter object
    /// </summary>
    public static class StaticCurrencyConverter
    {
        public static void Init()
        {
            Converter = new converter();
        }

        static StaticCurrencyConverter()
        {
            Converter = new converter();
        }

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

        private class converter : ICurencyConverter
        {
            private const string EUR = "EUR";

            Dictionary<string, float> rates;

            public string[] Rates
            {
                get
                {
                    if (rates != null)
                    {
                        return rates.Keys.ToArray();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public converter()
            {
                init();
            }

            private async void init()
            {
                rates = await new Model.APIs.ApiClients.Fixer().GetRatesAsync();
            }

            public float this[string from, string to]
            {
                get
                {
                    if (rates != null
                        && (rates.ContainsKey(from) || from == EUR)
                        && rates.ContainsKey(to) || to == EUR)
                    {
                        return  (to == EUR ? 1 : rates[to]) / (from == EUR ? 1 : rates[from]);
                    }
                    return 1;
                }
            }

            public float Convert(float cost, string from, string to)
            {
                return cost * this[from, to];
            }
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

        string[] Rates { get; }

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
