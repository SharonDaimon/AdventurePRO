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
        private const uint HOTEL_CONTENT_API_LIMIT = 1000;
        private static readonly XNamespace xmlns = "http://www.hotelbeds.com/schemas/messages";

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

            var x_countries = await WithFromTo("locations/countries", parameters, "countriesRS", "countries", "country");

            var countries = from xc in x_countries
                            select new Country()
                            {
                                Name = xc.Element(xmlns + "description").Value.Trim(' '),
                                Code = xc.Attribute("code").Value
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

            var x_destinations = await WithFromTo("locations/destinations", parameters, "destinationsRS", "destinations", "destination");

            //  To prevent NullReferenceException in linq to sql below
            if (countries == null)
            {
                countries = new Country[0];
            }

            var destinations = from xc in x_destinations
                               select new Destination()
                               {
                                   Name = xc.Element(xmlns + "name").Value,
                                   Code = xc.Attribute("code").Value,
                                   Country = countries.FirstOrDefault(c => c.Code == xc.Attribute("countryCode").Value)
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
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("fields",
                "name,description,coordinates,categoryCode,chaincode,address,email,phones,images,web");
            parameters.Add("destinationCode", d.Code);

            var elements = await WithFromTo("hotels", parameters, "hotelsRS", "hotels", "hotel");

            var empty = new XElement("Empty");

            var empty_ar = new XElement[0];

            try
            {
                return (from xe in elements
                        select new Hotel()
                        {
                            Code = xe.Attribute("code").Value,
                            Name = (xe.Element(xmlns + "name") ?? empty).Value,
                            Description = (xe.Element(xmlns + "description") ?? empty).Value,
                            Site = (xe.Element(xmlns + "web") ?? empty).Value,
                            Location = new Location()
                            {
                                Attitude = float.Parse((xe.Element(xmlns + "coordinates") ?? empty)
                                                    .Attribute("latitude").Value,
                                                    CultureInfo.InvariantCulture),
                                Longitude = float.Parse((xe.Element(xmlns + "coordinates") ?? empty)
                                                    .Attribute("longitude").Value,
                                                    CultureInfo.InvariantCulture)
                            },
                            Photos = (from image in ((xe.Element(xmlns + "images") ?? empty)
                                            .Elements(xmlns + "image") ?? empty_ar)
                                      where image.Attribute("imageTypeCode").Value == "RES"
                                      select new Uri("http://photos.hotelbeds.com/giata/" + image.Attribute("path").Value))
                                      .ToArray()

                        }).ToArray();
            }
            catch (Exception e)
            {
                throw e;
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
            string root_name, string array_name, string item_name)
        {
            uint from = 1;
            uint to;

            List<XElement> elements = new List<XElement>();

            XDocument xdoc;
            do
            {
                to = from + HOTEL_CONTENT_API_LIMIT - 1;

                xdoc = await from_to(method, parameters, root_name, array_name, item_name, from, to);

                var root = xdoc.Root;
                var array = root.Element(xmlns + array_name);
                var ar_elements = array.Elements(xmlns + item_name);

                elements.AddRange(ar_elements /*xdoc.Element(root_name).Element(array_name).Elements(item_name)*/);

                from += HOTEL_CONTENT_API_LIMIT;

            } while (uint.Parse(xdoc.Element(xmlns + root_name).Element(xmlns + "total").Value) > to);

            return elements;
        }

        //  Returns portion of objects from "from" to "to"
        private async Task<XDocument> from_to(string method, NameValueCollection parameters,
            string root_name, string array_name, string item_name, uint from, uint to)
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
