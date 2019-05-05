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

        public Product addProduct(string productName, string productCategory, int price, int rank, int quantityLeft, Store store, Session session)
        {
            DBStore storeDB = DBStore.getInstance();

            checkProduct(productName, productCategory, price, rank, quantityLeft);

            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            Product product = new Product(productName, productCategory, price, rank, quantityLeft, store);

            sr.addProduct(product);
            return product;
        }

        public void removeProduct(Product product, Session session)
        {
            if (product == null)
                throw new NullReferenceException("product is a null reference");
            if (session == null)
                throw new NullReferenceException("session is a null reference");

            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            sr.removeProduct(product);
        }

        public void setProductPrice(Product product, int price, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            if (price <= 0)
                throw new IllegalAmountException("error - price must be a positive number");
            sr.setProductPrice(product, price);
        }

        public void setProductName(Product product, String name, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            if (name.Length == 0)
                throw new ArgumentException("error -product must have a name");
            sr.setProductName(product, name);
        }

        public void addToProductQuantity(Product product, int amount, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            if (amount <= 0)
                throw new IllegalAmountException("error - amount must be a positive number");
            sr.addToProductQuantity(product, amount);
        }

        public void decFromProductQuantity(Product product, int amount, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            if (amount <= 0)
                throw new IllegalAmountException("error - amount must be a positive number");
            sr.decFromProductQuantity(product, amount);
        }

        public void setProductDiscount(Product product, DiscountComponent discount, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            sr.setProductDiscount(product, discount);
        }

        public Store addStore(string storeName, string storeDescription, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            if (storeName.Length == 0)
            {
                throw new ArgumentException("error - store must have a name");
            }
            return session.createStore(storeName, storeDescription);
        }

        public void closeStore(Store store, Session session)
        {
            session.closeStore(store);
        }

        public void addManager(Store store, string username,
            bool editProduct, bool editDiscount, bool editPolicy, Session session)
        {
            SubscribedUser toAdd = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (toAdd == null)
            {
                throw new NullReferenceException("error - no such user ID");
            }
            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            if (sr.getStore() != store)
                throw new RoleException("this user can't appoint to this store");
            Permissions permissions = new Permissions(editProduct, editDiscount, editPolicy);
            sr.addManager(toAdd, permissions);
        }

        public void addOwner(Store store, string username, Session session)
        {
            SubscribedUser toAdd = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (toAdd == null)
                throw new NullReferenceException("error - no such user ID");

            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            if (sr.getStore() != store)
                throw new RoleException("this user can't appoint to this store");
            sr.addOwner(toAdd);
        }

        public void removeRole(Store store, string username, Session session)
        {
            SubscribedUser toRemove = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (toRemove == null)
                throw new NullReferenceException("error -no such user ID)");
            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            if (sr.getStore() != store)
                throw new RoleException("this user can't remove roles from this store");
            sr.remove(toRemove);
        }

        private void checkProduct(string productName, string productCategory, int price, int rank, int quantityLeft)
        {
            if (productName == "")
                throw new ArgumentException("illeagal name");
            if (price < 0)
                throw new IllegalAmountException("error - price must be a positive number");
            if (rank < 0 | rank > 5)
                throw new IllegalAmountException("error - rank must be a number between 1 to 5");
            if (quantityLeft < 0)
                throw new IllegalAmountException("error - quantity must be a positive number");

        }
    }
}