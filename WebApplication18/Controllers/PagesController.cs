using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication18.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult AllProducts()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
    }
}