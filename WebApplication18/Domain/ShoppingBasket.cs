using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class ShoppingBasket
    {
        private string username = null;
        public Dictionary<int, ShoppingCart> shoppingCarts;

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
            ShoppingCart sc = null;
            foreach (ShoppingCart s in shoppingCarts.Values)
            {
                if (s.getStoreID() == storeID)
                {
                    sc = s;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                sc = new ShoppingCart(storeID);
                if (username != null)
                {
                    DBSubscribedUser.getInstance().addCartToBasketCartTable(username, storeID);
                }
                shoppingCarts.Add(storeID, sc);
            }
            if (username != null)
            {
                DBSubscribedUser.getInstance().addProductToCartProductTable(username, storeID, product.getProductID(), amount);
            }
            sc.addToCart(product, amount);

        }
        public void addToCartNoDBUpdate(Product product, int amount, int storeID)
        {
            //int storeID = product.getStore().getStoreID();
            bool found = false;
            ShoppingCart sc = null;
            foreach (ShoppingCart s in shoppingCarts.Values)
            {
                if (s.getStoreID() == storeID)
                {
                    sc = s;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                sc = new ShoppingCart(storeID);
                shoppingCarts.Add(storeID, sc);
            }
            sc.addToCart(product, amount);

        }
        public void removeFromCart(int productId)
        {
            DBSubscribedUser dbuser = DBSubscribedUser.getInstance();
            foreach (KeyValuePair<int, ShoppingCart> cart in shoppingCarts)
            {
                Product p = cart.Value.cartContainsProduct(productId);
                if (p != null)
                {
                    cart.Value.removeFromCart(p);
                    if (username != null)
                    {
                        dbuser.removeProductFromCartProductTable(username, cart.Value.getStoreID(), productId);
                    }

                    if (cart.Value.CartIsEmpty())
                    {
                        deleteCart(cart.Value);
                       
                    }

                    return;
                }
            }
            throw new DoesntExistException("Product cannot be removed, it does not exist in cart");
        }

        private void deleteCart(ShoppingCart sc)
        {

            foreach (KeyValuePair<int, ShoppingCart> cart in shoppingCarts)
            {
                if (cart.Value.getStoreID() == sc.getStoreID())
                {
                    shoppingCarts.Remove(cart.Key);

                    if (username != null)
                    {
                        DBSubscribedUser.getInstance().deleteCartFromBasketCartTable(username, cart.Value.getStoreID());
                    }
                    return;
                }
            }
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
        public void addCoupon(string coupon, int storeID)
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

        public void purchaseBasket(string address, string creditcard, string month, string year, string holder, string cvv)
        {
            foreach (KeyValuePair<int, ShoppingCart> pair1 in shoppingCarts)
            {
                ShoppingCart cart = pair1.Value;
                Dictionary<Product, int> productsInCart = cart.getProductsInCarts();
                foreach (KeyValuePair<Product, int> pair2 in productsInCart)
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
            int isOk = PaymentService.getInstance().checkOut( address,  creditcard,  month,  year,  holder,  cvv, getActualTotalPrice()).Result;
            if (isOk !=-1)
            {
                if (DeliveryService.getInstance().sendToUser(address, creditcard, month, year, holder, cvv).Result == -1)
                {
                    throw new CartException("Delivery FAILED");
                }
                if (username != null)
                {
                    DBSubscribedUser.getInstance().updateTablesAfterPurchase(username, shoppingCarts);
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
            if (username != null)
            {
                DBSubscribedUser.getInstance().updateAmountOnCartProductTable(username, storeID, p.getProductID(), newAmount);
            }
        }
        internal void setUsername(string username)
        {
            this.username = username;

        }
    }
}