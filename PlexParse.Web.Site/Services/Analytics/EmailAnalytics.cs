using System.ComponentModel.Composition;
using System.Web;

namespace PlexParse.Web.Site.Services
{
    [Export(typeof(IAnalytics))]
    public class EmailAnalytics : IAnalytics
    {
        public void Process(HttpContextBase ctx, string message)
        {
            // nothing yet
            // ensure is async
        }

        public void Dispose()
        {
            //
        }
    }
}