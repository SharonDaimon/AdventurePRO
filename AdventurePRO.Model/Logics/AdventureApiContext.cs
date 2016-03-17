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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
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

            HotelRoom[] rooms;


            var center = centerOfMass(options.Attractions.Select(a => a.Location).ToArray());

            var nearest_attr = options.Attractions.OrderBy(a => distance(center, a.Location));

            if (!options.SearchByGPS)
            {
                rooms = await hotelbeds.PostHotelsAsync
                    (
                        options.Hotels,
                        start,
                        finish,
                        options.Accomodations
                    );
            }
            else
            {
                rooms = await hotelbeds.PostHotelsByGPSRadiusAsync
                    (
                        options.Hotels,
                        start,
                        finish,
                        options.Accomodations,
                        center,
                        (float)distance(center, nearest_attr.Last().Location)
                    );
            }

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

        // Returns distance in kilometers between two points
        private static double distance(Location a, Location b)
        {
            // This code is taken from
            // http://stackoverflow.com/questions/6544286/calculate-distance-of-two-geo-points-in-km-c-sharp

            double R = 6371; // km

            double sLat1 = Math.Sin(radians(a.Attitude));
            double sLat2 = Math.Sin(radians(b.Attitude));
            double cLat1 = Math.Cos(radians(a.Attitude));
            double cLat2 = Math.Cos(radians(b.Attitude));
            double cLon = Math.Cos(radians(a.Longitude) - radians(b.Longitude));

            double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;

            double d = Math.Acos(cosD);

            double dist = R * d;

            return dist;
        }

        private static double radians(double l)
        {
            return 2 * Math.PI * (l / 360.0); 
        }
    }
}
