// Author: Kristina Enikeeva
// Date: 10.03.2016
// This file contains hotelbeds hotel-api logic

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;
using AdventurePRO.Model.APIs.Results;
using AdventurePRO.Model.APIs.Options;

namespace AdventurePRO.Model.APIs.ApiClients
{
    public partial class Hotelbeds
    {
        private const string HOTEL_API = "hotel-api";
        private const string API_VERSION = "1.0";
        private const string HOTEL_API_HOTELS_METHOD = "hotels";

        private static readonly XName OCCUPANCIES_AR = xmlns + "occupancies";
        private static readonly XName PAXES_AR = xmlns + "paxes";

        private static readonly XName AVAILABILITY_RQ_EL = xmlns + "availabilityRQ";
        private static readonly XName STAY_EL = xmlns + "stay";
        private static readonly XName OCCUPANCY_EL = xmlns + "occupancy";
        private static readonly XName PAX_EL = xmlns + "pax";
        private static readonly XName GEOLOCATION_EL = xmlns + "geolocation";

        private static readonly XName CHECK_IN_ATTR = "checkIn";
        private static readonly XName CHECK_OUT_ATTR = "checkOut";
        private static readonly XName PERSON_TYPE_ATTR = "type";
        private static readonly XName PERSON_AGE_ATTR = "age";
        private static readonly XName RADIUS_ATTR = "radius";
        private static readonly XName UNIT_ATTR = "unit";

        private const string CHILD_VAL = "CH";
        private const string ADULT_VAL = "AD";
        private const string DEFAULT_UNIT = "km";

        private const string DATE_PATTERN = "yyyy-MM-dd";

        /// <summary>
        /// Creates POST request to get a hotel rooms list by given hotels
        /// </summary>
        /// <param name="hotels">Hotels list to search rooms in</param>
        /// <param name="checkIn">Check in date</param>
        /// <param name="checkOut">Check out date</param>
        /// <param name="accomodations">Accomodation option</param>
        /// <returns>A list of hotel rooms</returns>
        public async Task<HotelRoom[]> PostHotelsAsync
        (
            IEnumerable<Hotel> hotels,
            DateTime checkIn,
            DateTime checkOut,
            IEnumerable<Accomodation> accomodations
        )
        {
            XDocument x_request = new XDocument
            (
                new XElement(AVAILABILITY_RQ_EL,
                    new XElement(STAY_EL,
                        new XAttribute(CHECK_IN_ATTR, checkIn.ToString(DATE_PATTERN)),
                        new XAttribute(CHECK_OUT_ATTR, checkOut.ToString(DATE_PATTERN))),
                        new XElement(OCCUPANCIES_AR,
                            from accomodation in accomodations
                            select new XElement(OCCUPANCY_EL,
                                new XAttribute(ROOMS_COUNT_ATTR, accomodation.RoomsCount ?? 1),
                                new XAttribute(ADULTS_NUMBER_ATTR, accomodation.Guests.Count(p => p.Type == PersonType.Adult)),
                                new XAttribute(CHILDREN_NUMBER_ATTR, accomodation.Guests.Count(p => p.Type == PersonType.Child)),
                                new XElement(PAXES_AR,
                                    from person in accomodation.Guests
                                    select new XElement(PAX_EL,
                                        new XAttribute(PERSON_TYPE_ATTR, person.Type == PersonType.Child ? CHILD_VAL : ADULT_VAL),
                                        new XAttribute(PERSON_AGE_ATTR, person.Age)
                                    )
                                )
                            )
                        ),
                        new XElement(HOTELS_AR, from hotel in hotels select new XElement(HOTEL_EL, hotel.Code))
                )
            );

            var rooms = await sendRequsetAndParseRespnonse_HotelApi(x_request, hotels);

            return rooms.ToArray();
        }

        /// <summary>
        /// Creates POST request to get a hotel rooms list by given hotels
        /// </summary>
        /// <param name="hotels">Hotels list to search rooms in</param>
        /// <param name="checkIn">Check in date</param>
        /// <param name="checkOut">Check out date</param>
        /// <param name="accomodations">Accomodation option</param>
        /// <param name="center">Center of the area to search in</param>
        /// <param name="radius">radius of the area</param>
        /// <returns>The list of hotel accomodations for given parameters</returns>
        public async Task<HotelRoom[]> PostHotelsByGPSRadiusAsync
        (
            IEnumerable<Hotel> hotels,
            DateTime checkIn, DateTime checkOut,
            IEnumerable<Accomodation> accomodations,
            Location center,
            float radius
        )
        {
            XDocument xdoc = new XDocument
            (
                new XElement(AVAILABILITY_RQ_EL,
                    new XElement(STAY_EL,
                        new XAttribute(CHECK_IN_ATTR, checkIn.ToString(DATE_PATTERN)),
                        new XAttribute(CHECK_OUT_ATTR, checkOut.ToString(DATE_PATTERN))),
                        new XElement(OCCUPANCIES_AR,
                            from accomodation in accomodations
                            select new XElement(OCCUPANCY_EL,
                                new XAttribute(ROOMS_COUNT_ATTR, accomodation.RoomsCount ?? 1),
                                new XAttribute(ADULTS_NUMBER_ATTR, accomodation.Guests.Count(p => p.Type == PersonType.Adult)),
                                new XAttribute(CHILDREN_NUMBER_ATTR, accomodation.Guests.Count(p => p.Type == PersonType.Child)),
                                new XElement(PAXES_AR,
                                    from person in accomodation.Guests
                                    select new XElement(PAX_EL,
                                        new XAttribute(PERSON_TYPE_ATTR, person.Type == PersonType.Child ? CHILD_VAL : ADULT_VAL),
                                        new XAttribute(PERSON_AGE_ATTR, person.Age)
                                    )
                                )
                            )
                        ),
                        new XElement(GEOLOCATION_EL,
                            new XAttribute(LONGITUDE_ATTR, center.Longitude),
                            new XAttribute(LATITUDE_ATTR, center.Attitude),
                            new XAttribute(RADIUS_ATTR, radius),
                            new XAttribute(UNIT_ATTR, DEFAULT_UNIT)
                        )
                )
            );

            var rooms = await sendRequsetAndParseRespnonse_HotelApi(xdoc, hotels);

            return rooms.ToArray();
        }
    }
}
