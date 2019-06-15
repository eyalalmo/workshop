using System;
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
                SystemLogger.getEventLog().Error("getStoreByID Error: " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: getStore; Stack Trace: " + e.StackTrace);
                throw e;
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
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: addOwner; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }

        [Route("api/store/getStoreProducts")]
        [HttpGet]
        public string getStoreProducts(int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                string s = StoreService.getInstance().getProducts(storeId);
                return s;
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Error in store display : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: getStoreProducts; Stack Trace: " + e.StackTrace);
                throw e;
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
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch(ArgumentException e)
            {
                return "cant open store with no name";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: getStore; Stack Trace: " + e.StackTrace);
                throw e;
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
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Edit product error : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Edit Product; Stack Trace: " + e.StackTrace);
                throw e;
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
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Get All Roles; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }

        [Route("api/store/getAllPending")]
        [HttpGet]
        public string getAllPending(int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().getAllPending(storeId,session);

            }
            catch (ClientException e)
            {
                return e.Message.ToString();
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Get All Roles; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }

        [Route("api/store/signContract")]
        [HttpGet]
        public string signContract(string username, int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().signContract(storeId, username, session);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Error in signing a contract : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: signContract; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }

        [Route("api/store/declineContract")]
        [HttpGet]
        public string declineContract(string username, int storeId)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().declineContract(storeId, username, session);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Error in declining a contract : " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: declineContract; Stack Trace: " + e.StackTrace);
                throw e;
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
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Add Discount; Stack Trace: " + e.StackTrace);
                throw e;
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
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Add Discount; Stack Trace: " + e.StackTrace);
                throw e;
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
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Add Discount; Stack Trace: " + e.StackTrace);
                throw e;
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
                //WebSocketController.messageClient(username, "you have no longer a role in store " + storeId);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Remove Role Error: " + e.Message.ToString());
                return e.Message.ToString();
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Remove Role; Stack Trace: " + e.StackTrace);
                throw e;
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
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
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
                SystemLogger.getEventLog().Error("Add Product Error : " + e.Message.ToString());
                return e.Message;
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Add Product; Stack Trace: " + e.StackTrace);
                throw e;
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
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Delete Product; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }


        [Route("api/store/SetMaxPolicy")]
        [HttpGet]
        public string SetMaxPolicy(int storeID, int maxVal)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addMaxAmountPolicy(storeID, session, maxVal);
                 return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Policy edit :" + e.Message.ToString());
                return e.Message;
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Set max policy; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }

        [Route("api/store/SetMinPolicy")]
        [HttpGet]
        public string SetMinPolicy(int storeID, int minVal)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addMinAmountPolicy(storeID, session, minVal);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Policy edit :" + e.Message.ToString());
                return e.Message;
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Set Min Policy; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }
       /* [Route("api/store/GetMaxPolicy")]
        [HttpGet]
        public string GetMaxPolicy(int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
           //    return StoreService.getInstance().getMaxAmountPolicy(storeID,session);
               
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
          //       StoreService.getInstance().removeMaxAmountPolicy(storeID, session);
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
         //       StoreService.getInstance().removeMinAmountPolicy(storeID, session);
                return "ok";

            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Policy Delete :" + e.Message.ToString());
                return e.Message;
            }
        }
        */
        [Route("api/store/StoreDiscounts")]
        [HttpGet]
        public string StoreDiscounts(int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().getStoreDiscounts(storeID, session);
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Get Store Discounts; Stack Trace: " + e.StackTrace);
                throw e;
            }
           
        }
        [Route("api/store/StorePolicies")]
        [HttpGet]
        public string StorePolicies(int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                return StoreService.getInstance().getStorePolicies(storeID, session);
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Get Store Policies; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }


        [Route("api/store/setDiscountPercentage")]
        [HttpGet]
        public Object setDiscountPercentage(int discountID, int percentage)
        {
            try
            {
                double p = percentage / 100.0;
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().setDiscountPercentage(discountID, p);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("discount percentage :" + e.Message.ToString());
                return e.Message;
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Set Discount Percentage; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }

        [Route("api/store/setPolicyAmount")]
        [HttpGet]
        public Object setPolicyAmount(int policyID, int amount, int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().setPolicyAmount(policyID, amount, session, storeID);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Policy amount :" + e.Message.ToString());
                return e.Message;
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Set Policy Amount; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }

        [Route("api/store/removeDiscount")]
        [HttpGet]
        public Object removeDiscount(int discountID,int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().removeStoreDiscount(discountID,storeID, session);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("remove discount :" + e.Message.ToString());
                return e.Message;
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Remove Discount; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }
        [Route("api/store/removePolicy")]
        [HttpGet]
        public Object removePolicy(int policyID, int storeID)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().removeStorePolicy(policyID, storeID, session);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("remove policy :" + e.Message.ToString());
                return e.Message;
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Remove policy; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }

        [Route("api/store/complexDiscount")]
        [HttpGet]
        public Object complexDiscount(string discounts, int storeID, string type, double percentage, string duration)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().complexDiscount(discounts,storeID,type,percentage,duration,session);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("complex discount :" + e.Message.ToString());
                return e.Message;
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: complex discount; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }
        //url: baseUrl+"/api/store/payAndSend?address=" + address + "&creditcard=" + creditcard + "&month=" + month + "&year=" + year + "&holder=" + holder + "&cvv=" + cvv + "&creditcard=" + creditcard,
        [Route("api/store/complexPolicy")]
        [HttpGet]
        public Object complexPolicy(string policies, int storeID, string type)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().complexPolicy(policies, storeID, type, session);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("complex policy :" + e.Message.ToString());
                return e.Message;
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: complex policy; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }
        [Route("api/store/AddTotalPolicy")]
        [HttpGet]
        public Object AddTotalPolicy(int storeID, int totalVal)
        {
            try
            {
                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                StoreService.getInstance().addTotalPolicy(storeID,totalVal,session);
                return "ok";
            }
            catch (ClientException e)
            {
                SystemLogger.getEventLog().Error("Add to total policy:" + e.Message.ToString());
                return e.Message;
            }
            catch (ConnectionException e)
            {
                SystemLogger.getEventLog().Error("Database Error : " + e.Message.ToString());
                return "There has been a problem with the connection to the database. Please try again.";
            }
            catch (Exception e)
            {
                SystemLogger.getErrorLog().Error("An Error has occured. Function: Add Total Policy; Stack Trace: " + e.StackTrace);
                throw e;
            }
        }


    }
}

