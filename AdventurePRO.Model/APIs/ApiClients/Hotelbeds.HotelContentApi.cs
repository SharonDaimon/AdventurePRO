// Author: Kristina Enikeeva
// Date: 08.03.2016
// This file contains hotelbeds hotel-content-api access logic

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System;
using System.Globalization;

namespace AdventurePRO.Model.APIs.ApiClients
{
    public partial class Hotelbeds
    {
        private const string HOTEL_CONTENT_API = "hotel-content-api";
        private const string HOTEL_CONTENT_API_VERSION = "1.0";
        private const string HOTEL_CONTENT_API_HOTELS_METHOD = "hotels";
        private const string HOTEL_CONTENT_API_COUNTRIES_METHOD = "locations/countries";
        private const string HOTEL_CONTENT_API_DESTINATIONS_METHOD = "locations/destinations";
        private const uint HOTEL_CONTENT_API_LIMIT = 1000;

        private const string PATH_TO_IMAGES = "http://photos.hotelbeds.com/giata/";

        private static readonly XName COUNTRIES_AR = xmlns + "countries";
        private static readonly XName DESTINATIONS_AR = xmlns + "destinations";
       
        private static readonly XName TOTAL_EL = xmlns + "total";
        private static readonly XName COUNTRIES_RS_EL = xmlns + "countriesRS";
        private static readonly XName COUNTRY_EL = xmlns + "country";
        private static readonly XName COUNTRY_NAME_EL = xmlns + "description"; // Yes, description!
        private static readonly XName DESTINATIONS_RS_EL = xmlns + "destinationsRS";
        private static readonly XName DESTINATION_EL = xmlns + "destination";
        private static readonly XName DESTINATION_NAME_EL = xmlns + "name";
        private static readonly XName HOTELS_RS_EL = xmlns + "hotelsRS";
        private static readonly XName HOTEL_NAME_EL = xmlns + "name";
        private static readonly XName HOTEL_DESCRIPTION_EL = xmlns + "description";
        private static readonly XName HOTEL_WEB_EL = xmlns + "web";
        private static readonly XName HOTEL_COORDINATES_EL = xmlns + "coordinates";
        

        private static readonly XName COUNTRY_CODE_ATTR = "code";
        private static readonly XName DESTINATION_CODE_ATTR = "code";

        /// <summary>
        /// Requests hotel-comtent-api for countries list
        /// </summary>
        /// <param name="country_codes">
        /// The country codes to request for. 
        /// Set null to get all available countries
        /// </param>
        /// <returns>The list of available countries codes</returns>
        public async Task<Country[]> GetCountriesAsync(params string[] country_codes)
        {
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("fields", "all");
            parameters.Add("language", LANG);

            if (country_codes != null && country_codes.Count() > 0)
            {
                parameters.Add("codes", string.Join(",", country_codes));
            }

            var x_countries = await WithFromTo(HOTEL_CONTENT_API_COUNTRIES_METHOD, parameters, COUNTRIES_RS_EL, COUNTRIES_AR, COUNTRY_EL);

            var countries = from xc in x_countries
                            select new Country()
                            {
                                Name = value(xc, COUNTRY_NAME_EL).Trim(' '),
                                Code = attribute(xc,COUNTRY_CODE_ATTR)
                            };

            return countries.ToArray();
        }

        /// <summary>
        /// Requests hotel-comtent-api for destinations list
        /// </summary>
        /// <param name="countries">
        /// The list of countries for the list of destinations is returned. 
        /// If it is null, method should return the list of destinations for or of available countries,
        /// but Country property in each of returned destination should be null
        /// </param>
        /// <returns>The list of available destinations</returns>
        public async Task<Destination[]> GetDestinationsAsync(IEnumerable<Country> countries)
        {
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("fields", "code,countryCode,name");
            parameters.Add("language", LANG);

            if (countries != null && countries.Count() > 0)
            {
                parameters.Add("countryCodes", string.Join(",", countries.Select(c => c.Code)));
            }

            var x_destinations = await WithFromTo(HOTEL_CONTENT_API_DESTINATIONS_METHOD, parameters, DESTINATIONS_RS_EL, DESTINATIONS_AR, DESTINATION_EL);

            //  To prevent NullReferenceException in linq to sql below
            if (countries == null)
            {
                countries = new Country[0];
            }

            var destinations = from xc in x_destinations
                               select new Destination()
                               {
                                   Name = value(xc,DESTINATION_NAME_EL),
                                   Code = attribute(xc,DESTINATION_CODE_ATTR),
                                   Country = countries.FirstOrDefault(c => c.Code == attribute(xc,COUNTRY_CODE_ATTR))
                               };

            return destinations.ToArray();
        }

        /// <summary>
        /// Requests hotels list by destination
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public async Task<Hotel[]> GetHotelsByDestination(Destination d)
        {
            if (d == null)
            {
                return null;
            }

            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("fields",
                "name,description,coordinates,categoryCode,chaincode,address,email,phones,images,web");
            parameters.Add("destinationCode", d.Code);

            var x_hotels = await WithFromTo(HOTEL_CONTENT_API_HOTELS_METHOD, parameters, HOTELS_RS_EL, HOTELS_AR, HOTEL_EL);

            var hotels = from h in x_hotels
                         select new Hotel()
                         {
                             Code = attribute(h, HOTEL_CODE_ATTR),
                             Name = value(h, HOTEL_NAME_EL),
                             Description = value(h, HOTEL_DESCRIPTION_EL),
                             Site = value(h, HOTEL_WEB_EL),
                             Location = new Location()
                             {
                                 Attitude = float_attribute(element(h, HOTEL_COORDINATES_EL), LATITUDE_ATTR),
                                 Longitude = float_attribute(element(h, HOTEL_COORDINATES_EL), LONGITUDE_ATTR)
                             },
                             Photos = parseHotelImages(h)
                         };

            if (hotels != null)
            {
                return hotels.ToArray();
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Gets the full list of objects where api returns portions of them from "from" to "to" position
        /// </summary>
        /// <param name="method">API method</param>
        /// <param name="parameters">Request parameters</param>
        /// <param name="root_name">Name of the response body xml root element</param>
        /// <param name="array_name">Name of the xml object array</param>
        /// <param name="item_name">Name of desirable objects</param>
        /// <returns>Full list of desirable objects</returns>
        public async Task<IEnumerable<XElement>> WithFromTo(string method, NameValueCollection parameters,
            XName root_name, XName array_name, XName item_name)
        {
            uint from = 1;
            uint to;

            List<XElement> x_elements = new List<XElement>();

            XDocument xdoc;
            do
            {
                to = from + HOTEL_CONTENT_API_LIMIT - 1;

                xdoc = await from_to(method, parameters, from, to);

                var root = element(xdoc, root_name);
                var array = element(root, array_name);
                var ar_elements = elements(array, item_name);

                x_elements.AddRange(ar_elements);

                from += HOTEL_CONTENT_API_LIMIT;

            } while (uint.Parse(xdoc.Element(root_name).Element(TOTAL_EL).Value) > to);

            return x_elements;
        }

        //  Returns portion of objects from "from" to "to"
        private async Task<XDocument> from_to(string method, NameValueCollection parameters, uint from, uint to)
        {
            parameters["from"] = from.ToString();
            parameters["to"] = to.ToString();

            byte[] response = await GetAsync(HOTEL_CONTENT_API, HOTEL_CONTENT_API_VERSION, method, parameters);

            using (var stream = new MemoryStream(response))
            {
                return XDocument.Load(stream);
            }
        }
    }
}
