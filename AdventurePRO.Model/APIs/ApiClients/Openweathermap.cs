﻿// Author: Kristina Enikeeva
// Date: 12.03.2016
// This file contains weather api access logic

using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace AdventurePRO.Model.APIs.ApiClients
{
    /// <summary>
    /// Describes a weather api client
    /// </summary>
    public class Openweathermap
    {
        private const string endpoint = "http://api.openweathermap.org";
        private const string api = "data";
        private const string version = "2.5";

        /// <summary>
        /// The temperature of 0°C in Kalvins
        /// </summary>
        public float KELVIN_CONST = 273.15f;

        /// <summary>
        /// Default api key
        /// </summary>
        public const string DEFAULT_KEY = "91ed84cc4f2d00e3af6e4ff785ab5e4d";

        /// <summary>
        /// Creates new client with given api key
        /// </summary>
        /// <param name="key">Api key</param>
        public Openweathermap(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Api key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Queries a weather forecast for nearest given count of days
        /// </summary>
        /// <param name="location"></param>
        /// <param name="count_of_days"></param>
        /// <returns>The weather forecast for nearest given count of days</returns>
        public async Task<Weather[]> GetWeatherAsync(Location location, uint count_of_days)
        {
            NameValueCollection parameters = new NameValueCollection();

            //parameters.Add("lat", location.Attitude.ToString());
            //parameters.Add("lon", location.Longitude.ToString());
            parameters.Add("q", "Sankt-Peterburg");
            parameters.Add("cnt", count_of_days.ToString());
            parameters.Add("mode", "xml");
            parameters.Add("appid", Key);

            byte[] data = await HttpManager.GetAsync(endpoint, api, version, "forecast/daily",
                parameters, null, null);

            string str = Encoding.ASCII.GetString(data);

            XDocument response; 
            
            using (var stream = new MemoryStream(data))
            {
                using(var reader = XmlReader.Create(stream))
                {
                    response = XDocument.Load(reader);
                }
            }

            var weatherdata = response.Root;

            float lat = float.Parse(weatherdata.Element("location").Element("location").Attribute("altitude").Value, CultureInfo.InvariantCulture);
            float lon = float.Parse(weatherdata.Element("location").Element("location").Attribute("longitude").Value, CultureInfo.InvariantCulture);

            Location loc = new Location { Attitude = lat, Longitude = lon };

            var weather = from time in response.Element("weatherdata").Element("forecast").Elements("time")
                          select new Weather
                          {
                              Region = loc,
                              Date = DateTime.ParseExact(time.Attribute("day").Value, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                              Temperature = float.Parse(time.Element("temperature").Attribute("day").Value, CultureInfo.InvariantCulture) - KELVIN_CONST,
                              Unit = "°C"
                          };

            return weather.ToArray();

        }
    }
}
