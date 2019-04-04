using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    public class DBProduct
    {
        public void initDB()
        {
            productList = new LinkedList<Product>();
            nextProductID = 0;
        }
        private static DBProduct  instance;

        private  LinkedList<Product> productList;
        public static int nextProductID;

        public static DBProduct getInstance()
        {
            if (instance == null)
                instance = new DBProduct();
            return instance;
        }
        public void init()
        {
            productList = new LinkedList<Product>();
            nextProductID = 0;
        }
        private DBProduct()
        {
            productList = new LinkedList<Product>();
            nextProductID = 0;
        }

        public int addProduct(Product p)
        {
            productList.AddFirst(p);
            return p.getProductID();
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

        public  List<Product> searchProducts(string name, string keywords, string category, int[] price_range, int minimumRank)
        {
            List<Product> res = new List<Product>();
            foreach(Product p in productList)
            {
                if(name!= null && p.getProductName()!=null && p.getProductName() == name)
                {
                    res.Add(p);
                }
                string pName = p.getProductName();
                if (keywords != null && pName != null && pName.Contains(keywords))
                {
                    if (!res.Contains(p))
                        res.Add(p);
                }
                string categ = p.getProductCategory();
                if (keywords != null && categ != null && categ.Contains(keywords))
                {
                    if (!res.Contains(p))
                        res.Add(p);
                }

                if(category!=null && categ!=null & categ.Contains(category))
                {
                    if (!res.Contains(p))
                        res.Add(p);
                }


            }


            res = filterBy(res,price_range, minimumRank);
            return res;


        }

        private List<Product> filterBy(List<Product> list, int[] price_range, int minimumRank)
        {
            List<Product> toRemove = new List<Product>();
            foreach(Product p in list)
            {
                int price = p.getActualPrice();
                if (price_range!=null && (price < price_range[0] || price > price_range[1]))
                    toRemove.Add(p);
                else if (minimumRank != 0 && p.getRank() < minimumRank)
                    if (list.Contains(p))
                        toRemove.Add(p);
            }

            return list.Except(toRemove).ToList();

        }
    }
}
