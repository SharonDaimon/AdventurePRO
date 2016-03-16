// Author: Anastasia Mukalled
// Дата: 16.03.2016
// This file contains the description of the application logics class AdventureApiContext

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using AdventurePRO.Model.APIs.ApiClients;
using AdventurePRO.Model.APIs.Options;
using AdventurePRO.Model.APIs.Results;

namespace AdventurePRO.Model.Logics
{
    /// <summary>
    /// Describes an context contains adventure options and reults
    /// </summary>
    public class AdventureApiContext : INotifyPropertyChanged
    {
        private const double MIN_AIRPORT_HOTEL_TRAVELLING_TIME_IN_HOURS = 2;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public AdventureApiContext()
        {
            options = new AdventureOptions();

            options.PropertyChanged += Options_PropertyChanged;
        }

        private void Options_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AdventureResults = null;
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private AdventureOptions options;

        /// <summary>
        /// An adventure options
        /// </summary>
        public AdventureOptions Options
        {
            get
            {
                return options;
            }
            set
            {
                options = value;

                options.PropertyChanged += Options_PropertyChanged;

                AdventureResults = null;

                notifyPropertyChanged("Options");
            }
        }

        private Adventure[] adv_results;

        /// <summary>
        /// Results of creating
        /// </summary>
        public Adventure[] AdventureResults
        {
            get
            {
                if (adv_results == null)
                {

                }
                return adv_results;
            }
            private set
            {
                adv_results = value;
                notifyPropertyChanged("AdventureResults");
            }
        }

        private async void initAdventureResults()
        {
            if (options == null)
            {
                return;
            }

            var hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            var start = options.Earliest.AddHours(MIN_AIRPORT_HOTEL_TRAVELLING_TIME_IN_HOURS);

            var finish = options.Lastest.AddHours(-MIN_AIRPORT_HOTEL_TRAVELLING_TIME_IN_HOURS);

            var rooms = await hotelbeds.PostHotelsAsync
                (
                    options.Hotels,
                    start,
                    finish,
                    options.Accomodations
                );

            var hotels = rooms.Select(r => r.Hotel).Distinct();

            foreach(var h in hotels)
            {
                var h_rooms = from r in rooms
                              where r.Hotel == h
                              select r;

                h.Occupancies = (from r in h_rooms
                                 select new Occupancy
                                 {
                                     Code = r.Code,
                                     Name = r.Name,
                                     CheckIn = start,
                                     CheckOut = finish,
                                     RoomsCount = r.RoomsCount,
                                     Capacity = r.AdultsNumber + r.ChildrenNumber,
                                     OrderLink = r.OrderLink,
                                     Cost = r.Cost,
                                     Currency = r.Currency
                                 })
                                .ToArray();
            }

            var center = centerOfMass(hotels.Select(h => h.Location).ToArray());

            var nearest = hotels.OrderBy(h => distance(h.Location, center));

            var weather = new Openweathermap(Openweathermap.DEFAULT_KEY)
                .GetWeatherAsync(options.Destination.Location, 
                (uint)DateTime.Now.AddDays(10).Subtract(options.StartDate).Days);

            var adventures = from t in options.Trips
                             from h in hotels
                             select new Adventure
                             {
                                 Attractions = options.Attractions,
                                 Home = options.Origin,
                                 Destination = options.Destination,
                                 StartDate = options.StartDate,
                                 FinishDate = options.FinishDate,
                                 Hotels = new Hotel[1] { h },
                                 Persons = options.Persons,
                                 Tickets = new Ticket[2] { t.There, t.Back }
                             };

            AdventureResults = adventures.ToArray();
        }

        private static Location centerOfMass(params Location[] locations)
        {
            return new Location
            {
                Attitude = locations.Select(l => l.Attitude).Sum() / (float)locations.Count(),
                Longitude = locations.Select(l => l.Longitude).Sum() / (float)locations.Count()
            };
        }

        private static double distance(Location a, Location b)
        {
            return Math.Sqrt(
                            Math.Pow(a.Attitude - b.Attitude, 2) +
                            Math.Pow(a.Longitude - b.Longitude, 2)
                            );
        }
    }
}
