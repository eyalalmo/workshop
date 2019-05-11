using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace workshop192.Domain
{
    public class ShoppingCart
    {

        private Dictionary<Product, int> productList;
        private Dictionary<Product, double> productsActualPrice;
        private int storeID;
        private Store store;
        private string coupon;


        public ShoppingCart(int storeID)
        {
            productList = new Dictionary<Product, int>();
            productsActualPrice = new Dictionary<Product, double>();

            this.storeID = storeID;
            store = DBStore.getInstance().getStore(storeID);
            coupon = "";
        }
        public Dictionary<Product, int> getProductsInCarts()
        {
            return productList;
        }
        public int getStoreID()
        {
            return this.storeID;
        }
        public Product cartContainsProduct(int productId)
        {
            foreach (KeyValuePair<Product, int> p in productList)
            {
                if (p.Key.getProductID() == productId)
                    return p.Key;
            }
            return null;
        }
        public void addToCart(Product product, int amount)
        {
            store.checkPolicy(product, amount);

            int quantityLeft = product.getQuantityLeft();
            if (quantityLeft - amount >= 0)
            {
                if (productList.ContainsKey(product))
                    throw new CartException("Error: Product already exists on Cart");
                productList.Add(product, amount);
                productsActualPrice.Add(product, product.getPrice());
            }
            else
            {
                throw new AlreadyExistException("Error: The amount asked is larger than the quantity left");
            }
        }

        public void addStoreCoupon(string couponCode)
        {
            if (coupon != "")
                throw new AlreadyExistException("Error: Can not have more than one coupon in shopping cart");
            store.checkCouponCode(couponCode);
            coupon = couponCode;
        }


        public void removeCoupon()
        {
            coupon = "";
        }


        public void removeFromCart(Product p)
        {
            if (!productList.ContainsKey(p))
                throw new CartException("error- product does not exist");
            productList.Remove(p);
            productsActualPrice.Remove(p);
          
        }

       

        public void changeQuantityOfProduct(Product p, int newAmount)
        {
            if (!productList.ContainsKey(p))
                throw new CartException("error - cart does not contains product");
            store.checkPolicy(p, newAmount);

            int oldAmount = productList[p];
            int quantity = p.getQuantityLeft();
            if (quantity + oldAmount - newAmount < 0)
            {
                throw new AlreadyExistException("error - The amount asked is larger than the quantity left");
            }
            productList.Remove(p);
            productList.Add(p, newAmount);

        }
        public double getTotalPrice()
        {
            updatePriceAfterCoupon();
            updateActualProductPrice();
            updateStoreDiscount();
            double sum = 0;
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                Product p = entry.Key;
                double actualPrice = productsActualPrice[p];
                sum += (entry.Value * actualPrice);
            }

            return sum;
        }
        
            private void updatePriceAfterCoupon()
        {
            if (coupon != "")
                productsActualPrice = store.updatePriceAfterCoupon(coupon, productList, productsActualPrice);
        }

        private void updateStoreDiscount()
        {
            productsActualPrice = store.updatePrice(productList, productsActualPrice);
        }

        private void updateActualProductPrice()
        {
            productsActualPrice = new Dictionary<Product, double>();
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                Product p = entry.Key;
                double actual = p.getActualPrice();
                if (actual != entry.Value)
                {
                    productsActualPrice[p] = actual;
                }
            }
        }

       /* public void checkout(String address,String creditCard) {
           // String res = "";
            int sum = 0;
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                if (entry.Key.getQuantityLeft() > entry.Value)
                {
                    
                    sum = entry.Key.getPrice() * entry.Value;
                    Boolean isOk = PaymentService.getInstance().checkOut(creditCard,sum);
                    if (isOk)
                    {
                        entry.Key.setQuantityLeft(entry.Key.getQuantityLeft() - entry.Value);

                        if (DeliveryService.getInstance().sendToUser(address, entry.Key) == false)
                        {
                            entry.Key.setQuantityLeft(entry.Key.getQuantityLeft() + entry.Value);
                            throw new CartException("Cannot deliver " + entry.Key.getProductName());
                        }
                    }
                    else
                    {
                        throw new CartException("Payment for " + entry.Key.getProductName());
                    }
                }
                else
                {
                    throw new CartException("Not enough quantity of " + entry.Key.getProductName() );
                }

            }
        } */
    }
}
