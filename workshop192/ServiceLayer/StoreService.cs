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

        public string addProduct(string productName, string productCategory, int price, int rank, int quantityLeft, Store store, Session session)
        {
            DBStore storeDB = DBStore.getInstance();

            string res = checkProduct(productName, productCategory, price, rank, quantityLeft);
            if (res != "")
                return res;
            
            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            Product product = new Product(productName, productCategory, price, rank, quantityLeft, store);

            return sr.addProduct(product);
        }

        public string removeProduct(Product product, Store store, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(store, user);

            return sr.removeProduct(product);
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
