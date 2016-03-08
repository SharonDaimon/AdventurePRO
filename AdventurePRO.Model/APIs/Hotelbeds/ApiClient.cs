// Author: Kristina Enikeeva
// Дата: 08.03.2016
// This file contains general hotelbeds apitude access logic

using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace AdventurePRO.Model.APIs.Hotelbeds
{
    /// <summary>
    /// This class is designed to receive data via the hotelbeds API
    /// </summary>
    public partial class ApiClient
    {
        /// <summary>
        /// API key by default
        /// </summary>
        public const string DEFAULT_KEY = "vg8b333xg5bxfvd5khfz2mgg";

        /// <summary>
        /// API shared secret by default
        /// </summary>
        public const string DEFAULT_SECRET = "gqA5MBZfAH";

        /// <summary>
        /// API endpoint
        /// </summary>
        public const string ENDPOINT = "https://api.test.hotelbeds.com";

        /// <summary>
        /// Default language
        /// </summary>
        public const string LANG = "ENG";

        /// <summary>
        /// Creates an ApiClient with given key and secret
        /// </summary>
        /// <param name="key">Api key</param>
        /// <param name="secret">Api shared secret</param>
        public ApiClient(string key, string secret)
        {
            this.Key = key;
            this.Secret = secret;
        }

        /// <summary>
        /// API key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Api shared secret
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// Creates a GET request to apitude
        /// </summary>
        /// <param name="api">An api</param>
        /// <param name="api_version">An api version</param>
        /// <param name="method">An api method</param>
        /// <param name="parameters">Requets parameters</param>
        /// <returns>A request result (Response body)</returns>
        public async Task<byte[]> GetAsync(string api, string api_version, string method, NameValueCollection parameters)
        {
            return await HttpManager.GetAsync(ENDPOINT, api, api_version, method, parameters, null, createHeaders(Key, Secret));
        }

        /// <summary>
        /// Creates a GET request to apitude
        /// </summary>
        /// <param name="api">An api</param>
        /// <param name="api_version">An api version</param>
        /// <param name="method">An api method</param>
        /// <param name="parameters">Requets parameters</param>
        /// <param name="data">A data to be posted</param>
        /// <returns>A request result (Response body)</returns>
        public async Task<byte[]> PostAsync(string api, string api_version, string method, NameValueCollection parameters, byte[] data)
        {
            return await HttpManager.PostAsync(ENDPOINT, api, api_version, method, parameters, null, createHeaders(Key, Secret), data);
        }

        // Creates an X-Signature for hotelbeds apitude
        private static string createSignature(string key, string secret)
        {
            /*
            Code below is taken from
            https://developer.hotelbeds.com/docs (".Net C# code")
            */
            using (var sha = SHA256.Create())
            {
                long ts = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000;
                Console.WriteLine("Timestamp: " + ts);
                var computedHash = sha.ComputeHash(Encoding.UTF8.GetBytes(key + secret + ts));

                return BitConverter.ToString(computedHash).Replace("-", "");
            }
        }

        // Creates request headers
        private static NameValueCollection createHeaders(string key, string secret)
        {
            NameValueCollection headers = new NameValueCollection();

            headers.Add("X-Signature", createSignature(key, secret));
            headers.Add("Api-Key", key);
            headers.Add("Content-Type", "application/xml");
            headers.Add("Accept", "application/xml");

            return headers;
        }
    }
}
