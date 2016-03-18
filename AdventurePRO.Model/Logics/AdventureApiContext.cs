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
            AdventureResult = null;
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

                AdventureResult = null;

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

        private async void initAdventureResult()
        {
            if (options == null ||
                options.StartDate == null ||
                options.FinishDate == null ||
                options.Accomodations == null ||
                options.Hotel == null ||
                options.AvailableTrips == null)
            {
                return;
            }

            var hotelbeds = new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET);

            var checkIn = options.StartDate.AddHours(options.AfterArrivalRelaxTime);

            var checkOut = options.FinishDate.AddHours(options.BeforeDepartureRelaxTime);

            var h = options.Hotel;

            var rooms = await hotelbeds.PostHotelsAsync
                (
                    new Hotel[1] { h },
                    checkIn,
                    checkOut,
                    options.Accomodations
                );

            if (rooms == null)
            {
                return;
            }

            var occupancies = (from r in rooms
                               select new Occupancy
                               {
                                   Code = r.Code,
                                   Name = r.Name,
                                   CheckIn = checkIn,
                                   CheckOut = checkOut,
                                   RoomsCount = r.RoomsCount,
                                   Capacity = r.AdultsNumber + r.ChildrenNumber,
                                   OrderLink = r.OrderLink,
                                   Cost = r.Cost,
                                   Currency = r.Currency
                               });

            if (occupancies != null)
            {

                h.Occupancies = occupancies.ToArray();
            }
            
            var weather = await new Openweathermap(Openweathermap.DEFAULT_KEY)
                .GetWeatherAsync(options.Destination.Location,
                (uint)DateTime.Now.AddDays(10).Subtract(options.StartDate).Days);

            AdventureResult = new Adventure
            {
                Attractions = options.Attractions,
                Home = options.Origin,
                Destination = options.Destination,
                StartDate = options.StartDate,
                FinishDate = options.FinishDate,
                Hotels = new Hotel[1] { h },
                Persons = options.Persons,
                Tickets = new Ticket[2] { null, null },
                Weather = weather
            };
        }

    }
}
