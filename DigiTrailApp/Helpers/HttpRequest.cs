using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Util;

namespace DigiTrailApp.Helpers {
    class HttpRequest {

        /// <summary>
        /// Fetch an image from a given URL
        /// </summary>
        /// <param name="query">Search term(s)</param>
        /// <returns>Json response</returns>
        public async Task<byte[]> GetImage(string url)
        {
            var result = await CreateHttpRequest(url, "photo");
            return result;
        }


        /// <summary>
        /// Fetch result in string format
        /// </summary>
        /// <param name="url">URL of the resource</param>
        /// <returns>String</returns>
        public async Task<string> GetString(string url)
        {
            var result = await CreateHttpRequest(url, "string");
            return result;
        }

        /// <summary>

        /// <summary>
        /// Creates HTTP request using <see cref="HttpClient"/>, gets HTTP response using <see cref="HttpResponseMessage"/>
        /// and returns response in format determined by output paremeter.
        /// </summary>
        /// <param name="url">Target of the HTTP request</param>
        /// <param name="output">Determines type of the returned response</param>
        /// <returns>Response</returns>
        private async Task<dynamic> CreateHttpRequest(string url, string output) {

            HttpClient client;
            HttpResponseMessage response = null;

                client = new HttpClient();
                response = await client.GetAsync(url);

            switch (output)
            {
                case "string":
                    var @string = await response.Content.ReadAsStringAsync();
                    return @string;

                case "photo":
                    var photo = await response.Content.ReadAsByteArrayAsync();
                    return photo;

                case "stream":
                    var stream = await response.Content.ReadAsStreamAsync();
                    return stream;

                default:
                    response.Dispose();
                    throw new ArgumentException("No valid output given or provided output is null or empty"); //TODO: Translate
            }
        }
    }
}