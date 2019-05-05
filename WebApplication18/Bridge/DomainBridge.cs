using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.Bridge
{
    class DomainBridge
    {
        private static DomainBridge instance;
        private static DBSession sessionDb;

        public static DomainBridge getInstance()
        {
            if (instance == null)
            {
                instance = new DomainBridge();
                sessionDb = DBSession.getInstance();
            }
            return instance;
        }


        public Dictionary<int, ShoppingCart> getShoppingCarts(Session user)
        {
            Session session = sessionDb.get
            return user.getShoppingBasket().getShoppingCarts();
        }
    }
}
