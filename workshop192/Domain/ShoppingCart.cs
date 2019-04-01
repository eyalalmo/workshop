using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class ShoppingCart
    {
        private Dictionary <Product, int> productList;

        public ShoppingCart()
        {
            productList = new Dictionary<Product, int>();
        }

        public String addToCart(Product product, int amount)
        {
            int quantityLeft = product.getQuantityLeft();
            if ( quantityLeft - amount> 0)
            {
                product.setQuantityLeft(quantityLeft - amount);
                productList.Add(product, amount);
                return "";
            }

            return "- product quantity";

        }

        public String removeFromCart(Product p)
        {
            if (!productList.ContainsKey(p))
               return "- product does not exist";
            productList.Remove(p);
            return "";
            
        }

        public String changeQuantityOfProduct(Product p, int newAmount)
        {
            if (!productList.ContainsKey(p))
                return "- product does not exist";
            int oldAmount = productList[p];
            int quantity = p.getQuantityLeft();
            if (quantity + oldAmount - newAmount < 0)
                return "- quantity below zero";
            p.setQuantityLeft(quantity + oldAmount - newAmount);
            productList.Remove(p);
            productList.Add(p, newAmount);
            return "";

        }
    }
}
