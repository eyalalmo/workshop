using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication18.Domain
{
    public class Contract
    {
        public int storeId;
        public string userName;
        public string approvedBy;


        public Contract(int storeId, string userName, string approvedBy)
        {
            this.storeId = storeId;
            this.approvedBy = approvedBy;
            this.userName = userName;
        }
    }
}