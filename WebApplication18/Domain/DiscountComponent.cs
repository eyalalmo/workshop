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
        public DateTime duration;
        public abstract string getDiscountType();
        public abstract string description();
        public abstract bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice);
        public abstract  Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice);
        protected bool complexCondition;

        public DiscountComponent(double percentage, string duration)
        {
            this.id = DBDiscount.getNextDiscountID();
            this.percentage = percentage;
            this.duration = stringToDate(duration);
            this.complexCondition = false;

        }

        private DateTime stringToDate(string duration)
        {
            int day = Int32.Parse(duration.Substring(0, 2));
            int month = Int32.Parse(duration.Substring(3, 2));
            int year = Int32.Parse(duration.Substring(6, 4));
            return new DateTime(year, month, day);
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
        public DateTime getDuration()
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
