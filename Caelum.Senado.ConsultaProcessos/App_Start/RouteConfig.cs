using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Confere.ProcessosWeb.Consulta
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Processos do Senado", 
                url: "senado/{sigla}/{numero}/{ano}",
                defaults: new { controller = "Senado", action = "Index" }
            );

            routes.MapRoute(name: "Processos da Câmara", url: "camara/{sigla}/{numero}/{ano}");
        }
    }
}
