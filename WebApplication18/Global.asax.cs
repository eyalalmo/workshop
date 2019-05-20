using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using workshop192.Bridge;
using workshop192.ServiceLayer;
using workshop192.Domain;
using WebApplication18.Logs;

namespace WebApplication18
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            SystemLogger.configureLogs();
            DomainBridge.getInstance().setup();
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie cookie = Context.Request.Cookies["HashCode"];

            if (cookie == null || UserService.getInstance().getUserByHash(cookie.Value) == -1)
            {
                String hash = UserService.getInstance().generate();
                int session = UserService.getInstance().startSession();
                UserService.getInstance().addUser(hash, session);
                Response.Cookies[""]["HashCode"] = hash;
            }

        }
    }
}