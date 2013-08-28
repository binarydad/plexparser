using Newtonsoft.Json.Linq;
using PlexParse.Web.Site.Enum;
using PlexParse.Web.Site.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace PlexParse.Web.Site.Services
{
    [Export(typeof(DataProcessor))]
    public class YoutubeDataProcessor : DataProcessor
    {
        protected override string GetApiRequestUrl(string inputUrl, int max, int offset)
        {
            var patterns = new[]
                {
                    new { Pattern = "youtube.com/user/(.+)/videos", Type = ListType.User },
                    new { Pattern = "youtube.com/playlist\\?.*list=([a-zA-Z0-9-_]+)", Type = ListType.Playlist },
                    new { Pattern = "youtube.com/course\\?.*list=([a-zA-Z0-9-_]+)", Type = ListType.Course }
                };

            var pattern = patterns.FirstOrDefault(p => Regex.IsMatch(inputUrl, p.Pattern));

            if (pattern != null)
            {
                var identifier = Regex.Match(inputUrl, pattern.Pattern).Groups[1].Value;

                if (pattern.Type == ListType.User)
                {
                    return String.Format("http://gdata.youtube.com/feeds/api/users/{0}/uploads?v=2&alt=jsonc&max-results={1}&start-index={2}", identifier, max, offset);
                }
                else if (pattern.Type == ListType.Playlist || pattern.Type == ListType.Course)
                {
                    return String.Format("https://gdata.youtube.com/feeds/api/playlists/{0}?v=2&alt=jsonc&max-results={1}&start-index={2}", identifier, max, offset);
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
            var json = JObject.Parse(apiResult);

            // Get the items from response
            var items = json["data"]["items"] as JArray;

            // Pull out data from the JSON data
            return items
                .Select(i =>
                {
                    // need to work out better parsing
                    if (i["video"] != null)
                    {
                        i = i["video"];
                    }

                    return new VideoModel
                    {
                        Title = i["title"].Value<string>(),
                        Url = i["player"]["default"]
                            .Value<string>()
                            .Replace("&feature=youtube_gdata_player", String.Empty) // this isnt very sexy
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