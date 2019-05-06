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

            int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
            UserService.getInstance().register(session, Username, Password);
 
           return "ok";
    
          
        }

        [Route("api/user/login")]
        [HttpGet]
        public Object login(String Username, String Password)
        {
            //Session session = UserService.getInstance().startSession();
            int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
            UserService.getInstance().login(session, Username, Password);
           //String hash = System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value;
           // UserService.addUser(hash, session);
            return "ok";
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
            //Session session = UserService.getInstance().startSession();
            int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
            string basket = UserService.getInstance().getShoppingBasket(session);
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, basket);
            //String hash = System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value;
            // UserService.addUser(hash, session);
            string response = basket;
            /*foreach (KeyValuePair<int, ShoppingCart> cart in basket.getShoppingCarts())
            {
                foreach (KeyValuePair<Product, int> p in cart.Value.getProductsInCarts())
                {
                    response+= p.Key.getProductName() + "," + p.Key.getPrice() + "," + p.Key.getProductID() +"," + p.Value + ";";
                }
            }*/


            return response;
        }
        [Route("api/user/removeProductFromCart")]
        [HttpGet]
        public string removeProductFromCart(int productId)
        {
            //Session session = UserService.getInstance().startSession();
            int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);

            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, basket);
            //String hash = System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value;
            // UserService.addUser(hash, session);
            UserService.getInstance().removeFromShoppingBasket(session, productId);
            return "ok";
           
        }


    }
}
