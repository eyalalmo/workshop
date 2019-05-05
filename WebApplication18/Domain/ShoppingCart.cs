using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace workshop192.Domain
{
    public class ShoppingCart
    {

        private Dictionary <Product, int> productList;
        private Dictionary<Product, double> productsActualPrice;
        private int storeID;
        private Store store;


        public ShoppingCart(int storeID)
        {
            productList = new Dictionary<Product, int>();
            this.storeID = storeID;
            store = DBStore.getInstance().getStore(storeID);
        }
        public Dictionary<Product, int> getProductsInCarts()
        {
            return productList;
        }
        public int getStoreID()
        {
            return this.storeID;
        }

        public void addToCart(Product product, int amount)
        {
            int quantityLeft = product.getQuantityLeft();
            if ( quantityLeft - amount>= 0)
            {
                if (productList.ContainsKey(product))
                    throw new CartException( "error: product exist");
                productList.Add(product, amount);
            }

            throw new IllegalAmountException("The amount asked is larger than the quantity left");

        }

        public void removeFromCart(Product p)
        {
            if (!productList.ContainsKey(p))
                throw new CartException("error- product does not exist");
            productList.Remove(p);
          
        }

        public void changeQuantityOfProduct(Product p, int newAmount)
        {
            if (!productList.ContainsKey(p))
                throw new CartException("error - cart does not contains product");
            int oldAmount = productList[p];
            int quantity = p.getQuantityLeft();
            if (quantity + oldAmount - newAmount < 0)
            {
                throw new IllegalAmountException("error - The amount asked is larger than the quantity left");
            }
            productList.Remove(p);
            productList.Add(p, newAmount);

        }
        public int getTotalAmount()
        {
           // updateActualProductPrice();
            updateStoreDiscount();
            double sum = 0;
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                Product p = entry.Key;
                double actualPrice = productsActualPrice[p];
                sum += (entry.Key.getPrice() * actualPrice);
            }

            return sum;
        }

        private void updateStoreDiscount()
        {
            productsActualPrice = store.calcPrice(productList, productsActualPrice);

        }

        private void updateActualProductPrice()
        {
            productsActualPrice = new Dictionary<Product, double>();
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                Product p = entry.Key;
                productsActualPrice.Add(p, p.getActualPrice());
            }
        }

        public String checkout(String address,String creditCard) {
            String res = "";
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
                            res += " product: " + entry.Key.getProductID() + " can't deliver.\n ";
                        }
                        else
                        {
                            res += " product: " + entry.Key.getProductID() + " complete payment. ";
                        }

                        //////////add eilon part
                    }
                    else
                    {
                        res += " product: " + entry.Key.getProductID() + " cant submmit checkout.\n ";
                    }
                }
                else
                {
                    res += " product: " + entry.Key.getProductID() + " have no this Quantity.\n ";
                }

            }
            return res;


        } 
    }
}
