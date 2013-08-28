using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlexParse.Web.Site.Services;
using System.Linq;

namespace PlexParse.Tests
{
    [TestClass]
    public class ParseTests
    {
        [TestMethod]
        public void Youtube_List()
        {
            var youtube = new YoutubeDataProcessor();

            var media = youtube.ProcessMediaFromUrl("http://www.youtube.com/playlist?feature=addto&list=PLvXXzsIQxhtS0xtw47KAGaDYdX945QnQm", 50, 1);

            Assert.IsTrue(media.Count() > 0);
        }

        [TestMethod]
        public void Vimeo_List()
        {
            var vimeo = new VimeoDataProcessor();

            var media = vimeo.ProcessMediaFromUrl("https://vimeo.com/nlsi/videos", 50, 1);

            Assert.IsTrue(media.Count() > 0);
        }
    }
}
