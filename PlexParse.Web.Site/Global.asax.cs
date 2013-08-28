using System.ComponentModel.Composition.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace PlexParse.Web.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes); 

            SetMefControllerFactory();
        }

        private void SetMefControllerFactory()
        {
            // NOTE: This is just the current assembly, not all assemblies in bin
            var catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);

            ControllerBuilder.Current.SetControllerFactory(new MefControllerFactory(container));
        }
    }
}