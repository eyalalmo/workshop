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
    public class AdminPanController : ApiController
    {
        [Route("api/AdminPan/removeUser")]
        [HttpGet]
        public string removeUser(String username)
        {
            try
            {
                Session session = UserService.getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                UserService.getInstance().removeUser(session, username);
                return "ok";
            }
            catch (Exception e) {
                return e.Message;
            }
        }
    }
}
