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
    public class StoreController : ApiController
    {


        [Route("api/store/getStoreProducts")]
        [HttpGet]
        public string getStoreProducts(int storeId)
        {
            try
            {
                string res = "";
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                LinkedList<Product> p = StoreService.getInstance().getProducts(storeId);
                foreach (Product p1 in p)
                {
                    res += p1.getProductID() + "," + p1.getProductName() + ";";
                }
                return res;

            }
            catch (Exception)
            {
                string s = "fail";

                return s;
            }
        }


        [Route("api/store/addStore")]
        [HttpGet]
        public int addStore(string name, string description)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
               return  StoreService.getInstance().addStore(name, description, session);
              

            }
            catch (Exception)
            {
                return -1;
            }
        }
    



    }
}

