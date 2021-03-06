﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using workshop192.Domain;
using workshop192.ServiceLayer;

namespace WebApplication18
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (System.Web.HttpContext.Current.Request.Cookies["HashCode"] != null)
            {
                string u = UserService.getStateName(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);

                if (u != null && u == "LoggedIn")
                {
                    logout.Visible = true;
                    login.Visible = false;
                    register.Visible = false;
                    myAccount.Visible = true;
                }
                if (u != null && u == "Admin")
                {
                    logout.Visible = true;
                    login.Visible = false;
                    register.Visible = false;
                    Admin.Visible = true;
                    myAccount.Visible = true;
                }

                int session = UserService.getInstance().getUserByHash(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                if (session == -1)
                {
                    Basket.InnerHtml = "<i class=\"fa fa-fw fa-shopping-cart\"></i>0";
                    myAccount.InnerHtml = "< i class=\"fa fa-fw fa-user\"></i><i class=\"fa fa-fw fa-sort-down\"></i>";
                }
                else
                {
                    Basket.InnerHtml = "<i class=\"fa fa-fw fa-shopping-cart\"></i>" + 
                        UserService.getInstance().getNumOfProductsInBasket(session).ToString();
                    myAccount.InnerHtml = "<i class=\"fa fa-fw fa-user\"></i>" +
                                UserService.getInstance().getUserNameBySession(session) +
                                "<i class=\"fa fa-fw fa-sort-down\"></i>";
                }
            }
        }
    }
}