using PlexParse.Web.Site.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using PlexParse.Web.Site.Models;
using MarriedGeek;

namespace PlexParse.Web.Site.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeController : AsyncController
    {
        #region Public Properties
        [ImportMany]
        public IEnumerable<DataProcessor> DataProcessors { get; set; }

        [ImportMany]
        public IEnumerable<IAnalytics> Analytics { get; set; } 
        #endregion

        #region HttpGet
        public ActionResult Index()
        {
            return View();
        } 
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult Parse(string url, int total = 50, int offset = 1)
        {
            #region Input checking
            if (offset < 1)
            {
                offset = 1;
            }

            if (total < 0 || total > 50)
            {
                total = 50;
            }
            #endregion

            var urls = DataProcessors
                .AsParallel()
                .SelectMany(p => p.ProcessMediaFromUrl(url, total, offset).EmptyIfNull())
                .ToList();

            #region Stats
            // handle async here?
            Analytics.ForEach(a => a.Process(HttpContext, String.Format("{0} returned", urls.IfNotNull(u => u.Count()))));
            #endregion

            return View(urls);
        } 
        #endregion
    }
}
