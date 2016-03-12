// Author: Kristina Enikeeva
// Date: 11.03.2016
// This file contains seatwave api access logic

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Threading.Tasks;
using AdventurePRO.Model.APIs.Results;

namespace AdventurePRO.Model.APIs.ApiClients
{
    /// <summary>
    /// Describes an seatwaves api client
    /// </summary>
    public class Seatwave
    {
        private const string endpoint = "http://api-sandbox.seatwave.com";
        private const string api = "v2/discovery";
        private const string date_format = "yy-MM-dd";

        /// <summary>
        /// The default api key
        /// </summary>
        public const string DEFAULT_API_KEY = "05ab88651cf14449a31450bc9d5881d0";        
        
        /// <summary>
        /// The default api secret
        /// </summary>
        public const string DEFAULT_API_SECRET = "519c938343c84013ac998d0e1e3671fa";

        private const uint MAX_PAGE_SIZE = 50;

        /// <summary>
        /// Creates new api client
        /// </summary>
        /// <param name="key">Api key</param>
        /// <param name="secret">Api secret</param>
        public Seatwave(string key, string secret)
        {
            Key = key;
            Secret = secret;
        }

        /// <summary>
        /// Api key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Api secret
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// Returns the list of events by given parameters
        /// </summary>
        /// <param name="where">The place of event</param>
        /// <param name="whenFrom">Date from which to search</param>
        /// <param name="whenTo">Date until which to search</param>
        /// <param name="what">What do you search</param>
        /// <param name="max_price">The maximum price of event</param>
        /// <returns></returns>
        public async Task<SeatwaveEvent[]> GetEventsAsync(string where, DateTime whenFrom, DateTime whenTo, string what, float max_price = 0)
        {
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("what", what);
            parameters.Add("whenFrom", whenFrom.ToString(date_format));
            parameters.Add("whenTo", whenTo.ToString(date_format));

            if(what != null)
            {
                parameters.Add("what", what);
            }

            if(max_price != 0)
            {
                parameters.Add("maxPrice", max_price.ToString());
            }

            var pages = await GetAllPagesAsync("events", parameters, "EventsResponse", "Events", "Event");

            var serializer = new XmlSerializer(typeof(SeatwaveEvent));

            return (from p in pages
                    select (SeatwaveEvent)serializer.Deserialize(p.CreateReader()))
                    .ToArray();
        }

        /// <summary>
        /// Returns an atraction venue
        /// </summary>
        /// <param name="id">The id of the venue</param>
        /// <returns>Venue with given id</returns>
        public async Task<SeatwaveVenue> GetVenueAsync(string id)
        {
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("venueId", id);

            var root = await GetRootElementAsync("venue", parameters, "VenueResponse");

            var venue = root.Element("Venue");

            return new SeatwaveVenue()
            {
                VenueId = venue.Element("Id").Value,
                Name = venue.Element("Name").Value,
                Location = new Location()
                {
                    Attitude = float.Parse(venue.Element("Lat").Value),
                    Longitude = float.Parse(venue.Element("Long").Value)
                }
            };
        }

        /// <summary>
        /// Retturns all the pages
        /// </summary>
        /// <param name="method">Api method</param>
        /// <param name="parameters">Api parameters</param>
        /// <param name="root_name">The name of the root element</param>
        /// <param name="array_name">Name of array</param>
        /// <param name="element_name">Name of each element in array</param>
        /// <returns>All the pages</returns>
        public async Task<XElement[]> GetAllPagesAsync(string method, NameValueCollection parameters,
            string root_name, string array_name, string element_name)
        {
            parameters["pageSize"] = MAX_PAGE_SIZE.ToString();
            
            List<XElement> elements = new List<XElement>();

            XElement x_root;

            uint page_number;
            uint total_page_count;

            do
            {
                x_root = await GetRootElementAsync(method, parameters, root_name);

                elements.AddRange(x_root.Element(array_name).Elements(element_name));

                total_page_count = uint.Parse(x_root.Element("Paging").Element("TotalPageCount").Value);

                page_number = uint.Parse(x_root.Element("Paging").Element("PageNumber").Value) + 1;

                parameters["pageNumber"] = page_number.ToString();

            } while (page_number <= total_page_count);

            return elements.ToArray();
        }

        /// <summary>
        /// Creates a GET request to api and returns the root element of the response
        /// </summary>
        /// <param name="method">Api method</param>
        /// <param name="parameters">Api parameters</param>
        /// <param name="root_name">The name of the root element</param>
        /// <returns>The root element of the response</returns>
        public async Task<XElement> GetRootElementAsync(string method, NameValueCollection parameters, string root_name)
        {
            parameters["apikey"] = Key;

            byte[] data = await HttpManager.GetAsync(endpoint, api, null, method, parameters, null, null);

            using (var stream = new MemoryStream(data))
            {
                var x_doc = XDocument.Load(stream);

                var x_root = x_doc.Element(root_name);

                if (x_root.Element("Status").Element("Code").Value != "0")
                {
                    return null; // TODO: throw am exception
                }

                return x_root;

            }
        }

    }
}
