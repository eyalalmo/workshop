using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class DBProduct
    {
        public static DBProduct  instance;
        LinkedList<Product> productList;
        int nextProductID;

        public static DBProduct getInstance()
        {
            if (instance == null)
                instance = new DBProduct();
            return instance;
        }

        public DBProduct()
        {
            productList = new LinkedList<Product>();
            nextProductID = 0;
        }

        public void addProduct(Product p)
        {
            productList.AddFirst(p);
        }

        public int getNextProductID()
        {
            int id = nextProductID;
            nextProductID++;
            return id;
        }
        public String removeProduct(Product p)
        {
            if (!productList.Contains(p))
                return "- product does not exist";
            productList.Remove(p);
            return "";
        }

        public Product getProductByID(int id)
        {
            foreach(Product p in productList) {
                if (p.getProductID() == id)
                    return p;
            }

            return null;
        }
        public LinkedList<Product> getAllProducts()
        {
            return productList;
        }

        }
}
