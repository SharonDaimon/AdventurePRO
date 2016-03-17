// Author: Anastasia Mukalled
// Дата: 12.03.2016
// This file contains Openweathermap class methods test

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdventurePRO.Model.APIs.ApiClients;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventurePRO.Model.APIs.ApiClients.Tests
{
    [TestClass()]
    public class OpenweathermapTests
    {
        [TestMethod()]
        public void GetWeatherAsyncTest()
        {
            uint days_count = 10;

            Openweathermap map = new Openweathermap(Openweathermap.DEFAULT_KEY);

            var location = new Location
            {
                Attitude = 59.916668f,
                Longitude = 30.25f
            };

            var weather = map.GetWeatherAsync(location, days_count).Result;

            Assert.IsNotNull(weather);
            Assert.AreEqual<uint>((uint)weather.Length, days_count);

            DateTime today = DateTime.Now;

            for(uint day = 0; day < days_count; ++day)
            {
                DateTime cur = today.AddDays(day);

                Assert.AreEqual(cur.Year, weather[day].Date.Year);
                Assert.AreEqual(cur.Month, weather[day].Date.Month);
                Assert.AreEqual(cur.Day, weather[day].Date.Day);
            }
        }
    }
}