using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class ShoppingBasket
    {
        private Dictionary<int,ShoppingCart> shoppingCarts;

        public ShoppingBasket()
        {
            this.shoppingCarts = new Dictionary<int, ShoppingCart>();
        }

        public Dictionary<int,ShoppingCart> getShoppingCarts()
        {
            return this.shoppingCarts;
        }
        public void addToCart(Product product, int amount)
        {
            int storeID = product.getStoreID();
            bool found = false;
            foreach(ShoppingCart sc in shoppingCarts.Values)
                if (sc.getStoreID() == storeID)
                {
                    sc.addToCart(product,amount);
                    found = true;
                    break;
                }
            if(!found)
            {
                ShoppingCart sc = new ShoppingCart(storeID);
                sc.addToCart(product, amount);
            }
        }
        public void checkout (){    ////////// TODO ///////////
            foreach(ShoppingCart sc in shoppingCarts.Values)
            {
                sc.checkout();            }
        }
    }
}
