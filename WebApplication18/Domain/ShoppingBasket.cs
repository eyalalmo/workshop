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
        private string username;

        public ShoppingBasket(string username)
        {
            this.shoppingCarts = new Dictionary<int, ShoppingCart>();
            this.username = username;
        }


        public ShoppingBasket()
        {
            this.shoppingCarts = new Dictionary<int, ShoppingCart>();
            this.username = null;
        }
        public Dictionary<int,ShoppingCart> getShoppingCarts()
        {
            return this.shoppingCarts;
        }


        public double getActualTotalPrice()
        {
            double sum = 0;
            foreach (ShoppingCart sc in shoppingCarts.Values)
            {
                sum += sc.getActualTotalPrice();
            }
            return sum;

        }
        public double getTotalPrice()
        {
            double sum = 0;
            foreach (ShoppingCart sc in shoppingCarts.Values)
            {
                sum += sc.getTotalPrice();
            }
            return sum;

        }
        public void addToCart(Product product, int amount)
        {
            int storeID = product.getStore().getStoreID();
            bool found = false;
            foreach (ShoppingCart sc in shoppingCarts.Values)
            {
                if (sc.getStoreID() == storeID)
                {
                    sc.addToCart(product, amount);
                    return;
                }
            }
            if (!found)
            {
                ShoppingCart sc = new ShoppingCart(storeID);
                sc.addToCart(product, amount);
                {
                    DBSubscribedUser.getInstance().addCart()
                    shoppingCarts.Add(storeID, sc);
                }
            }
        }
        public void removeFromCart(int productId)
        {
            foreach (KeyValuePair<int, ShoppingCart> cart in shoppingCarts)
            {
                Product p = cart.Value.cartContainsProduct(productId);
                if (p != null)
                {
                    cart.Value.removeFromCart(p);
                    return;
                }
            }
            throw new DoesntExistException("Product cannot be removed, it does not exist in cart");
        }

        public ShoppingCart getShoppingCartByID(int storeID)
        {
            foreach (int id in shoppingCarts.Keys)
            {
                if (id == storeID)
                    return shoppingCarts[id];
            }
            return null;
        }
        public void addCoupon(string coupon,int storeID)
        {
            ShoppingCart sc = getShoppingCartByID(storeID);
            if (sc != null)
                sc.addStoreCoupon(coupon);
            else
                throw new DoesntExistException("no such store ID in Shopping basket");
        }

        public void removeCoupon(int storeID)
        {
            ShoppingCart sc = getShoppingCartByID(storeID);
            if (sc != null)
                sc.removeCoupon();
            else
                throw new DoesntExistException("no such store ID in Shopping basket");
        }

        public void purchaseBasket(string address, string creditCard)
        {
            foreach (KeyValuePair<int, ShoppingCart> pair1 in shoppingCarts)
            {
                ShoppingCart cart = pair1.Value;
                Dictionary<Product, int> productsInCart = cart.getProductsInCarts();
                foreach(KeyValuePair<Product,int> pair2 in productsInCart)
                {
                    Product product = pair2.Key;
                    int amount = pair2.Value;
                    if (product.getQuantityLeft() < amount)
                    {
                        
                        throw new IllegalAmountException("Error: Cannot complete purchase- " + product.getProductName() + " does not have enough quantity left");
                    }
                    product.decQuantityLeft(amount);
                }

            }
            Boolean isOk = PaymentService.getInstance().checkOut(creditCard, getActualTotalPrice());
            if (isOk)
            {
                if (DeliveryService.getInstance().sendToUser(address) == false)
                {
                    throw new CartException("Delivery FAILED");
                }
            }
            else
            {
                throw new CartException("Payment FAILED");
            }
        }

        internal void changeQuantityOfProduct(int storeID, Product p, int newAmount)
        {
            ShoppingCart sc = getShoppingCartByID(storeID);
            sc.changeQuantityOfProduct(p, newAmount);

        }
    }
}
