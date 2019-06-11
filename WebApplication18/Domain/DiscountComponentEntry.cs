using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication18.Domain
{
    public class DiscountComponentEntry
    {
        private int id;
        private double percentage;
        private string duration;
        private string type;
        private int storeId;
        public DiscountComponentEntry(int id, double percentage, string duration, string type, int storeId)
        {
            this.id = id;
            this.percentage = percentage;
            this.duration = duration;
            this.type = type;
            this.storeId = storeId;
        }
        public int getId()
        {
            return this.id;
        }
        public double getPercentage()
        {
            return this.percentage;
        }
        public string getDuration()
        {
            return this.duration;
        }
        public string getType()
        {
            return this.type;
        }
        public int getStoreId()
        {
            return this.storeId;
        }
    }
}