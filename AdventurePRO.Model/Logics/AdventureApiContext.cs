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

                notifyPropertyChanged("Options");
            }
        }

        private Adventure adv_result;

        /// <summary>
        /// Results of creating
        /// </summary>
        public Adventure AdventureResult
        {
            get
            {
                if (adv_result == null)
                {
                    initAdventureResult();
                }
                return adv_result;
            }
            private set
            {
                adv_result = value;
                notifyPropertyChanged("AdventureResult");
            }
        }

        public async Task<Adventure> GetResultAsync()
        {
            if (options == null ||
                options.StartDate == null ||
                options.FinishDate == null ||
                options.Persons == null ||
                options.AvailableHotels == null)
            {
                return null;
            }

            var hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            var checkIn = options.StartDate.AddHours(options.AfterArrivalRelaxTime);

            var checkOut = options.FinishDate.AddHours(options.BeforeDepartureRelaxTime);

            var h = options.Hotel;

            if (h == null)
            {

                h = options.AvailableHotels.First();
            }

            var rooms = await hotelbeds.PostHotelsAsync
                (
                    new Hotel[1] { h },
                    checkIn,
                    checkOut,
                    new Accomodation[1] { new Accomodation { Guests = options.Persons, RoomsCount = options.CountOfRooms } }
                );

            if (rooms == null)
            {
                return null;
            }

            var r = rooms.FirstOrDefault();
            if (r == null)
            {
                return null;
            }
            
            var occupancies = new Occupancy[1]
                { new Occupancy
                               {
                                   Code = r.Code,
                                   Name = r.Name,
                                   CheckIn = checkIn,
                                   CheckOut = checkOut,
                                   RoomsCount = r.RoomsCount,
                                   Capacity = r.AdultsNumber + r.ChildrenNumber,
                                   OrderLink = r.OrderLink,
                                   Cost = r.Cost,
                                   Currency = r.Currency,
                                   Guests = options.Persons
                               }
                };

            h.Occupancies = occupancies;

            var weather = await new Openweathermap(Openweathermap.DEFAULT_KEY)
                .GetWeatherAsync(options.Destination.Location,
                (uint)DateTime.Now.AddDays(10).Subtract(options.StartDate).Days);

            var trips = await options.GetAvailableTripsAsync();
            if (trips == null) { return null; }

            var trip = trips.First();


            return new Adventure
            {
                Attractions = options.Attractions,
                Home = options.Origin,
                Destination = options.Destination,
                StartDate = options.StartDate,
                FinishDate = options.FinishDate,
                Hotels = new Hotel[1] { h },
                Persons = options.Persons,
                Tickets = new Ticket[2] { trip.There, trip.Back },
                Weather = weather,
                Currency = options.Currency
            };
        }

        private async void initAdventureResult()
        {
            AdventureResult = await GetResultAsync();
        }

    }
}
