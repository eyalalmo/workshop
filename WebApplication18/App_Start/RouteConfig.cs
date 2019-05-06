using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace WebApplication18
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
          //  var settings = new FriendlyUrlSettings();
           // settings.AutoRedirectMode = RedirectMode.Permanent;
            // routes.EnableFriendlyUrls(settings);

              routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                    "login",
                    "{action}/{id}",
                    new
                    {
                        controller = "Pages",
                        action = "Default",
                        id = UrlParameter.Optional
                    }
            );
            routes.MapRoute(
                name: "Default",
                url: "api/{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
       
    }
    }
  


            
