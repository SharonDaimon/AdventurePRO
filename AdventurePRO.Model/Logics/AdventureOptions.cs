// Author: Anastasia Mukalled
// Дата: 16.03.2016
// This file contains the description of the application logics class adventureOptions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventurePRO.Model.APIs.ApiClients;
using AdventurePRO.Model.APIs.Options;
using AdventurePRO.Model.APIs.Results;
using System.ComponentModel;

namespace AdventurePRO.Model.Logics
{
    /// <summary>
    /// Describes an options to create an adventure list
    /// </summary>
    public class AdventureOptions : INotifyPropertyChanged
    {
        /// <summary>
        /// The property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #region Date

        private DateTime startDate, finishDate;

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;

                AvailableTrips = null;

                notifyPropertyChanged("StartDate");
            }
        }

        /// <summary>
        /// Finish date
        /// </summary>
        public DateTime FinishDate
        {
            get
            {
                return finishDate;
            }
            set
            {
                finishDate = value;

                AvailableTrips = null;

                notifyPropertyChanged("FinishDate");
            }
        }

        /// <summary>
        /// How much time does user need to relax after arriving to the destination
        /// </summary>
        public double AfterArrivalRelaxTime { get; set; }

        /// <summary>
        /// How much time does user need to relax before departing the destination
        /// </summary>
        public double BeforeDepartureRelaxTime { get; set; }

        /// <summary>
        /// How much time does user need to relax between day attractions
        /// </summary>
        public double DayRelaxTime { get; set; }

        /// <summary>
        /// How much time does user need to relax at night
        /// </summary>
        public double NightRelaxTime { get; set; }

        #endregion

        #region Countries

        private Country[] availableCountries;

        /// <summary>
        /// An available countries list
        /// </summary>
        public Country[] AvailableCountries
        {
            get
            {
                if (availableCountries == null)
                {
                    initAvailableCountries();
                }
                return availableCountries;
            }

            private set
            {
                availableCountries = value;

                // Set dependent properties to null
                Country = null;

                notifyPropertyChanged("AvailableCountries");
            }
        }

        private async void initAvailableCountries()
        {
            AvailableCountries =
                await new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET)
                .GetCountriesAsync();
        }

        private Country country;

        /// <summary>
        /// Country
        /// </summary>
        public Country Country
        {
            get { return country; }
            set
            {
                country = value;

                // Set dependent properties to null
                AvailableDestinations = null;

                notifyPropertyChanged("Country");
            }
        }

        #endregion

        #region Origin

        private Country originCountry;

        /// <summary>
        /// The origin country
        /// </summary>
        public Country OriginCountry
        {
            get
            {
                return originCountry;
            }
            set
            {
                originCountry = value;

                // Set dependent properties to null
                AvailableOrigins = null;

                notifyPropertyChanged("OriginCountry");
            }
        }

        private Destination[] availableOrigins;

        /// <summary>
        /// The available places of origin country
        /// that could be an origin
        /// </summary>
        public Destination[] AvailableOrigins
        {
            get
            {
                return availableOrigins;
            }
            set
            {
                availableOrigins = value;

                // Set dependent properties to null
                Origin = null;

                notifyPropertyChanged("AvailableOrigins");
            }
        }

        private async void initAvailableOrigins()
        {
            if (OriginCountry == null)
            {
                return;
            }

            AvailableOrigins =
                await new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET)
                .GetDestinationsAsync(new Country[1] { OriginCountry });
        }

        private Destination origin;

        /// <summary>
        /// An origin place
        /// </summary>
        public Destination Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;

                AvailableTrips = null;

                notifyPropertyChanged("Origin");
            }
        }

        #endregion

        #region Destination

        private Destination[] availableDestinations;

        /// <summary>
        /// The destinations available in this country
        /// </summary>
        public Destination[] AvailableDestinations
        {
            get
            {
                if (availableDestinations == null)
                {
                    initAvailableDestinations();
                }
                return availableDestinations;
            }
            private set
            {
                availableDestinations = value;

                // Set dependent properties to null
                Destination = null;

                notifyPropertyChanged("AvailableDestinations");
            }
        }

        private async void initAvailableDestinations()
        {
            if (Country == null)
            {
                return;
            }

            AvailableDestinations =
                await new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET)
                .GetDestinationsAsync(new Country[1] { Country });
        }

        private Destination destination;

        /// <summary>
        /// Destination
        /// </summary>
        public Destination Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;

                // Set dependent properties to null
                AvailableTrips = null;
                AvailableHotels = null;

                notifyPropertyChanged("Destination");
            }
        }

        #endregion

        #region Persons

        private Person[] persons;

        /// <summary>
        /// Persons list
        /// </summary>
        public Person[] Persons
        {
            get
            {
                return persons;
            }
            set
            {
                persons = value;

                AvailableTrips = null;
                AvailableHotels = null;

                notifyPropertyChanged("Persons");
            }
        }

        #endregion

        private Accomodation[] accomodations;

        public Accomodation[] Accomodations
        {
            get
            {
                return accomodations;
            }
            set
            {
                accomodations = value;

                notifyPropertyChanged("Accomodations");
            }
        }

        #region Trips

        private QPXTrip[] availableTrips;

        /// <summary>
        /// The list of available tickets from and to
        /// </summary>
        public QPXTrip[] AvailableTrips
        {
            get
            {
                if (availableTrips == null)
                {
                    initAvailableTrips();
                }
                return availableTrips;
            }
            private set
            {
                availableTrips = value;

                notifyPropertyChanged("AvailableTrips");
            }
        }

        private async void initAvailableTrips()
        {
            AvailableTrips = await new QPX(QPX.DEFAULT_API_KEY).RequestTicketsAsync(
                    Origin.Code,
                    Destination.Code,
                    StartDate,
                    FinishDate,
                    (uint)Persons.Where(p => p.Type == PersonType.Adult).Count(),
                    (uint)Persons.Where(p => p.Type == PersonType.Child).Count()
                );
        }

        private QPXTrip[] trips;

        /// <summary>
        /// Trips selected by user
        /// </summary>
        public QPXTrip[] Trips
        {
            get
            {
                return Trips;
            }
            set
            {
                trips = value;

                AvailableHotels = null;

                notifyPropertyChanged("Trips");
            }
        }

        /// <summary>
        /// Returns the earliset arrival to destination
        /// </summary>
        public DateTime Earliest
        {
            get
            {
                if(Trips == null)
                {
                    return StartDate; ;
                }
                return Trips.OrderBy(t => t.There.Arrival).First().There.Arrival;
            }
        }

        /// <summary>
        /// Returns the lastest departure from destination
        /// </summary>
        public DateTime Lastest
        {
            get
            {
                if (Trips == null)
                {
                    return StartDate; ;
                }
                return Trips.OrderBy(t => t.Back.Departure).First().Back.Departure;
            }
        }

        #endregion

        #region Hotels

        private Hotel[] hotels;

        private Hotel[] availableHotels;

        /// <summary>
        /// The hotels available in this destination
        /// </summary>
        public Hotel[] AvailableHotels
        {
            get
            {
                if (availableHotels == null)
                {
                    initAvailableHotels();
                }
                return availableHotels;
            }
            set
            {
                availableHotels = value;

                // Set dependent properties to null
                Hotels = null;

                notifyPropertyChanged("AvailableHotels");
            }
        }

        private async void initAvailableHotels()
        {
            if (Destination == null)
            {
                return;
            }
            AvailableHotels =
                await new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET)
                .GetHotelsByDestination(Destination);
        }

        /// <summary>
        /// List of hotels in which to lodge
        /// </summary>
        public Hotel[] Hotels
        {
            get { return hotels; }
            set
            {
                hotels = value;

                notifyPropertyChanged("Hotels");
            }
        }

        /// <summary>
        /// Determines whether user needs to search by GPS
        /// </summary>
        public bool SearchByGPS { get; set; }

        #endregion

        #region Attractions and events

        private SeatwaveEvent[] availableEvents;

        /// <summary>
        /// The events available in this destination
        /// </summary>
        public SeatwaveEvent[] AvailableEvents
        {
            get
            {
                if (availableEvents == null)
                {
                    initAvailableEvets();
                }
                return availableEvents;
            }
            set
            {
                availableEvents = value;

                AvailableAttractions = null;

                notifyPropertyChanged("AvailableEvents");
            }
        }

        private async void initAvailableEvets()
        {
            if (Destination == null)
            {
                return;
            }

            AvailableEvents =
                await new Seatwave(Seatwave.DEFAULT_API_KEY, Seatwave.DEFAULT_API_SECRET)
                    .GetEventsAsync
                    (
                        Destination.Name, 
                        Earliest.AddHours(AfterArrivalRelaxTime), 
                        Lastest.AddHours(BeforeDepartureRelaxTime),
                        null
                    );
        }

        /// <summary>
        /// The attractions available in this destination.
        /// Only creates an attractions list from AvailableEvents
        /// </summary>
        public Attraction[] AvailableAttractions
        {
            get
            {
                return getAvailableAttractions();
            }
            private set
            {
                // Set dependent properties to null
                Attractions = null;

                notifyPropertyChanged("AvailableAttractions");
            }
        }

        private Attraction[] getAvailableAttractions()
        {
            // Calling the AvailableEvents property should call the update,
            // calling the update should set the AvailableAttractions property to null
            // since the AvailableAttractions property is dependent on AvailableEvents,
            // then it should notify about AvailableAttractions property changing,
            // and then calling an AvailableAttractions property getter should call this method,
            // but by that time AvailableEvents property is not null 
            // and this method should return the AvailableAttraction property
            var events = AvailableEvents;

            if (events == null)
            {
                return null;
            }

            return (from e in events
                    select new Attraction
                    {
                        Code = e.Id,
                        Name = e.GroupName,
                        Site = e.EventSwURL,
                        Photos = new Uri[1] { new Uri(e.GroupImageURL) }
                    })
                    .ToArray();
        }

        private Attraction[] attractions;

        /// <summary>
        /// List of Attractions, which are interesting for user
        /// </summary>
        public Attraction[] Attractions
        {
            get { return attractions; }
            set
            {
                attractions = value;

                notifyPropertyChanged("Attractions");
            }
        }

        #endregion

    }
}
