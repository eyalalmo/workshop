using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class DiscountComposite : DiscountComponent
    {
        enum Type { or, and, xor };
        private List<DiscountComponent> children;
        private Type type;

        public DiscountComposite(int id, List<DiscountComponent> children, string type) : base(id)
        {
            if (children == null)
            {
                throw new IllegalAmountException();
            }
            this.children = children;
            if (type == "and")
            {
                this.type = Type.and;
            }
            else if (type == "or")
            {
                this.type = Type.or;
            }
            else if (type == "xor")
            {
                this.type = Type.xor;
            }
            else
                throw new IllegalNameException("wrong type name in discount composite");
        }

        
        public override bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            if (type == Type.and)
            {
                bool cond = true;
                foreach (Discount d in children)
                {
                    if (!d.checkCondition(productList, productsActualPrice))
                    {
                        cond = false;
                        break;
                    }
                }
                return cond;
            }
            else if (type == Type.or)
            {
                bool cond = true;
                foreach (Discount d in children)
                {
                    if (!d.checkCondition(productList, productsActualPrice))
                    {
                        cond = false;
                        break;
                    }
                }
                return cond;
            }
            else { 
                int count = 0;
                foreach (Discount d in children)
                {
                    if (d.checkCondition(productList, productsActualPrice))
                    {
                        count++;
                    }
                }
                if (count == 0)
                    return false;
                return true;
            }
        }

        public override Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            bool cond = checkCondition(productList, productsActualPrice);
            if (!cond)
                return productsActualPrice;
            if(type == Type.and )
            {
                foreach (Discount d in children)
                {
                    productsActualPrice = d.updatePrice(productList, productsActualPrice);
                }

            }
            else if (type == Type.or)
            {
                foreach(Discount d in children)
                {
                    if(d.checkCondition(productList, productsActualPrice))
                    {
                        productsActualPrice = d.updatePrice(productList, productsActualPrice);
                    }
                }
            }
            else
            { // xor
                foreach (Discount d in children)
                {
                    if (d.checkCondition(productList, productsActualPrice))
                    {
                        productsActualPrice = d.updatePrice(productList, productsActualPrice);
                        break;
                    }
                }

            }
            return productsActualPrice;
        

        }
    }
}


