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
    public class ProductsController : ApiController
    {
        [Route("api/products/getAllProducts")]
        [HttpGet]
        public string getAllProducts()
        {

            string list = UserService.getInstance().getAllProducts();
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
            return list;


        }
    }
}
