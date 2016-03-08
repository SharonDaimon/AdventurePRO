// Author: Kristina Enikeeva
// Дата: 08.03.2016
// This file contains some Http requests methods

using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Collections.Specialized;

namespace AdventurePRO.Model.APIs
{
    /// <summary>
    /// The class that contains the query http methods
    /// </summary>
    public static class HttpManager
    {
        /// <summary>
        /// Creates client with headers
        /// </summary>
        /// <param name="headers">Headers collection</param>
        /// <returns>Web client with assigned headers</returns>
        public static WebClient CreateClient(NameValueCollection headers)
        {
            WebClient client = new WebClient();
            client.Headers.Add(headers);
            return client;
        }

        /// <summary>
        /// Creates request url string with assigned parameters
        /// </summary>
        /// <param name="endpoint">The endpoint to which need to send request</param>
        /// <param name="api">The api name of given endpoint. Must be null if not given</param>
        /// <param name="version">The api version. Must be null if not given</param>
        /// <param name="method">The api method</param>
        /// <param name="parameters">The request parameters. Must be null if not given</param>
        /// <param name="fragment">The fragment. Must be null if not given</param>
        /// <returns>The request url with given parameters</returns>
        public static string CreateRequestUrl(string endpoint, string api, string version, string method, NameValueCollection parameters, string fragment)
        {
            string slash = "/";
            string query = endpoint;
            query += api != null ? slash + api : "";
            query += version != null ? slash + version : "";
            query += method != null ? slash + method : "";

            if (parameters != null && parameters.Count > 0)
            {
                var params_array = from key in parameters.AllKeys
                                   select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(parameters[key]));
                query += "?"  + string.Join("&", params_array);
            }

            query += fragment != null ? "#" + fragment : "";

            return query;
        }

        /// <summary>
        /// Creates GET http method with given parameters
        /// </summary>
        /// <param name="endpoint">The endpoint to which need to send request</param>
        /// <param name="api">The api name of given endpoint. Must be null if not given</param>
        /// <param name="version">The api version. Must be null if not given</param>
        /// <param name="method">The api method</param>
        /// <param name="parameters">The request parameters. Must be null if not given</param>
        /// <param name="fragment">The fragment. Must be null if not given</param>
        /// <param name="headers">Request headers</param>
        /// <returns>The data returned by server</returns>
        public static async Task<byte[]> GetAsync(string endpoint, string api, string version, string method, 
            NameValueCollection parameters, string fragment, NameValueCollection headers)
        {
            var url = CreateRequestUrl(endpoint, api, version, method, parameters, fragment);
            using (var client = CreateClient(headers))
            {
                return await client.DownloadDataTaskAsync(url);
            }
        }

        /// <summary>
        /// Creates POST http method with given parameters
        /// </summary>
        /// <param name="endpoint">The endpoint to which need to send request</param>
        /// <param name="api">The api name of given endpoint. Must be null if not given</param>
        /// <param name="version">The api version. Must be null if not given</param>
        /// <param name="method">The api method</param>
        /// <param name="parameters">The request parameters. Must be null if not given</param>
        /// <param name="fragment">The fragment. Must be null if not given</param>
        /// <param name="headers">Request headers</param>
        /// <param name="data">The data to post</param>
        /// <returns>The data returned by server</returns>
        public static async Task<byte[]> PostAsync(string endpoint, string api, string version, string method,
            NameValueCollection parameters, string fragment, NameValueCollection headers, byte[] data)
        {
            var url = CreateRequestUrl(endpoint, api, version, method, parameters, fragment);
            using (var client = CreateClient(headers))
            {
                return await client.UploadDataTaskAsync(url, data);
            }
        }
    }
}
