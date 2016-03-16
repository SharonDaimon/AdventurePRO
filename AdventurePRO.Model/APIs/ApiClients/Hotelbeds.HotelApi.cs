// Author: Kristina Enikeeva
// Date: 10.03.2016
// This file contains hotelbeds hotel-api logic

using System;
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
        private const string DATE_PATTERN = "yyyy-MM-dd";

        /// <summary>
        /// Creates POST request to get a hotel rooms list by given hotels
        /// </summary>
        /// <param name="hotels">Hotels list to search rooms in</param>
        /// <param name="checkIn">Check in date</param>
        /// <param name="checkOut">Check out date</param>
        /// <param name="accomodations">Accomodation option</param>
        /// <returns>A list of hotel rooms</returns>
        public async Task<HotelRoom[]> PostHotelsAsync(IEnumerable<Hotel> hotels, DateTime checkIn, DateTime checkOut, IEnumerable<Accomodation> accomodations)
        {
            XDocument xdoc = new XDocument
                (new XElement(xmlns + "availabilityRQ",
                    new XElement(xmlns + "stay", new XAttribute("checkIn", checkIn.ToString(DATE_PATTERN)), new XAttribute("checkOut", checkOut.ToString(DATE_PATTERN))),
                    new XElement(xmlns + "occupancies", from accomodation in accomodations
                                                select new XElement(xmlns + "occupancy",
                                                new XAttribute("rooms", accomodation.RoomsCount ?? 1),
                                                new XAttribute("adults", accomodation.Guests.Count(p => p.Type == PersonType.Adult)),
                                                new XAttribute("children", accomodation.Guests.Count(p => p.Type == PersonType.Child)),
                                                new XElement(xmlns + "paxes", from person in accomodation.Guests
                                                                      select new XElement(xmlns + "pax", new XAttribute("type", person.Type == PersonType.Child ? "CH" : "AD"), new XAttribute("age", person.Age)))
                                                                      )),
                    new XElement(xmlns + "hotels", from hotel in hotels select new XElement(xmlns + "hotel", hotel.Code))
                ));

            byte[] request_data = null;

            using (var stream = new MemoryStream())
            {
                xdoc.Save(stream);
                request_data = stream.ToArray();
            }

            byte[] response = await PostAsync("hotel-api", "1.0", "hotels", null, request_data);

            XDocument x_response;

            using(var stream = new MemoryStream(response))
            {
                x_response = XDocument.Load(stream);
            }

            var rooms = from hotel in x_response.Element(xmlns + "availabilityRS").Element(xmlns + "hotels").Elements(xmlns + "hotel")
                        from room in hotel.Element(xmlns + "rooms").Elements(xmlns + "room")
                        from rate in room.Element(xmlns + "rates").Elements(xmlns + "rate")
                        select new HotelRoom()
                        {
                            Hotel = hotels.FirstOrDefault(h => h.Code == hotel.Attribute("code").Value),
                            Currency = hotel.Attribute("currency").Value,
                            Name = room.Attribute("name").Value,
                            Code = room.Attribute("code").Value,
                            AdultsNumber = uint.Parse(rate.Attribute("adults").Value),
                            ChildrenNumber = uint.Parse(rate.Attribute("children").Value),
                            ChildrenAges = from a in rate.Attribute("childrenAges").Value.Split(',') select uint.Parse(a),
                            RoomsCount = uint.Parse(room.Attribute("rooms").Value),
                            Cost = float.Parse(rate.Attribute("net").Value),
                            Key = rate.Attribute("rateKey").Value                            
                        };

            return rooms.ToArray();
        }
    }
}
