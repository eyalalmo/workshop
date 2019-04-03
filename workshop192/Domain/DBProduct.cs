using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class DBProduct
    {

        private static DBProduct  instance;
<<<<<<< HEAD
        private LinkedList<Product> productList;

=======
        private  LinkedList<Product> productList;
>>>>>>> origin/ProductsAndPurchases
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

        public  LinkedList<Product> searchProducts(string name, string keywords, string category, int[] price_range, int minimumRank)
        {
            LinkedList<Product> res = new LinkedList<Product>();
            foreach(Product p in productList)
            {
                if(name!= null && p.getProductName()!=null && p.getProductName() == name)
                {
                    res.AddFirst(p);
                }
                string pName = p.getProductName();
                if (keywords != null && pName != null && pName.Contains(keywords))
                {
                    if (!res.Contains(p))
                        res.Contains(p);
                }
                string categ = p.getProductCategory();
                if (keywords != null && categ != null && categ.Contains(keywords))
                {
                    if (!res.Contains(p))
                        res.Contains(p);
                }

                if(category!=null && categ!=null & categ.Contains(category))
                {
                    if (!res.Contains(p))
                        res.Contains(p);
                }


            }


            res = filterBy(res,price_range, minimumRank);
            return res;


        }

        private LinkedList<Product> filterBy(LinkedList<Product> list, int[] price_range, int minimumRank)
        {
            foreach(Product p in list)
            {
                int price = p.getActualPrice();
                if (price < price_range[0] || price > price_range[1])
                    list.Remove(p);
                else if (minimumRank != 0 && p.getRank() < minimumRank)
                    if (list.Contains(p))
                        list.Remove(p);
            }

            return list;

        }
    }
}
