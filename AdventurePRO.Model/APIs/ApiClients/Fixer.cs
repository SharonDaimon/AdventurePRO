// Author: Kristina Enikeeva
// Дата: 12.03.2016
// This file contains "fixer" currency rates api logic

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventurePRO.Model.APIs.ApiClients
{
    /// <summary>
    /// Represents a curency rates api client
    /// </summary>
    public class Fixer
    {
        private const string endpoint = "http://api.fixer.io/latest";

        /// <summary>
        /// Queries for currency rates by EUR
        /// </summary>
        /// <returns>Dictionary of currency rates. The base is EUR</returns>
        public async Task<Dictionary<string, float>> GetRatesAsync()
        {
            byte[] data = await HttpManager.GetAsync(endpoint, null, null, null, null, null, null);

            JObject jresponse;

            using (var stream = new MemoryStream(data))
            {
                using (var reader = new JsonTextReader(new StreamReader(stream)))
                {
                    jresponse = (JObject)JToken.ReadFrom(reader);
                }
            }

            IDictionary<string, JToken> dict = (JObject)jresponse["rates"];

            return dict.ToDictionary(t => t.Key, t => (float)t.Value);
        }
    }
}
