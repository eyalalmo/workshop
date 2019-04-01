using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class ShoppingCart
    {
        private LinkedList<Product> productList;

        public ShoppingCart()
        {
            productList = new LinkedList<Product>();
        }

        public String addToCart(Product product, int amount)
        {
            int quantity = product.getQuantityLeft();
            if ( quantity - amount> 0)
            {
                product.setQuantityLeft(quantity - amount);
                return "";
            }

            return "- product quantity";

        }
    }
}
