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

        double percentage;
        public abstract string getDiscountType();
        public abstract string description();
        public abstract bool checkCondition(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice);
        public abstract  Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice);

        public DiscountComponent(double percentage)
        {
            this.id = DBDiscount.getNextDiscountID();
            this.percentage = percentage;
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

    }
}
