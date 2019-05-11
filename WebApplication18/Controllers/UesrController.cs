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
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                UserService.getInstance().register(session, Username, Password);
                return "ok";
            }
            catch(Exception e)
            {
                return e.Message;
            }

        }

        [Route("api/user/login")]
        [HttpGet]
        public Object login(String Username, String Password)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                UserService.getInstance().login(session, Username, Password);
                return "ok";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        [Route("api/user/logout")]
        [HttpGet]
        public Object logout()
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                UserService.getInstance().logout(session);
                return "ok";
            }
            catch( Exception e)
            {
                return e.Message;
            }
        }
        [Route("api/user/getAllProducts")]
        [HttpGet]
        public string getAllProducts(String Username, String Password)
        {

            //LinkedList<Product> list = UserService.getInstance().getAllProducts();
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
            //return response;
            return "ok";


        }


        [Route("api/user/getShoppingBasket")]
        [HttpGet]
        public string getShoppingBasket()
        {
            int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
            return UserService.getInstance().getShoppingBasket(session);
        }

        [Route("api/user/basketTotalPrice")]
        [HttpGet]
        public string basketTotalPrice()
        {
            //Session session = UserService.getInstance().startSession();
            int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
            return ""+BasketService.getInstance().getTotalPrice(session);
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, basket);
            //String hash = System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value;
            // UserService.addUser(hash, session);
            //string response = basket;
            /*foreach (KeyValuePair<int, ShoppingCart> cart in basket.getShoppingCarts())
            {
                foreach (KeyValuePair<Product, int> p in cart.Value.getProductsInCarts())
                {
                    response+= p.Key.getProductName() + "," + p.Key.getPrice() + "," + p.Key.getProductID() +"," + p.Value + ";";
                }
            }*/


            // return response;
        }

        [Route("api/user/removeProductFromCart")]
        [HttpGet]
        public string removeProductFromCart(int productId)
        {
            
            int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);

            UserService.getInstance().removeFromShoppingBasket(session, productId);
            return "ok";

        }
        [Route("api/user/getAllStores")]
        [HttpGet]
        public string getAllStores()
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return UserService.getInstance().getAllStores(session);
              
            }
            catch (Exception e)
            {
               return  e.Message;
            }
        }

        [Route("api/user/Checkout")]
        [HttpGet]
        public string Checkout(string address, string creditCard)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                UserService.getInstance().purchaseBasket(session, address, creditCard);
                return "";

            }
            catch (Exception e)
            {
                string s = e.Message;
                return s;
            }
        }


        [Route("api/user/removeUser")]
        [HttpGet]
        public string removeUser(String username)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                UserService.getInstance().removeUser(session, username);
                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [Route("api/user/waitingMessages")]
        [HttpGet]
        public Object waitingMessages()
        {
            try
            {
                WebSocketController.messageClient("et", "");
                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }
}
 

