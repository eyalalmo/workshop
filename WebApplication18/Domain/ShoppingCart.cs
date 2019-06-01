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
        public double getActualTotalPrice()
        {
            productsActualPrice = new Dictionary<Product, double>();
            fillActualPriceDic();
            //updateActualProductPrice();
           // updatePriceAfterCoupon();
            //updateStoreDiscount();
            double sum = 0;
            LinkedList<DiscountComponent> discounts = store.getDiscounts();

            foreach (DiscountComponent dis in discounts)
            {
                if (dis is DiscountComposite)
                {
                    if (dis.checkCondition(productList, productsActualPrice))
                    {
                        dis.setComplexCondition(true, productList, productsActualPrice);
                    }
                    else
                    {
                        dis.setComplexCondition(false, productList, productsActualPrice);
                    }

                }
            }
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                    Product p = entry.Key;
                    double actualPrice = p.getActualPrice(entry.Value);
                    sum += (entry.Value * actualPrice);
            }
         
            foreach (DiscountComponent dis in discounts)
            {
                if (dis is ReliantDiscount)
                {
                    ReliantDiscount r = (ReliantDiscount)dis;
                    if (!r.getIsPartOfComplex())
                    {
                        if (r.isTotalAmountDiscount() && sum >= r.getTotalAmount())
                            sum = sum * (1 - r.getPercentage());
                    }
                }
          


            }

            foreach (DiscountComponent dis in discounts)
            {
                if (dis is VisibleDiscount)
                {
                    VisibleDiscount v = (VisibleDiscount)dis;
                    if (!v.getIsPartOfComplex()&&v.isStoreVisibleDiscount())
                    {
                        if (v.isStoreVisibleDiscount())
                            sum = sum * (1 - v.getPercentage());
                    }
                }

            }

            return sum;
        }

        public double getTotalPrice()
        {
           
            double sum = 0;
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                Product p = entry.Key;
                sum += (entry.Value * p.getPrice());
            }

            return sum;
        }

        private void fillActualPriceDic()
        {
            foreach(KeyValuePair<Product,int> entry in productList)
            {
                Product p = entry.Key;
                productsActualPrice.Add(p, entry.Value);
            }
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
            
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                Product p = entry.Key;
                double actual = p.getActualPrice(entry.Value);
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
