using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class ReliantDiscount : Discount
    {
        public enum reliantType {singleProduct, multiProducts,totalAmount}
        private double percentage;
        private String duration;
        private reliantType type;
        int amount;
        int totalAmount;
        Dictionary<Product, int> products;

        public ReliantDiscount(double percentage, String duration, int amount, string type) : base(percentage, duration)
        {
            if(type == "singleProduct")
            {
                this.amount = amount;
                this.type = reliantType.singleProduct;
            }
            else if( type == "totalAmount")
            {
                this.totalAmount = amount;
                this.type = reliantType.totalAmount;
            }

        }

        public ReliantDiscount(double percentage, String duration, Dictionary<Product, int> products) : base(percentage, duration)
        {
            this.type = reliantType.multiProducts;
            this.products = products;
        }



        public override bool checkCondition()
        {
            throw new NotImplementedException();
        }
    }
}
