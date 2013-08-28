using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace PlexParse.Web.Site
{
    public class MefControllerFactory : DefaultControllerFactory
    {
        private CompositionContainer _container;

        public MefControllerFactory(CompositionContainer container)
        {
            this._container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var controller = this._container.GetExports(controllerType, null, null).FirstOrDefault();

            if (controller != null)
            {
                return controller.Value as IController;
            }

            return base.GetControllerInstance(requestContext, controllerType);
        }
    }
}