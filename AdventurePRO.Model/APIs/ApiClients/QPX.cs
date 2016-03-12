// Author: Kristina Enikeeva
// Дата: 12.03.2016
// This file contains qpx access logic

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AdventurePRO.Model.APIs.Results;

namespace AdventurePRO.Model.APIs.ApiClients
{
    /// <summary>
    /// A QPX API client
    /// </summary>
    public class QPX
    {
        private const string endpoint = "https://www.googleapis.com";
        private const string api = "qpxExpress";
        private const string version = "v1";

        private static readonly char[] numbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

        /// <summary>
        /// Default api key
        /// </summary>
        public const string DEFAULT_API_KEY = "AIzaSyAQ1FVevSqFvWJUGSeF26WDVqTSU-qDpIQ";

        /// <summary>
        /// Creates new client with given api key
        /// </summary>
        /// <param name="key"></param>
        public QPX(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Api key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Requests for available tickets
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="date"></param>
        /// <param name="returning_date"></param>
        /// <param name="adult_count"></param>
        /// <param name="child_count"></param>
        /// <returns>Available tickets list</returns>
        public async Task<QPXTrip[]> RequestTicketsAsync(string origin, string destination, DateTime date, DateTime returning_date, uint adult_count, uint child_count)
        {
            JObject req_obj = new JObject
                (
                    new JProperty("request",
                        new JProperty("passengers",
                            new JProperty("kind", "qpxexpress#passengerCounts"),
                            new JProperty("adultCount", adult_count),
                            new JProperty("childCount", child_count)
                        ),
                        new JProperty("slice",
                            new JArray(
                                new JObject(
                                    new JProperty("kind", "qpxexpress#sliceInput"),
                                    new JProperty("origin", origin),
                                    new JProperty("destination", destination),
                                    new JProperty("date", date.ToString("yyyy-MM-dd"))
                                ),
                                 new JObject(
                                    new JProperty("kind", "qpxexpress#sliceInput"),
                                    new JProperty("origin", destination),
                                    new JProperty("destination", origin),
                                    new JProperty("date", returning_date.ToString("yyyy-MM-dd"))
                                )
                            )
                        )
                    )
                );

            byte[] req_data;

            using (var stream = new MemoryStream())
            {
                using (var jwriter = new JsonTextWriter(new StreamWriter(stream)))
                {
                    req_obj.WriteTo(jwriter);
                    req_data = stream.ToArray();
                }
            }

            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("key", "AIzaSyAQ1FVevSqFvWJUGSeF26WDVqTSU-qDpIQ");

            NameValueCollection headers = new NameValueCollection();

            headers.Add("Content-Type", "application/json");

            byte[] resp_data = await HttpManager.PostAsync(endpoint, api, version, "trips/search",
                parameters, null, headers, req_data);

            JObject resp_obj;

            using (var stream = new MemoryStream(resp_data))
            {
                using (var reader = new JsonTextReader(new StreamReader(stream)))
                {
                    resp_obj = (JObject)JToken.ReadFrom(reader);
                }
            }

            var trips = from trip in resp_obj["trips"]["tripOption"]
                          select new QPXTrip
                          {
                              Cost = parsePrice(trip["saleTotal"].ToString()),
                              Currency = parseCurrency(trip["saleTotal"].ToString()),
                              Code = trip["id"].ToString(),
                              There = createTicket(trip["slice"].First, trip["pricing"].First["saleTotal"].ToString()),
                              Back = createTicket(trip["slice"].ElementAt(1), trip["pricing"].First["saleTotal"].ToString())
                          };

            return trips.ToArray();
        }

        private float parsePrice(string s)
        {
            string price = s.Substring(s.IndexOfAny(numbers));
            return float.Parse(price);
        }

        private string parseCurrency(string s)
        {
            return s.Substring(0, s.IndexOfAny(numbers));
        }

        private Ticket createTicket(JToken slice, string priceStr)
        {
            string date_format = "yyyy-MM-ddThh:MMzzz";

            var first_segment = slice["segment"].First;

            var first_leg = first_segment["leg"].First;

            return new Ticket
            {
                From = first_leg["origin"].ToString(),
                To = first_leg["destination"].ToString(),
                Arrival = DateTime.ParseExact(first_leg["arrivalTime"].ToString(), date_format, CultureInfo.InvariantCulture),
                Departure = DateTime.ParseExact(first_leg["departureTime"].ToString(), date_format, CultureInfo.InvariantCulture),
                Code = first_leg["id"].ToString(),
                Cost = parsePrice(priceStr),
                Currency = parseCurrency(priceStr)
            };
        }
    }
}
