using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication18.Logs;
using workshop192.ServiceLayer;
using workshop192.Domain;


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
            catch(ClientException e)
            {
                SystemLogger.getEventLog().Error("Register : " + e.Message.ToString());
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
            catch(ClientException e)
            {
                SystemLogger.getEventLog().Error("Login : " + e.Message.ToString());
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
            catch(ClientException e)
            {
                SystemLogger.getEventLog().Error("Logout : " + e.Message.ToString());
                return e.Message;
            }
        }


        [Route("api/user/getShoppingBasket")]
        [HttpGet]
        public string getShoppingBasket()
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return UserService.getInstance().getShoppingBasket(session);
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }

        }
        [Route("api/user/checkBasket")]
        [HttpGet]
        public string checkBasket()
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return UserService.getInstance().checkBasket(session);
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }

        }
        [Route("api/user/setProductQuantity")]
        [HttpGet]
        public string setProductQuantity(int product, int quantity)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                BasketService.getInstance().changeQuantity(session, product, quantity);
                return "ok";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }

        }

        [Route("api/user/basketTotalPrice")]
        [HttpGet]
        public string basketTotalPrice()
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return "" + BasketService.getInstance().getActualTotalPrice(session)+","+BasketService.getInstance().getTotalPrice(session);
            }
            catch (Exception e)
            {

                return e.Message.ToString();
            }
        }

        [Route("api/user/removeProductFromCart")]
        [HttpGet]
        public string removeProductFromCart(int productId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);

            UserService.getInstance().removeFromShoppingBasket(session, productId);
            return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Remove from Cart : " + e.Message.ToString());
                return e.Message.ToString();
            }

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
                return e.Message.ToString();
            }
        }

        [Route("api/user/Checkout")]
        [HttpGet]
        public string Checkout(string address, string creditcard, string month, string year, string holder, string cvv)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                UserService.getInstance().purchaseBasket(session, address,  creditcard,  month,  year,  holder,  cvv);
                return "";

            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Checkout : " + e.Message.ToString());
                return e.Message.ToString();
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
            catch (ClientException e)
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
                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


    }
}
 

