using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{

    public abstract class DiscountComponent
    {
        int id;

        public double percentage;
        public string duration;
        public abstract string getDiscountType();
        public abstract string description();
        public abstract bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice);
        public abstract  Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice);
        protected bool complexCondition;

        public DiscountComponent(double percentage, string duration)
        {
            this.id = DBDiscount.getNextDiscountID();
            this.percentage = percentage;
            this.duration = duration;
            this.complexCondition = false;

        }
        public int getId()
        {
            return this.id;
        }
        public double getPercentage()
        {
            return percentage;
        }
        public void setPercentage(double percentage)
        {
            this.percentage = percentage;
        }
        public string getDuration()
        {
            return duration;
        }
        public bool getComplexCondition()
        {
            return this.complexCondition;
        }
        public abstract void setComplexCondition(bool complexCondition, Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice);

    }
}
