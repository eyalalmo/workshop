using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication18.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult login(string user )
        {
            ViewData["user"] = user;
            return View();
        }


    }
}