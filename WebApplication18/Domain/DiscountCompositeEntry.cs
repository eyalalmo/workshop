using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication18.Domain
{
    public class DiscountCompositeEntry
    {
        private int id;
        private int childid;
        private string type;
        public DiscountCompositeEntry(int id, int childid, string type)
        {
            this.id = id;
            this.childid = childid;
            this.type = type;
        }
        public int getId()
        {
            return this.id;
        }
        public int getchildid()
        {
            return this.childid;
        }
        public string getType()
        {
            return this.type;
        }
    }
}