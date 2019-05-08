﻿using System;
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
     
        [Route("api/store/isOwner")]
        [HttpGet]
        public bool isOwner(int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().isOwner(storeId, session);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }


        [Route("api/store/isManager")]
        [HttpGet]
        public bool isManager(int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().isManager(storeId, session);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        //IsAllowedToEditProduct
        [Route("api/store/IsEP")]
        [HttpGet]
        public bool IsEP(int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().isAllowedToEditProduct(storeId, session);
               

            }
            catch (Exception e)
            {
                return false;
            }
        }
        //isAllowedToEditDiscount
        [Route("api/store/IsED")]
        [HttpGet]
        public bool IsED(int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().isAllowedToEditDiscount(storeId, session);
               

            }
            catch (Exception e)
            {
                return false;
            }
        }

        //isAllowedToEditProduct
        [Route("api/store/IsEPo")]
        [HttpGet]
        public bool IsEPo(int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().isAllowedToEditPolicy(storeId, session);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [Route("api/store/addProduct")]
        [HttpGet]
        public string addProduct(string productName, string productCategory, int price, int rank, int quantityLeft, int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addProduct(productName, productCategory, price, rank, quantityLeft, storeID, session);
                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [Route("api/store/deleteProduct")]
        [HttpGet]
        public string deleteProduct(int storeID, int productID) { 
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                 StoreService.getInstance().removeProduct(productID,session);
                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


    }
}

