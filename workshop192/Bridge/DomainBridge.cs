using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.Bridge
{
    class DomainBridge
    {
        private static DomainBridge instance;

        public static DomainBridge getInstance()
        {
            if (instance == null)
                instance = new DomainBridge();
            return instance;
        }

        private DomainBridge()
        { }

        // use case 2.1 - the constructor defines guest as the default state
        public Session startSession()
        {
            return new Session();
        }
        
        //use case 2.3
        public void login(Session user, String username, String password)
        {
            user.login(username, password);
        }

        //use case 2.2
        public void register(Session user, String username, String password)
        {
            user.register(username, password);
        }
        //use case 6.2
        public void removeUser(Session admin, String username)
        {
            admin.removeUser(username);
        }

        public void logout(Session user)
        {

            user.logout();
        }

        public int createStore(Session session, String storeName, String description)
        {
            return session.createStore(storeName, description);
        }

        //use case 2.5

        public List<Product> searchProducts(String name, String keywords, String category)
        {
            return DBProduct.getInstance().searchProducts(name, keywords, category);
        }

        public List<Product> filterProducts(List<Product> list, int[] price_range, int minimumRank)
        {
            return DBProduct.getInstance().filterBy(list, price_range, minimumRank);

        }

        public void addToShoppingBasket(int product, int amount, Session session)
        {
            Product toAdd = DBProduct.getInstance().getProductByID(product);
            session.addToShoppingBasket(toAdd, amount);
        }

        public void purchaseBasket(Session session)
        {
            session.purchaseBasket();
        }

        public int createSession()
        {
            throw new NotImplementedException();
        }

        public int addProduct(string productName, string productCategory, int price, int rank, int quantityLeft, int storeID, Session session)
        {
            DBStore storeDB = DBStore.getInstance();
            Store store = storeDB.getStore(storeID);
            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            Product product = new Product(productName, productCategory, price, rank, quantityLeft, store);

            sr.addProduct(product);
            return product.getProductID();
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

        public void setProductDiscount(Product product, Discount discount, Session session)
        {
            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            sr.setProductDiscount(product, discount);
        }

        public int addStore(string storeName, string storeDescription, Session session)
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
                throw new UserException("no such username");

            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            if (sr.getStore() != store)
                throw new RoleException("this user can't appoint to this store");
            sr.addOwner(toAdd);
        }

        public void removeRole(Store store, string username, Session session)
        {
            SubscribedUser toRemove = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (toRemove == null)
                throw new UserException("no such username");
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

        //use case 2.7
        public Dictionary<int, ShoppingCart> getShoppingCarts(Session user)
        {
            return user.getShoppingBasket().getShoppingCarts();
        }

        public ShoppingCart getCart(Session user, int store)
        {
            return user.getShoppingBasket().getShoppingCartByID(store);
        }
        //use case 2.6
        public void addToCart(Session user, int product, int amount)
        {
            if (amount <= 0)
            {
                throw new IllegalAmountException("error : amount should be a positive number");
            }
            Product p = DBProduct.getInstance().getProductByID(product);
            user.getShoppingBasket().addToCart(p, amount);

        }
        //use case 2.7
        public void removeFromCart(Session user, int store, int product)
        {
            Product p = DBProduct.getInstance().getProductByID(product);
            user.getShoppingBasket().getShoppingCartByID(store).removeFromCart(p);
        }
        //use case 2.7
        public void changeQuantity(Session user, int product, int store, int newAmount)
        {
            if (newAmount <= 0)
            {
                throw new IllegalAmountException("ERROR: quantity should be a positive number");
            }
            Product p = DBProduct.getInstance().getProductByID(product);
            user.getShoppingBasket().getShoppingCartByID(store).changeQuantityOfProduct(p, newAmount);
        }

        public void checkoutCart(Session user, int store, String address, String creditCard)
        {
            user.getShoppingBasket().getShoppingCartByID(store).checkout(address, creditCard);
        }

        public void checkoutBasket(Session user, String address, String creditCard)
        {
            user.getShoppingBasket().purchaseBasket();
        }
    }
}
