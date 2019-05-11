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
    public class ProductsController : ApiController
    {
        [Route("api/products/getAllProducts")]
        [HttpGet]
        public string getAllProducts()
        {
            try
            {
                string list = UserService.getInstance().getAllProducts();
                return list;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        [Route("api/products/searchByName")]
        [HttpGet]
        public string searchByName(string param)
        {
            try
            {
                string list = UserService.getInstance().searchByName(param);
                return list;
            }

            catch (Exception e)
            {
                return e.Message;
            }
        }

        [Route("api/products/searchByCat")]
        [HttpGet]
        public string searchByCat(string param)
        {
            try
            {
                string list = UserService.getInstance().searchByCategory(param);
                return list;
            }
            
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [Route("api/products/searchByKey")]
        [HttpGet]
        public string searchByKey(string param)
        {
            try
            {
                string list = UserService.getInstance().searchByKeyword(param);
                return list;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [Route("api/products/addToBasket")]
        [HttpGet]
        public string addToBasket(int productID, string amount)
        {
            try
            {
                int am = Int32.Parse(amount);
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
              
                UserService.getInstance().addToShoppingBasket(productID, am, session);
                return "ok";
                
            }
            catch (FormatException)
            {
                return "please enter a valid Number";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }

        }
        [Route("api/products/addVisibleDiscount")]
        [HttpGet]
        public string addVisibleDiscount(int productID,string percentage, string duration)
        {
            try
            {
                double per = Double.Parse(percentage);
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addProductVisibleDiscount(productID, session, per, duration);
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


    }
}
