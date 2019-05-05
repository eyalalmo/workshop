using System;
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
                  String u = UserService.getStateName(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);

                if (u != null && u== "LoggedIn")
                {
                    logout.Visible = true;
                    login.Visible = false;
                    register.Visible = false;
                }
                
            }
        }




    
    }
}