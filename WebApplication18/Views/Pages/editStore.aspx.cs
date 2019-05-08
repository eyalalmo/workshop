using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using workshop192.ServiceLayer;

namespace WebApplication18.Views.Pages
{
    public partial class EditStore : System.Web.Mvc.ViewPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (System.Web.HttpContext.Current.Request.Cookies["HashCode"] != null)
            {
                //    string u = UserService.getStateName(System.Web.HttpContext.Current.Request.Cookies["HashCode"].Value);
                //    bool job = StoreService.getInstance().isOwner(ViewData["storeId"], u);
                //    if (u != null && job == true)
                //    {
                    
                //}


                

            }
        }
    }
}