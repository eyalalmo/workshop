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
        

       
        public DiscountComposite(List<DiscountComponent> children, string type, double percentage, string duration, int storeId) : base(percentage, duration, storeId)
        {
            if (children == null)
            {
                throw new AlreadyExistException();
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
                throw new IllegalNameException("Error: Wrong type name in discount composite");
        }
        public DiscountComposite(int id, List<DiscountComponent> children, string type, double percentage, string duration, int storeId, bool isPartOfComplex) : base(id, percentage, duration, storeId, isPartOfComplex)
        {
            if (children == null)
            {
                throw new AlreadyExistException();
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
                throw new IllegalNameException("Error: Wrong type name in discount composite");
        }
        public string getType()
        {
            if(type == Type.or)
            {
                return "or";
            }
            if (type == Type.and)
            {
                return "and";
            }
            else
                return "xor";
        }
        public override string getDiscountType()
        {
            return "Complex";
        }

        public override string description()
        {
            string str = "(";
            int i;
            for(i=0; i<children.Count-1; i++)
            {
                DiscountComponent dis = children.ElementAt(i);
                str = str + dis.description();
                str = str +" " +type+ " ";
            }
            DiscountComponent dis2 = children.ElementAt(i);
            str = str + dis2.description()+")";
            return str;
            
        }
        public List<DiscountComponent> getChildren()
        {
            return this.children;
        }
        public override bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            if (type == Type.and)
            {
                bool cond = true;
                foreach (DiscountComponent d in children)
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
                bool cond = false;
                foreach (DiscountComponent d in children)
                {
                    if (d.checkCondition(productList, productsActualPrice))
                    {
                        cond = true;
                        break;
                    }
                }
                return cond;
            }
            else { 
                int count = 0;
                foreach (DiscountComponent d in children)
                {
                    if (d.checkCondition(productList, productsActualPrice))
                    {
                        count++;
                    }
                }
                if (count>0)
                    return true;
                return false;
            }
        }
        /*public override bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
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
            else
            {
                int count = 0;
                foreach (Discount d in children)
                {
                    if (d.checkCondition(productList, productsActualPrice))
                    {
                        count++;
                    }
                }
                if (count == 0 || count > 1)
                    return false;
                return true;
            }
        }*/
        public override Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            bool cond = checkCondition(productList, productsActualPrice);
            if (!cond)
                return productsActualPrice;
            if(type == Type.and )
            {
                foreach (DiscountComponent d in children)
                {
                    productsActualPrice = d.updatePrice(productList, productsActualPrice);
                }

            }
            else if (type == Type.or)
            {
                foreach(DiscountComponent d in children)
                {
                    if(d.checkCondition(productList, productsActualPrice))
                    {
                        productsActualPrice = d.updatePrice(productList, productsActualPrice);
                    }
                }
            }
            else
            { // xor
                foreach (DiscountComponent d in children)
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

        public override void setComplexCondition(bool complexCondition, Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            if (type == Type.xor && complexCondition)
            {
                bool found = false;
                foreach (DiscountComponent d in children)
                {
                    if (!found)
                    {
                        if (d.checkCondition(productList, productsActualPrice)){
                            d.setComplexCondition(complexCondition, productList, productsActualPrice);
                            found = true;
                        }
                    }
                    else
                    {
                        d.setComplexCondition(false, productList, productsActualPrice);
                    }

                }
            }
            else
            {
                foreach (DiscountComponent d in children)
                {
                    d.setComplexCondition(complexCondition, productList, productsActualPrice);

                }
            }
        }
    }
}


