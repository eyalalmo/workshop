using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class Permissions
    {
       public  Dictionary<string, bool> per;
    
        public Permissions(bool editProduct, bool editDiscount, bool editPolicy)
        {
            per = new Dictionary<string, bool>();
            per.Add("editProduct", editProduct);
            per.Add("editDiscount", editDiscount);
            per.Add("editPolicy", editPolicy);
        }

        public bool editProduct()
        {
            return per["editProduct"];
        }

        public bool editDiscount()
        {
            return per["editDiscount"];
        }

        public bool editPolicy()
        {
            return per["editPolicy"];
        }

        public void setEditProduct(bool editProduct)
        {
            per["editProduct"] = editProduct;
        }

        public void setEditDiscount(bool editDiscount)
        {
            per["editDiscount"] = editDiscount;
        }

        public void setEditPolicy(bool editPolicy)
        {
            per["editPolicy"] = editPolicy;
        }
    }
}
