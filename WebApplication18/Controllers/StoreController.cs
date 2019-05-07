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
        [Route("api/store/getStoreById")]
        [HttpGet]
        public string getStoreByID(int storeId)
        {
            try
            {
                return StoreService.getInstance().getStore(storeId);
            }
            catch (Exception e)
            {
                string s = "fail";

                return s;
            }
        }
        [Route("api/store/addOwner")]
        [HttpGet]
        public string addOwner(string username,int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addOwner(storeId, username, session);
                return "ok";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }

        [Route("api/store/getStoreProducts")]
        [HttpGet]
        public string getStoreProducts(int storeId)
        {
            try
            {
                string res = "";
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().getProducts(storeId);
                

            }
            catch (Exception e)
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
            catch (Exception e)
            {
                return -1;
            }
        }



        [Route("api/store/SetProductInformation")]
        [HttpGet]
        public string SetProductInformation(int storeID, int productID, int price, int rank,int quantityLeft,string productName)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                 StoreService.getInstance().SetProductInformation(storeID,productID,price,rank, quantityLeft,productName,session);
                return "ok";

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }




    }
}

