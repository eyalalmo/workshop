using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace workshop192.Domain
{
    public class StoreRoleEntry
    {
        public int storeId;
        public string appointedBy;
        public string userName;
        public int isOwner;
        public  int editProduct;
        public int editDiscount;
        public int editPolicy;

        public StoreRoleEntry(int storeId, string appointedBy, string userName, int isOwner, int editProduct, int editDiscount, int editPolicy)
        {
            this.storeId = storeId;
            this.appointedBy = appointedBy;
            this.userName = userName;
            this.isOwner = isOwner;
            this.editProduct = editProduct;
            this.editDiscount = editDiscount;
            this.editPolicy = editPolicy;
        }
        public int getStoreId()
        {
            return storeId;
        }
        public string getAppointedBy()
        {
            return appointedBy;
        }
        public string getUserName()
        {
            return userName;
        }
        public int getIsOwner()
        {
            return isOwner;
        }
        public int getEditProduct()
        {
            return editProduct;
        }
        public int getEditDiscount()
        {
            return editDiscount;
        }
        public int getEditPolicy()
        {
            return editPolicy;
        }

    }
}