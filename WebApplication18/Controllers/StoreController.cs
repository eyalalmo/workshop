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
                return StoreService.getInstance().getProducts(storeId);
                

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

        [Route("api/store/addVisibleDiscount")]
        [HttpGet]
        public string addVisibleDiscount(int storeID,string percentage, string duration)
        {
            try
            {
                double per = Double.Parse(percentage);
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addStoreVisibleDiscount(storeID, per, duration, session);
                return "";

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [Route("api/store/addReliantDiscountTotalAmount")]
        [HttpGet]
        public string addReliantDiscountTotalAmount(int storeID,int totalAmount, string percentage, string duration)
        {
            try
            {
                double per = Double.Parse(percentage);
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addReliantDiscountTotalAmount(storeID, session, per, duration, totalAmount);
                return "";

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        [Route("api/store/addReliantDiscountSameProduct")]
        [HttpGet]
        public string addReliantDiscountSameProduct(int storeID,string percentage, string duration, int numOfProducts, int productID)
        {
            try
            {
                double per = Double.Parse(percentage);
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addReliantDiscountSameProduct(storeID, session, per, duration, numOfProducts, productID);
                return "";

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }




    }
}

