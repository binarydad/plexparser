using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Web;

namespace PlexParse.Web.Site.Services
{
    [Export(typeof(IAnalytics))]
    public class TextFileLoggingAnalytics : IAnalytics
    {
        public void Process(HttpContextBase ctx, string message)
        {
            var path = HttpContext.Current.Request.MapPath("/");
            var file = Path.Combine(path, "Logs", "log.txt");

            if (!File.Exists(file))
            {
                var f = File.Create(file);
                f.Close();
            }

            using (var writer = File.AppendText(file))
            {
                writer.WriteLine(String.Format("{0} => {1} - Url:{2}, Total:{3}, Offset:{4}, Message:{5}", DateTime.Now, ctx.Request.UserHostAddress, ctx.Request.Form["url"], ctx.Request.Form["total"], ctx.Request.Form["offset"], message));
            }
        }

        public void Dispose()
        {
            //
        }
    }
}