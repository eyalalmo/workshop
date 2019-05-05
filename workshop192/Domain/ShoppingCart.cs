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
        private int storeID;

        public ShoppingCart(int storeID)
        {
            productList = new Dictionary<Product, int>();
            this.storeID = storeID;
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

        public void removeFromCart(int p)
        {
            if (!productList.ContainsValue(p))
                throw new CartException("error- product does not exist");
            productList.Remove(DBProduct.getInstance().getProductByID(p));
          
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
            int sum = 0;
            foreach (KeyValuePair<Product, int> entry in productList)
            {
                sum += (entry.Key.getPrice() * entry.Value);
            }
            return sum;
        }
        public void checkout(String address,String creditCard) {
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
        } 
    }
}
