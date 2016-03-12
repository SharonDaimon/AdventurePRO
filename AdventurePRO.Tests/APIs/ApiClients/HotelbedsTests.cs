// Author: Anastasia Mukalled
// Дата: 12.03.2016
// This file contains Hotelbeds class methods test

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdventurePRO.Model.APIs.ApiClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePRO.Model.APIs.Options;
using AdventurePRO.Tests;

namespace AdventurePRO.Model.APIs.ApiClients.Tests
{
    [TestClass()]
    public class HotelbedsTests
    {
        [TestMethod]
        public void TestHotelbedsGetCountriesAsyncWithoutParameters()
        {
            Hotelbeds hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            var countries = hotelbeds.GetCountriesAsync().Result;

            Assert.IsNotNull(countries);
            Assert.IsTrue(countries.Length > 0);
            var russia = countries.First(c => c.Code == "RU");
            Assert.IsNotNull(russia);
            Assert.AreEqual<string>(russia.Name, "RUSSIA");
        }

        [TestMethod]
        public void TestHotelbedsGetCountriesAsyncWithOneParameter()
        {
            string code = "RU";

            Hotelbeds hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            var countries = hotelbeds.GetCountriesAsync(code).Result;

            Assert.IsNotNull(countries);
            Assert.AreEqual<int>(countries.Length, 1);
            Assert.AreEqual<string>(countries[0].Code, "RU");
            Assert.AreEqual<string>(countries[0].Name, "RUSSIA");
        }

        [TestMethod]
        public void TestHotelbedsGetCountriesAsyncWithSeveralParameters()
        {
            string code1 = "RU";
            string code2 = "UK";

            Hotelbeds hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            var countries = hotelbeds.GetCountriesAsync(code1, code2).Result;

            Assert.IsNotNull(countries);
            Assert.AreEqual<int>(countries.Length, 2);

            var russia = countries.First(c => c.Code == "RU");
            var uk = countries.First(c => c.Code == "UK");

            Assert.IsNotNull(russia);
            Assert.IsNotNull(uk);
            Assert.AreEqual(russia.Name, "RUSSIA");
            Assert.AreEqual(uk.Name, "UNITED KINGDOM");
        }

        [TestMethod]
        public void TestHotelbedsGetDestinationsAsyncWithNullParameter()
        {
            Hotelbeds hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            var destinations = hotelbeds.GetDestinationsAsync(null).Result;

            Assert.AreEqual<int>(destinations.Length, 0);
        }

        [TestMethod]
        public void TestHotelbedsGetDestinationsAsyncWithEmptyParameter()
        {
            Hotelbeds hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            var destinations = hotelbeds.GetDestinationsAsync(new List<Country>()).Result;

            Assert.AreEqual<int>(destinations.Length, 0);
        }

        [TestMethod]
        public void TestHotelbedsGetDestinationsAsyncWithOneParameter()
        {
            Hotelbeds hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            Country[] countries = {
                new Country
                {
                    Code = "RU",
                    Name = "RUSSIA"
                }
            };

            var destinations = hotelbeds.GetDestinationsAsync(countries).Result;

            Assert.IsNotNull(destinations);

            var spb = countries.First(c => c.Code == "LED");

            Assert.IsNotNull(spb);
            Assert.IsTrue(spb.Name.Contains("Petersburg"));
        }


        [TestMethod()]
        public void GetHotelsByDestinationTest()
        {
            Hotelbeds hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            var destination = new Destination { Name = "St Petersburg", Code = "LED" };

            var hotels = hotelbeds.GetHotelsByDestination(destination).Result;

            Assert.IsNotNull(hotels);
            Assert.IsTrue(hotels.Length > 0);
        }

        [TestMethod()]
        public void PostHotelsAsyncTest()
        {
            Hotelbeds hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            Hotel[] hotels =
            {
                new Hotel
                {
                    Code = "21918"
                }
            };

            Accomodation[] accomodations =
            {
                new Accomodation
                {
                    Guests = new Person[]
                    {
                        new Person
                        {
                            Age = 19,
                            Name = "Nasty",
                            Gender = Gender.Female,
                            Type = PersonType.Adult
                        }
                    },
                    RoomsCount = 1
                }
            };

            var hotel_rooms =
                hotelbeds.PostHotelsAsync(hotels, TestUtils.From, TestUtils.To, accomodations)
                .Result;

            Assert.IsNotNull(hotel_rooms);
        }

    }
}