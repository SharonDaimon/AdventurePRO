// Author: Kristina Enikeeva
// Дата: 08.03.2016
// This file contains general hotelbeds hotel-content-api access logic

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.IO;

namespace AdventurePRO.Model.APIs.Hotelbeds
{
    public partial class ApiClient
    {
        private const string HOTEL_CONTENT_API = "hotel-content-api";
        private const string HOTEL_CONTENT_API_VERSION = "1.0";
        private const uint HOTEL_CONTENT_API_LIMIT = 1000;

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
                elements.AddRange(xdoc.Element(root_name).Element(array_name).Elements(item_name));
                
                from += HOTEL_CONTENT_API_LIMIT;

            } while (uint.Parse(xdoc.Element(root_name).Element("total").Value) > to);

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
