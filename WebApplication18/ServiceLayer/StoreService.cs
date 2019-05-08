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

        public string getProducts(int id)
        {
          return  db.getProductsString(id);
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

        public void SetProductInformation(int storeID, int productID, int price, int rank, int quantityLeft, string productName, int session)
        {
            int oldPrice = db.getProductPrice(productID);
            string oldName = db.getProductName(productID);
            int oldRank = db.getProductRank(productID);
            int oldQuantityLeft = db.getProductQuantityLeft(productID);

            try
            {
                setProductName(productID, productName, session);
                setProductPrice(productID, price, session);
                setProductRank(productID, rank, session);
                setquantityLeft(productID, quantityLeft, session);


            }
            catch(Exception e)
            {
                setProductName(productID, oldName, session);
                setProductPrice(productID, oldPrice, session);
                setProductRank(productID, oldRank, session);
                setquantityLeft(productID, oldQuantityLeft, session);
                throw e;
            }
        }
        
        public void setquantityLeft(int productID, int setquantityLeft, int session)
        {
            if (productID < 0)
                throw new ArgumentException("illegal product number");
            if (setquantityLeft < 0)
                throw new IllegalAmountException("error - amount must be a positive number");
            db.setquantityLeft(productID, setquantityLeft, session);
        }
        private void setProductRank(int productID, int rank, int session)
        {
            if (productID < 0)
                throw new ArgumentException("illegal product number");
            if (rank < 0)
                throw new IllegalAmountException("error - rank must be a positive number");
            db.setProductRank(productID, rank, session);
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






    }
}