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
            sc.addToCartNoDBUpdate(product, amount);

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
        //public void addCoupon(string coupon, int storeID)
        //{
        //    ShoppingCart sc = getShoppingCartByID(storeID);
        //    if (sc != null)
        //        sc.addStoreCoupon(coupon);
        //    else
        //        throw new DoesntExistException("no such store ID in Shopping basket");
        //}

        //public void removeCoupon(int storeID)
        //{
        //    ShoppingCart sc = getShoppingCartByID(storeID);
        //    if (sc != null)
        //        sc.removeCoupon();
        //    else
        //        throw new DoesntExistException("no such store ID in Shopping basket");
        //}

        public async Task<int> purchaseBasket(string address, string creditcard, string month, string year, string holder, string cvv)
        {
        //    foreach (KeyValuePair<int, ShoppingCart> pair1 in shoppingCarts)
        //    {
        //        ShoppingCart cart = pair1.Value;
        //        Dictionary<Product, int> productsInCart = cart.getProductsInCarts();
        //        foreach (KeyValuePair<Product, int> pair2 in productsInCart)
        //        {
        //            Product product = pair2.Key;
        //            int amount = pair2.Value;
        //            if (product.getQuantityLeft() < amount)
        //            {

        //                throw new IllegalAmountException("Error: Cannot complete purchase- " + product.getProductName() + " does not have enough quantity left");
        //            }
        //            product.decQuantityLeft(amount);
        //        }

        //    }

            Task<int> result =  PaymentService.getInstance().checkOut(address, creditcard, month, year, holder, cvv);
            int res = await result;
            int resFromDelivery2;
            if (res != -1)
            {
                Task<int> resFromDelivery = DeliveryService.getInstance().sendToUser(address, creditcard, month, year, holder, cvv);
                 resFromDelivery2 = await resFromDelivery;
                if (resFromDelivery2==-1)
                {
                    Task<int> res3=PaymentService.getInstance().cancelPayment(res+"");
                    int res3Ans = await res3;
                    throw new CartException("Delivery FAILED");
                }
                if (username != null)
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
                    DBSubscribedUser.getInstance().updateTablesAfterPurchase(username, shoppingCarts);
                }
            }
            else
            {
                throw new CartException("Payment FAILED");
            }
            //throw new SuccessPaymentExeption("OK");
            return resFromDelivery2;
        }
        public void checkBasket()
        {
            int numOfProducts = 0;
            Product productToRemove = null;
            foreach (KeyValuePair<int, ShoppingCart> pair1 in shoppingCarts)
            {
                ShoppingCart cart = pair1.Value;
                Store store = DBStore.getInstance().getStore(cart.getStoreID());
                Dictionary<Product, int> productsInCart = cart.getProductsInCarts();
                foreach (KeyValuePair<Product, int> pair2 in productsInCart)
                {
                    numOfProducts++;
                    Product product = pair2.Key;
                    int amount = pair2.Value;
                    store.checkPolicy(product, amount);
                    if (product.getQuantityLeft() == 0)
                    {
                        productToRemove = product;
                        break;
                    }
                    if (product.getQuantityLeft() < amount)
                    {
                        cart.changeQuantityOfProduct(product, product.getQuantityLeft());
                        throw new IllegalAmountException("Total quantity of product: "+product.getProductName()+" ID: "+product.getProductID()+"\n is larger than the quantity left in the store\nquantity has been set to maximum quantity left in store\nPlease checkout again");
                    }
                    product.decQuantityLeft(amount);
                }
                if (productToRemove != null)
                {
                    removeFromCart(productToRemove.getProductID());
                    throw new IllegalAmountException("Prodcut: "+ productToRemove.getProductName() + " ID: "+productToRemove.getProductID()+" has ZERO quantity left\n Product has been removed from cart\nPlease checkout again");
                }
                   

            }
            if (numOfProducts == 0)
            {
                throw new DoesntExistException("Checkout failed. there are no items in the basket for purchase.");
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