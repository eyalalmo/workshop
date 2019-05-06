using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication18.Controllers
{
    public class LoadController : Controller
    {
        // GET: Load
        public ActionResult StoreId(int storeId)
        {
           
            ViewData["StoreId"] = storeId;
            return View();
        }
    }
}