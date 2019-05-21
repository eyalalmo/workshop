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
        private Dictionary<int, DiscountComponent> discounts;
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
            discounts = new Dictionary<int, DiscountComponent>();
            nextID = 1;
        }


        public void init()
        {
            discounts = new Dictionary<int, DiscountComponent>();
            nextID = 1;
        }
        public void addDiscount(DiscountComponent d)
        {
            if (discounts.ContainsKey(d.getId()))
                throw new AlreadyExistException("Error: Discount already exists");
            discounts.Add(d.getId(), d);
        }
        public void removeDiscount(int d)
        {
            if (!discounts.ContainsKey(d))
                throw new DoesntExistException("Error: Discount does not exist");
            discounts.Remove(d);
        }
        public static int getNextDiscountID()
        {
            int id = nextID;
            nextID++;
            return id;
        }

        public DiscountComponent getDiscountByID(int id)
        {
            discounts.TryGetValue(id, out DiscountComponent value);
            return value;
        }

    }
}

