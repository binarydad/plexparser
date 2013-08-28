using PlexParse.Web.Site.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace PlexParse.Web.Site.Services
{
    /// <summary>
    /// Processes videos from an API or other result string
    /// </summary>
    public abstract class DataProcessor : IDisposable
    {
        #region Abstract Methods
        /// <summary>
        /// Retrieves the API URL string matching the inputted URL request
        /// </summary>
        /// <param name="inputUrl">URL from the website</param>
        /// <param name="total">Maximum records to retrieve</param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected abstract string GetApiRequestUrl(string inputUrl, int total, int offset);

        /// <summary>
        /// Retrieves the result string from the API request
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        protected abstract string ProcessApiRequest(string apiUrl);

        /// <summary>
        /// Deserialize API result string into VideoModel collection
        /// </summary>
        /// <param name="apiResult"></param>
        /// <returns></returns>
        protected abstract IEnumerable<VideoModel> ParseApiResult(string apiResult);
        public abstract void Dispose(); 
        #endregion

        public IEnumerable<VideoModel> ProcessMediaFromUrl(string url, int max, int offset)
        {
            // Parses the user URL to determine the appropriate API request URL
            var apiUrl = GetApiRequestUrl(url, max, offset);

            if (apiUrl != null)
            {
                // Retrieves the API result data string
                var apiResult = ProcessApiRequest(apiUrl);

                if (apiResult != null)
                {
                    // Takes data string and creates video objects
                    return ParseApiResult(apiResult).Take(max); // ensure max is honored even if API doesn't
                }
            }

            return null;
        }
    }
}
