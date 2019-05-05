using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;
using workshop192.Bridge;

namespace workshop192.ServiceLayer
{
    public class StoreService
    {
        private static StoreService instance = null;
        private static DomainBridge db = DomainBridge.getInstance();

        public StoreService() { }

        public static StoreService getInstance()
        {
            if (instance == null)
            {
                instance = new StoreService();
            }
            return instance;
        }

        public int addProduct(string productName, string productCategory, int price, int rank, int quantityLeft, int store, Session session)
        {
            checkProduct(productName, productCategory, price, rank, quantityLeft);
            return db.addProduct(productName, productCategory, price, rank, quantityLeft, store, session);
        }

        public void removeProduct(int product, Session session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (session == null)
                throw new NullReferenceException("session is a null reference");

            db.removeProduct(product, session);
        }

        public void setProductPrice(int product, int price, Session session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (price <= 0)
                throw new IllegalAmountException("error - price must be a positive number");
            db.setProductPrice(product, price, session);
        }

        public void setProductName(int product, String name, Session session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (name.Length == 0)
                throw new ArgumentException("error -product must have a name");
            db.setProductName(product, name, session);
        }

        public void addToProductQuantity(int product, int amount, Session session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (amount <= 0)
                throw new IllegalAmountException("error - amount must be a positive number");
            db.addToProductQuantity(product, amount, session);
        }

        public void decFromProductQuantity(int product, int amount, Session session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (amount <= 0)
                throw new IllegalAmountException("error - amount must be a positive number");
             db.decFromProductQuantity(product, amount, session);
        }

        public void setProductDiscount(int product, int discount, Session session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            //if (product < 0)
            //    throw new ArgumentException("illegal discount number");
            if (session == null)
                throw new NullReferenceException("session is a null reference");
            db.setProductDiscount(product, discount, session);
        }

        public int addStore(string storeName, string storeDescription, Session session)
        {
            if (storeName.Length == 0)
            {
                throw new ArgumentException("error - store must have a name");
            }
            return db.createStore(session, storeName, storeDescription);
        }

        public void closeStore(int store, Session session)
        {
            if (store < 0)
            {
                throw new ArgumentException("illegal store number");
            }
            db.closeStore(store, session);
        }

        public void addManager(int store, string username, 
            bool editProduct, bool editDiscount, bool editPolicy, Session session)
        {
            if (store < 0)
            {
                throw new ArgumentException("illegal store number");
            }

            if (username == null)
            {
                throw new ArgumentNullException("null username");
            }

            if (username.Length == 0)
            {
                throw new ArgumentException("illegal username");
            }

            db.addManager(store, username, editProduct, editDiscount, editPolicy, session);
        }

        public void addOwner(int store, string username, Session session)
        {
            if (store < 0)
            {
                throw new ArgumentException("illegal store number");
            }

            if (username == null)
            {
                throw new ArgumentNullException("null username");
            }

            if (username.Length == 0)
            {
                throw new ArgumentException("illegal username");
            }
            db.addOwner(store, username, session);
        }

        public void removeRole(int store, string username, Session session)
        {
            if (store < 0)
            {
                throw new ArgumentException("illegal store number");
            }

            if (username == null)
            {
                throw new ArgumentNullException("null username");
            }

            if (username.Length == 0)
            {
                throw new ArgumentException("illegal username");
            }
            db.removeRole(store, username, session);
        }
        
        private void checkProduct(string productName, string productCategory, int price, int rank, int quantityLeft)
        {
            if (productName == "")
                throw  new ArgumentException("illeagal name");
            if (price < 0)
                throw new IllegalAmountException("error - price must be a positive number");
            if (rank < 0 | rank > 5)
                throw new IllegalAmountException("error - rank must be a number between 1 to 5");
            if (quantityLeft < 0)
                throw new IllegalAmountException("error - quantity must be a positive number");
        }
    }
}