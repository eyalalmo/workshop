using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.ServiceLayer
{
    class StoreService
    {
        private static StoreService instance =null;

        public StoreService() { }

        public static StoreService getStoreService()
        {
            if (instance == null)
            {
                instance = new StoreService();
            }
            return instance;
        }


        private int productID;
        private string productName;
        private string productCategory;
        private int price;
        private int rank;
        private int quantityLeft;
        private Discount discount;

        public string addProduct(int storeID, string productName, string productCategory, int price, int rank, int quantityLeft, Discount discount)
        {
            Store s = StoreDB.getStore(storeID);
            if (s == null)
                return "store not exist";
            string str = checkProduct(productName, productCategory, price, rank, quantityLeft);
            if (str != "")
                return str;
            int id = StoreDB.getNextProductId();

            Product product = new Product(id, productName, productCategory, price, rank, quantityLeft);

            string res = s.addProduct(product);
            return res;
        }

        public string removeProduct(int storeID,int productID ,string productName, string productCategory, int price, int rank, int quantityLeft, Discount discount)
        {
            Store s = StoreDB.getStore(storeID);
            if (s == null)
                return "store not exist";
            string str = checkProduct(productName, productCategory, price, rank, quantityLeft);
            if (str != "")
                return str;
           

            Product product = new Product(productID, productName, productCategory, price, rank, quantityLeft);
            string res = s.removeProduct(product);
            return res;
        }
        public string editProduct(int storeID, int productID ,string productName, string productCategory, int price, int rank, int quantityLeft, Discount discount)
        {
            Store s = StoreDB.getStore(storeID);
            if (s == null)
                return "store not exist";
            string str = checkProduct(productName, productCategory, price, rank, quantityLeft);
            if (str != "")
                return str;
           

            Product product = new Product(productID, productName, productCategory, price, rank, quantityLeft);
            string res = s.editProduct(product);
            return res;
        }
    
        private string checkProduct(string productName, string productCategory, int price, int rank, int quantityLeft)
        {
            if (productName == "")
                return "illeagal name";
            if (price < 0 )
                return "illeagal price";
            if (rank < 0 | rank > 5)
                return "illeagal rank";
            if (quantityLeft < 0)
                return "illeagal quantity";

            return "";
        }
    }
}
