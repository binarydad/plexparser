using PlexParse.Web.Site.Models;
using System;
using System.Web;

namespace PlexParse.Web.Site.Services
{
    /// <summary>
    /// Used for any logging of requests
    /// </summary>
    public interface IAnalytics : IDisposable
    {
        void Process(HttpContextBase ctx, string message);
    }
}
