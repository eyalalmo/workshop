using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication18.Controllers
{
    public class PagesController : Controller
    {
        // GET: Load
       
        public ActionResult Store(int storeId)
        {
            ViewData["storeId"] = storeId;
            return View();
        }
       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Default()
        {
            return View();
        }

        public ActionResult login()
        {
            return View();
        }
        public ActionResult register()
        {
            return View();
        }
        public ActionResult about()
        {
            return View();
        }
        public ActionResult AddStore()
        {
            return View();
        }
        public ActionResult AllProduct()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult MyAccount()
        {
            return View();
        }
        public ActionResult EditStore()
        {
            return View();
        }
        public ActionResult Basket()
        {
            return View();
        }
        public ActionResult AllProducts()
        {
            return View();
        }
        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult Discounts(int productID)
        {
            ViewData["productID"] = productID;
            return View();
        }
        public ActionResult ViewStore(int storeId)
        {
            ViewData["storeId"] = storeId;
            return View();
        }
        public ActionResult StoreDiscount(int storeId)
        {
            ViewData["storeID"] = storeId;
            return View();
        }

        public ActionResult StoreDiscounts(int storeId)
        {
            ViewData["storeID"] = storeId;
            return View();
        }

        public ActionResult ManageStaff(int storeId)
        {
            ViewData["storeId"] = storeId;
            return View();
        }

        public ActionResult AddProduct(int storeId)
        {
            ViewData["storeId"] = storeId;
            return View();
        }
        

        public ActionResult EditStorePolicy(int storeId)
        {
            ViewData["storeId"] = storeId;
            return View();
        }
        public ActionResult AdminPan()
        {
            return View();
        }
        



    }
}