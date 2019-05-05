using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class DBDiscount
    {
        private static DBDiscount instance;
        private Dictionary<int, Discount> discounts;
        private static int nextID;

        public static DBDiscount getInstance()
        {
            if (instance == null)
            {
                instance = new DBDiscount();
            }
            return instance;
        }

        private DBDiscount()
        {
            discounts = new Dictionary<int, Discount>();
            nextID = 1;
        }


        public void init()
        {
            discounts = new Dictionary<int, Discount>();
            nextID = 1;
        }
        public void addDiscount(Discount d)
        {
            if (discounts.ContainsKey(d.getId()))
                throw new AlreadyExistException("session already exists");
            discounts.Add(d.getId(), d);
        }
        public void removeDiscount(Discount d)
        {
            if (!discounts.ContainsKey(d.getId()))
                throw new DoesntExistException("session does not exist, can't remove it");
            discounts.Remove(d.getId());
        }
        public static int getNextDiscountID()
        {
            int id = nextID;
            nextID++;
            return id;
        }

    }
}

