using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdventurePRO.Model.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventurePRO.Model.APIs.ApiClients;
using AdventurePRO.Model.APIs.Options;
using AdventurePRO.Model.APIs.Results;
using AdventurePRO.Model.Logics;
using AdventurePRO.Model;
using AdventurePRO.Tests;
using Nito.AsyncEx.UnitTests;

namespace AdventurePRO.Model.Logics.Tests
{
    [TestClass]
    public class AdventureLogicsTests
    {
        [TestMethod]
        public async Task AdventureApiContextTestAsync()
        {
            AdventureOptions options = new AdventureOptions();

            options.StartDate = TestUtils.From;
            options.FinishDate = TestUtils.To;

            options.Persons = new Person[1] { new Person { Name = "Nasty", Age=19, Type=PersonType.Adult } };

            options.Accomodations = new Accomodation[1] { new Accomodation { Guests = options.Persons, RoomsCount = 1 } };

            options.AfterArrivalRelaxTime = 4;
            options.DayRelaxTime = 4;
            options.NightRelaxTime = 12;
            options.BeforeDepartureRelaxTime = 4;

            options.SearchByGPS = true;

            var countries = options.AvailableCountries;

            await Task.Delay(1000);
            countries = options.AvailableCountries;

            Assert.IsNotNull(countries);
            Assert.IsTrue(countries.Any());

            options.OriginCountry = countries.Where(c => c.Code == "RU").First();

            options.Country = countries.Last();

            var destinations = options.AvailableDestinations;

            await Task.Delay(1000);

            destinations = options.AvailableDestinations;

            Assert.IsNotNull(destinations);
            Assert.IsTrue(destinations.Any());
            
            options.Destination = destinations.First();

            var origins = options.AvailableOrigins;

            await Task.Delay(1000);

            origins = options.AvailableOrigins;

            Assert.IsNotNull(origins);

            options.Origin = origins.Where(o => o.Code == "LED").First();

            var trips = options.AvailableTrips;

            await Task.Delay(1000);

            trips = options.AvailableTrips;

            Assert.IsNotNull(trips);

            options.Trips = trips;

            var attractions = options.AvailableAttractions;

            await Task.Delay(1000);

            attractions = options.AvailableAttractions;

            Assert.IsNotNull(attractions);

            options.Attractions = attractions;

            var hotels = options.AvailableHotels;

            await Task.Delay(1000);

            hotels = options.AvailableHotels;

            Assert.IsNotNull(hotels);
            Assert.IsTrue(hotels.Any());

            options.Hotels = hotels;

            var context = new AdventureApiContext
            {
                Options = options
            };

            var results = context.AdventureResults;

            await Task.Delay(5000);

            results = context.AdventureResults;

            Assert.IsNotNull(results);

        }
    }
}
