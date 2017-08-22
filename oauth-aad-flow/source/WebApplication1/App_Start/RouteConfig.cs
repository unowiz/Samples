using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1 {

    public static class RouteConfig {

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{Resource}.axd/{*PathInfo}");
            routes.MapRoute(
                "Default",
                "{Controller}/{Action}/{Id}",
                new {
                    Controller = "Home",
                    Action = "Index",
                    Id = UrlParameter.Optional,
                }
            );
        }

    }

}
