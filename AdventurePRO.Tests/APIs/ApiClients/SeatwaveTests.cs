// Author: Anastasia Mukalled
// Дата: 12.03.2016
// This file contains Seatwave class methods test

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdventurePRO.Tests;

namespace AdventurePRO.Model.APIs.ApiClients.Tests
{
    [TestClass()]
    public class SeatwaveTests
    {
        [TestMethod()]
        public void GetEventsAsyncTest()
        {
            Seatwave seatwave = new Seatwave(Seatwave.DEFAULT_API_KEY, Seatwave.DEFAULT_API_SECRET);

            var events =  seatwave.GetEventsAsync("Moscow", TestUtils.From, TestUtils.To, null).Result;

            Assert.IsNotNull(events);
            Assert.IsTrue(events.Length > 0);
        }

        [TestMethod()]
        public void GetVenueAsyncTest()
        {
            Seatwave seatwave = new Seatwave(Seatwave.DEFAULT_API_KEY, Seatwave.DEFAULT_API_SECRET);

            var venue = seatwave.GetVenueAsync("227").Result;

            Assert.IsNotNull(venue);
        }
    }
}