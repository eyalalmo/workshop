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
    public class UesrController : ApiController
    {

        [Route("api/user/register")]
        [HttpGet]
        public string register(String Username, String Password)
        {
            
            try
            {
                Session session = UserService.getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                UserService.getInstance().register(session, Username, Password);

                return "ok";
            } catch(Exception e)
            {
                return "fail";
            }
          
        }

        [Route("api/user/login")]
        [HttpGet]
        public Object login(String Username, String Password)
        {
            try
            {
                Session session = UserService.getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                UserService.getInstance().login(session, Username, Password);

                return "ok";
            }
            catch(Exception e)
            {
                return "fail";
            }
        }

        [Route("api/user/logout")]
        [HttpGet]
        public Object logout()
        {
            try
            {
                Session session = UserService.getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                UserService.getInstance().logout(session);

                return "ok";
            }
            catch (Exception e)
            {
                return "fail";
            }
        }
        [Route("api/user/getAllStores")]
        [HttpGet]
        public string getAllStores()
        {
            try
            {
                string res = "";
                Session session = UserService.getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                LinkedList<Store> s= UserService.getInstance().getAllStores(session);
               foreach (Store s1 in s)
                {
                    res += s1.getStoreID() + "," + s1.getStoreName() + ";";
                }
                return res;

            }
            catch (Exception e)
            {
                string s = "fail";
              
                return s;
            }
        }


    }
}
