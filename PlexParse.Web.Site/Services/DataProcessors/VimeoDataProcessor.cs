using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlexParse.Web.Site.Enum;
using PlexParse.Web.Site.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace PlexParse.Web.Site.Services
{
    [Export(typeof(DataProcessor))]
    public class VimeoDataProcessor : DataProcessor
    {
        protected override string GetApiRequestUrl(string inputUrl, int max, int offset)
        {
            var patterns = new[]
                {
                    new { Pattern = "vimeo.com/([a-zA-Z0-9-_]+)", Type = ListType.User }
                };

            var pattern = patterns.FirstOrDefault(p => Regex.IsMatch(inputUrl, p.Pattern));

            if (pattern != null)
            {
                var identifier = Regex.Match(inputUrl, pattern.Pattern).Groups[1].Value;

                // max?
                if (pattern.Type == ListType.User)
                {
                    return String.Format("http://vimeo.com/api/v2/{0}/videos.json", identifier);
                }
            }

            return null;
        }

        protected override string ProcessApiRequest(string apiUrl)
        {
            var client = new WebClient();
            return client.DownloadString(apiUrl);
        }

        protected override IEnumerable<VideoModel> ParseApiResult(string apiResult)
        {
            // Parse to JSON...
            var json = JsonConvert.DeserializeObject(apiResult) as JArray;

            // Pull out data from the JSON data
            return json
                .Select(i =>
                {
                    return new VideoModel
                    {
                        Title = i["title"].Value<string>(),
                        Url = i["url"].Value<string>()
                    };
                })
                .ToList();
        }

        public override void Dispose()
        {
            //
        }
    }
}