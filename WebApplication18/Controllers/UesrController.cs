using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using workshop192.Domain;
using workshop192.ServiceLayer;


namespace WebApplication18.Controllers
{
    public class UesrController : ApiController
    {

        [Route("api/user/register")]
        [HttpGet]
        public string register(String Username, String Password)
        {

            Session session = UserService.getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
            String ans = UserService.getInstance().register(session, Username, Password);
 
                return "ok";
    
          
        }

        [Route("api/user/login")]
        [HttpGet]
        public Object login(String Username, String Password)
        {
            Session session = UserService.getInstance().startSession();
            String ans = UserService.getInstance().login(session, Username, Password);
            String hash = System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value;
            UserService.addUser(hash, session);
            return "ok";
        }



    }
}
