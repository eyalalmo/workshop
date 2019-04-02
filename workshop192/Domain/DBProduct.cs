using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class DBProduct
    {
<<<<<<< HEAD
        public static DBProduct  instance;
        LinkedList<Product> productList;
=======
        private static DBProduct  instance;
        private LinkedList<Product> productList;
>>>>>>> origin/Stores_and_Products
        public static int nextProductID;

        public static DBProduct getInstance()
        {
            if (instance == null)
                instance = new DBProduct();
            return instance;
        }

        private DBProduct()
        {
            productList = new LinkedList<Product>();
            nextProductID = 0;
        }

        public void addProduct(Product p)
        {
            productList.AddFirst(p);
        }

        public static int getNextProductID()
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
