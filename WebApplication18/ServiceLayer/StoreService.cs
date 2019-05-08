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

        public int addProduct(string productName, string productCategory, int price, int rank, int quantityLeft, int store, int session)
        {
            checkProduct(productName, productCategory, price, rank, quantityLeft);
            return db.addProduct(productName, productCategory, price, rank, quantityLeft, store, session);
        }

        public LinkedList<Product> getProducts(int id)
        {
            return db.getProducts(id);
        }

        public void removeProduct(int product, int session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (session < 0)
                throw new ArgumentException("illegal session number");

            db.removeProduct(product, session);
        }

        public void setProductPrice(int product, int price, int session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (price <= 0)
                throw new IllegalAmountException("error - price must be a positive number");
            db.setProductPrice(product, price, session);
        }

        public void setProductName(int product, String name, int session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (name.Length == 0)
                throw new ArgumentException("error -product must have a name");
            db.setProductName(product, name, session);
        }

        public void addToProductQuantity(int product, int amount, int session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (amount <= 0)
                throw new IllegalAmountException("error - amount must be a positive number");
            db.addToProductQuantity(product, amount, session);
        }

        public void decFromProductQuantity(int product, int amount, int session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (amount <= 0)
                throw new IllegalAmountException("error - amount must be a positive number");
            db.decFromProductQuantity(product, amount, session);
        }

        public int addStore(string storeName, string storeDescription, int session)
        {
            if (storeName.Length == 0)
            {
                throw new ArgumentException("error - store must have a name");
            }
            return db.createStore(session, storeName, storeDescription);
        }

        public void closeStore(int store, int session)
        {
            if (store < 0)
            {
                throw new ArgumentException("illegal store number");
            }
            db.closeStore(store, session);
        }

        public void addManager(int store, string username,
            bool editProduct, bool editDiscount, bool editPolicy, int session)
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

        public void addOwner(int store, string username, int session)
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

        public void removeRole(int store, string username, int session)
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
                throw new ArgumentException("illeagal name");
            if (price < 0)
                throw new IllegalAmountException("error - price must be a positive number");
            if (rank < 0 | rank > 5)
                throw new IllegalAmountException("error - rank must be a number between 1 to 5");
            if (quantityLeft < 0)
                throw new IllegalAmountException("error - quantity must be a positive number");
        }


        ///////////////////////////////////////////////////

        public void addProductVisibleDiscount(int product, int session, double percentage, string duration)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.addProductVisibleDiscount(product, percentage, duration, session);


        }

        public void removeProductDiscount(int product, int session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.removeProductDiscount(product, session);

        }


        public void setProductDiscount(int product, int discount, int session)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");
            //if (product < 0)
            //    throw new ArgumentException("illegal discount number");
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.setProductDiscount(product, discount, session);
        }

        public void addStoreVisibleDiscount(int store, double percentage, string duration, int session)
        {
            if (store < 0)
                throw new ArgumentException("illegal store number");
            if (percentage <= 0 || percentage >= 1)
            {
                throw new IllegalAmountException("percentage of discount must be a number between 0 to 1");
            }
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.addStoreVisibleDiscount(store, percentage, duration, session);
        }

        public void addReliantDiscountSameProduct(int store, int session, double percentage, String duration, int numOfProducts, int product)
        {
            if (product < 0)
                throw new ArgumentException("illegal product number");

            if (store < 0)
                throw new ArgumentException("illegal store number");
            if (percentage <= 0 || percentage >= 1)
            {
                throw new IllegalAmountException("percentage of discount must be a number between 0 to 1");
            }
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.addReliantdiscountSameProduct(store, product, percentage, numOfProducts, duration, session);
        }
        public void addReliantDiscountTotalAmount(int store, int session, double percentage, String duration, int amount)
        {
            if (store < 0)
                throw new ArgumentException("illegal store number");
            if (percentage <= 0 || percentage >= 1)
            {
                throw new IllegalAmountException("percentage of discount must be a number between 0 to 1");
            }
            if (session < 0)
                throw new NullReferenceException("session is a null reference");
            db.addReliantdiscountTotalAmount(store, percentage, amount, duration, session);
        }

        public void removeStoreDiscount(int store, int sessionID)
        {
            if (store < 0)
                throw new ArgumentException("illegal store number");
            db.removeStoreDiscount(store, sessionID);
        }
        public void addComplexDiscount(List<DiscountComponent> list, string type)
        {
            throw new NotImplementedException();
        }

        public void addMinAmountPolicy(int storeID, int sessionID, int minAmount)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.addMinAmountPolicy(storeID, sessionID, minAmount);
        }
        public void setMinAmountPolicy(int storeID, int sessionID, int newMinAmount)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.setMinAmountPolicy(storeID, sessionID, newMinAmount);

        }
        public void removeMinAmountPolicy(int storeID, int sessionID)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.removeMaxAmountPolicy(storeID, sessionID);


        }

        public void addMaxAmountPolicy(int storeID, int sessionID, int maxAmount)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.addMaxAmountPolicy(storeID, sessionID, maxAmount);
        }
        public void setMaxAmountPolicy(int storeID, int sessionID, int newMaxAmount)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.setMaxAmountPolicy(storeID, sessionID, newMaxAmount);

        }
        public void removeMaxAmountPolicy(int storeID, int sessionID)
        {
            if (storeID < 0)
                throw new ArgumentException("illegal store number");

            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            db.removeMaxAmountPolicy(storeID, sessionID);


        }

        public void addCouponToStore(int sessionID, int storeID, string couponCode, double percentage, string duration)
        {
            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            if (storeID < 0)
                throw new NullReferenceException("store is a null reference");

            db.addCouponToStore(sessionID, storeID, couponCode, percentage, duration);
        }
        public void removeCouponFromStore(int sessionID, int storeID, string couponCode)
        {
            if (sessionID < 0)
                throw new NullReferenceException("session is a null reference");
            if (storeID < 0)
                throw new NullReferenceException("store is a null reference");

            db.removeCouponFromStore(sessionID, storeID, couponCode);
        }




    }
}