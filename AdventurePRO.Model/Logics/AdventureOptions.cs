// Author: Anastasia Mukalled
// Дата: 16.03.2016
// This file contains the description of the application logics class adventureOptions

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
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

                AvailableAttractions = null;
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

                AvailableAttractions = null;
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
                OriginCountry = null;

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
                if (availableOrigins == null)
                {
                    initAvailableOrigins();
                }
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
                AvailableAttractions = null;

                notifyPropertyChanged("Destination");
            }
        }

        #endregion

        #region Persons and accomodations

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

                AvailableTrips = null;

                notifyPropertyChanged("Accomodations");
            }
        }

        /// <summary>
        /// Persons list
        /// </summary>
        public Person[] Persons
        {
            get
            {
                if (Accomodations == null)
                {
                    return null;
                }

                if(Accomodations.Any(a => a == null))
                {
                    return null;
                }

                if(Accomodations.Any(a => a.Guests == null))
                {
                    return null;
                }

                if(Accomodations.Any(a => a.Guests.Any(g =>  g == null)))
                {
                    return null;
                }

                var persons = from a in Accomodations
                              from g in a.Guests
                              select g;
                
                if (persons != null)
                {
                    return persons.ToArray();
                }
                else { return null; }
            }
        }

        #endregion

        #region Trips

        private QPXTrip[] availableTrips;

        /// <summary>
        /// The list of available tickets from and to
        /// </summary>
        public IOrderedEnumerable<QPXTrip> AvailableTrips
        {
            get
            {
                if(availableTrips != null)
                {
                    return availableTrips.OrderBy
                        (
                            t 
                            =>
                            Math.Pow(t.There.Arrival.Ticks - StartDate.Ticks, 2)
                            +
                            Math.Pow(t.Back.Departure.Ticks - FinishDate.Ticks, 2)
                        );
                }

                else 
                {
                    initAvailableTrips();
                    return null;
                }
            }
            private set
            {
                if (value != null)
                {
                    availableTrips = value.ToArray();
                }
                
                notifyPropertyChanged("AvailableTrips");
            }
        }

        private async void initAvailableTrips()
        {

            var empty_collection = new Person[] { };

            if (Origin == null
                || Destination == null
                || StartDate == null
                || FinishDate == null
                || Persons == null)
            {
                return;
            }

            availableTrips = await new QPX(QPX.DEFAULT_API_KEY).RequestTicketsAsync(
                    Origin.Code,
                    Destination.Code,
                    StartDate,
                    FinishDate,
                    (uint)(Persons.Where(p => p.Type == PersonType.Adult) ?? empty_collection).Count(),
                    (uint)(Persons.Where(p => p.Type == PersonType.Child) ?? empty_collection).Count()
                );

            notifyPropertyChanged("AvailableTrips");
        }

        #endregion

        #region Hotels

        private Hotel hotel;

        private Hotel[] availableHotels;

        /// <summary>
        /// The hotels available in this destination
        /// </summary>
        public IOrderedEnumerable<Hotel> AvailableHotels
        {
            get
            {
                if (availableHotels == null)
                {
                    initAvailableHotels();
                }
                return availableHotels.OrderBy(h => distance(h.Location, centerOfAttractions));
            }
            set
            {
                if (value != null)
                {
                    availableHotels = value.ToArray();
                }
                notifyPropertyChanged("AvailableHotels");
            }
        }

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

        private async void initAvailableHotels()
        {
            if (Destination == null)
            {
                return;
            }
            var hotels =
                await new Hotelbeds(Hotelbeds.DEFAULT_KEY, Hotelbeds.DEFAULT_SECRET)
                .GetHotelsByDestination(Destination);

            if(hotels != null)
            {
                availableHotels = hotels.ToArray();

                notifyPropertyChanged("AvailableHotels");
            }
        }

        /// <summary>
        /// The hotel selected by user
        /// </summary>
        public Hotel Hotel
        {
            get { return hotel; }
            set
            {
                hotel = value;

                notifyPropertyChanged("Hotels");
            }
        }

        /// <summary>
        /// Determines whether user needs to search by GPS
        /// </summary>
        public bool SearchByGPS { get; set; }

        #endregion

        #region Attractions and events

        private Location centerOfAttractions
        {
            get
            {
                if (Destination == null)
                {
                    return default(Location);
                }

                if (Attractions == null)
                {
                    return Destination.Location;
                }

                var locations = Attractions.Select(a => a.Location).ToArray();

                if (locations == null)
                {
                    return Destination.Location;
                }

                return centerOfMass(locations.ToArray());
            }
        }

        private static Location centerOfMass(params Location[] locations)
        {
            return new Location
            {
                Attitude = locations.Select(l => l.Attitude).Sum() / (float)locations.Count(),
                Longitude = locations.Select(l => l.Longitude).Sum() / (float)locations.Count()
            };
        }

        /// <summary>
        /// What attractions to search
        /// </summary>
        public string WhatAttraction { get; set; }

        private Attraction[] available_attractions;

        /// <summary>
        /// The attractions available in this destination.
        /// Only creates an attractions list from AvailableEvents
        /// </summary>
        public Attraction[] AvailableAttractions
        {
            get
            {
                if (available_attractions == null)
                {
                    initAvailableAttractions();
                }

                return available_attractions;
            }

            private set
            {
                available_attractions = value;

                notifyPropertyChanged("AvailableAttractions");
            }
        }

        private async void initAvailableAttractions()
        {
            if (Destination == null
                || StartDate == null
                || FinishDate == null)
            {
                return;
            }

            var events =
                await new Seatwave(Seatwave.DEFAULT_API_KEY, Seatwave.DEFAULT_API_SECRET)
                    .GetEventsAsync
                    (
                        Destination.Name,
                        StartDate.AddHours(AfterArrivalRelaxTime),
                        FinishDate.AddHours(BeforeDepartureRelaxTime),
                        WhatAttraction
                    );

            if (events == null)
            {
                return;
            }

            var avail = (from e in events
                         select new Attraction
                         {
                             Code = e.Id,
                             Name = e.GroupName,
                             Site = e.EventSwURL,
                             Photos = new Uri[1] { new Uri(e.GroupImageURL) },
                             Tickets = createTickets(e)
                         });

            if (avail == null)
            {
                return;
            }

            AvailableAttractions = avail.ToArray();
        }

        private AttractionTicket[] createTickets(SeatwaveEvent e)
        {
            if (Persons == null || e == null)
            {
                return null;
            }

            var tickets = from p in Persons
                          select new AttractionTicket
                          {
                              Owner = p,
                              Cost = e.Price,
                              Currency = e.Currency,
                              OrderLink = e.EventSwURL
                          };

            if (tickets != null)
            {
                return tickets.ToArray();
            }

            return null;
        }

        private List<Attraction> attractions;

        /// <summary>
        /// List of Attractions selected by user
        /// </summary>
        public Attraction[] Attractions
        {
            get {
                if(attractions != null)
                {
                    return attractions.ToArray();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                attractions = new List<Attraction>(value);

                notifyPropertyChanged("Attractions");
            }
        }

        /// <summary>
        /// Adds the selected by user attraction
        /// </summary>
        /// <param name="a"></param>
        public void AddAttraction(Attraction a)
        {
            attractions.Add(a);
        }

        /// <summary>
        /// Removes the selected by user attraction
        /// </summary>
        /// <param name="a"></param>
        public void RemoveAttraction(Attraction a)
        {
            if (attractions.Contains(a))
            {
                attractions.Remove(a);
            }
        }

        #endregion

    }
}
