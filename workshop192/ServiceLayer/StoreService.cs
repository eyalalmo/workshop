using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.ServiceLayer
{
    public class StoreService
    {
        private static StoreService instance = null;

        public StoreService() { }

        public static StoreService getInstance()
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

        public string removeProduct(Product product, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            return sr.removeProduct(product);
        }

        public string setProductPrice(Product product, int price, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            if (price <= 0)
                return "illegal price";
            return sr.setProductPrice(product, price);
        }

        public string setProductName(Product product, String name, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            if (name.Length == 0)
                return "illegal name";
            return sr.setProductName(product, name);
        }

        public string addToProductQuantity(Product product, int amount, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            if (amount <= 0)
                return "illegal amount";
            return sr.addToProductQuantity(product, amount);
        }

        public string decFromProductQuantity(Product product, int amount, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            if (amount <= 0)
                return "illegal amount";
            return sr.decFromProductQuantity(product, amount);
        }

        public string setProductDiscount(Product product, Discount discount, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            return sr.setProductDiscount(product, discount);
        }

        private string checkProduct(string productName, string productCategory, int price, int rank, int quantityLeft)
        {
            if (productName == "")
                return "illeagal name";
            if (price < 0)
                return "illeagal price";
            if (rank < 0 | rank > 5)
                return "illeagal rank";
            if (quantityLeft < 0)
                return "illeagal quantity";

            return "";
        }
    }
}