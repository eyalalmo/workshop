﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication18.Logs;
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
            catch (ClientException e)
            {
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: getStore; Stack Trace: " + e.StackTrace);
                return "error";
            }
        }
        [Route("api/store/addOwner")]
        [HttpGet]
        public string addOwner(string username, int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addOwner(storeId, username, session);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Error in adding an owner : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: addOwner; Stack Trace: " + e.StackTrace);
                return "error";
            }
        }

        [Route("api/store/getStoreProducts")]
        [HttpGet]
        public string getStoreProducts(int storeId)
        {
            try
            {
                
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().getProducts(storeId);
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Error in store display : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: getStoreProducts; Stack Trace: " + e.StackTrace);
                return "error";
            }
        }


        [Route("api/store/addStore")]
        [HttpGet]
        public string addStore(string name, string description)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addStore(name, description, session);

                return "ok";


            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Error in adding a store : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: getStore; Stack Trace: " + e.StackTrace);
                return "error";
            }
        }



        [Route("api/store/SetProductInformation")]
        [HttpGet]
        public string SetProductInformation(int storeID, int productID, int price, int rank, int quantityLeft, string productName)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().SetProductInformation(storeID, productID, price, rank, quantityLeft, productName, session);
                return "ok";

            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Edit product error : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Edit Product; Stack Trace: " + e.StackTrace);
                return "error";
            }
        }

        [Route("api/store/getAllRoles")]
        [HttpGet]
        public string getAllRoles(int storeId)
        {
            try
            {
                return StoreService.getInstance().getAllRoles(storeId);

            }
            catch (ClientException e)
            {
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Get All Roles; Stack Trace: " + e.StackTrace);
                return "error";
            }
        }

        [Route("api/store/addVisibleDiscount")]
        [HttpGet]
        public string addVisibleDiscount(int storeID, string percentage, string duration)
        {
            try
            {
                double per = Double.Parse(percentage);
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addStoreVisibleDiscount(storeID, per, duration, session);
                return "";

            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Add Visible Discount : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Add Discount; Stack Trace: " + e.StackTrace);
                return "error";
            }
        }

        [Route("api/store/addReliantDiscountTotalAmount")]
        [HttpGet]
        public string addReliantDiscountTotalAmount(int storeID, int totalAmount, string percentage, string duration)
        {
            try
            {
                double per = Double.Parse(percentage);
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addReliantDiscountTotalAmount(storeID, session, per, duration, totalAmount);
                return "";

            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Add Reliant Discount : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Add Discount; Stack Trace: " + e.StackTrace);
                return "error";
            }
        }
        [Route("api/store/addReliantDiscountSameProduct")]
        [HttpGet]
        public string addReliantDiscountSameProduct(int storeID, string percentage, string duration, int numOfProducts, int productID)
        {
            try
            {
                double per = Double.Parse(percentage);
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addReliantDiscountSameProduct(storeID, session, per, duration, numOfProducts, productID);
                return "";

            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Add Reliant Discount : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Add Discount; Stack Trace: " + e.StackTrace);
                return "error";
            }
        }




        [Route("api/store/removeRole")]
        [HttpGet]
        public string removeRole(string username, int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().removeRole(storeId, username, session);
                return "ok";

            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Remove Role Error: " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Remove Role; Stack Trace: " + e.StackTrace);
                return "error";
            }


        }
        
     
        [Route("api/store/isOwner")]
        [HttpGet]
        public bool isOwner(int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().isOwner(storeId, session);

            }
            catch (Exception)
            {
                return false;
            }
        }

        [Route("api/store/addManager")]
        [HttpGet]
        public string addManager(string username, int storeId, bool prod, bool disc, bool poli)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addManager(storeId, username, prod, disc, poli, session);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Add Manager : " + e.Message.ToString());
                return e.Message.ToString();
            }
        }




        [Route("api/store/isManager")]
        [HttpGet]
        public bool isManager(int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().isManager(storeId, session);
                

            }
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Add Product : " + e.Message.ToString());
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
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Delete Product : " + e.Message.ToString());
                return e.Message;
            }
        }


        [Route("api/store/SetMaxPolicy")]
        [HttpGet]
        public string SetMaxPolicy(int storeID, int maxVal)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().setMaxAmountPolicy(storeID, session, maxVal);
                 return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Policy edit :" + e.Message.ToString());
                return e.Message;
            }
        }

        [Route("api/store/SetMinPolicy")]
        [HttpGet]
        public string SetMinPolicy(int storeID, int minVal)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().setMinAmountPolicy(storeID, session, minVal);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Policy edit :" + e.Message.ToString());
                return e.Message;
            }
        }
        [Route("api/store/GetMaxPolicy")]
        [HttpGet]
        public string GetMaxPolicy(int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
               return StoreService.getInstance().getMaxAmountPolicy(storeID,session);
               
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Policy get :" + e.Message.ToString());
                return "fail";
            }
        }
        [Route("api/store/GetMinPolicy")]
        [HttpGet]
        public string GetMinPolicy(int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().getMinAmountPolicy(storeID, session);

            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Policy get :" + e.Message.ToString());
                return "fail";
            }
        }
     

        [Route("api/store/DeleteMaxPolicy")]
        [HttpGet]
        public string DeleteMaxPolicy(int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                 StoreService.getInstance().removeMaxAmountPolicy(storeID, session);
                 return "ok";

            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Policy Delete :" + e.Message.ToString());
                return  e.Message;
            }
        }

        [Route("api/store/DeleteMinPolicy")]
        [HttpGet]
        public string DeleteMinPolicy(int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().removeMinAmountPolicy(storeID, session);
                return "ok";

            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Policy Delete :" + e.Message.ToString());
                return e.Message;
            }
        }




    }
}

