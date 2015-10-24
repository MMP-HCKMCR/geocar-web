using System.Web.Mvc;
using System.Web.Routing;
using System.ServiceModel.Activation;
using GeoCar.WcfService;

namespace GeoCar.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var factory = new WebServiceHostFactory();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add(new ServiceRoute("api", factory, typeof(WebApiService)));

            routes.MapRoute(
                name: "Rental Management Console",
                url: "console/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
