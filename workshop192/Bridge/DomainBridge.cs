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
            if (user == null)
            {
                throw new NullReferenceException("error - no such user ID");
            }
            user.login(username, password);
        }

        //use case 2.2
        public void register(Session user, String username, String password)
        {
            if (user == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            if (username.Equals("") || password.Equals(""))
            {
                throw new ArgumentException("Illegal username or password");
            }
            user.register(username, password);
        }
        //use case 6.2
        public void removeUser(Session admin, String username)
        {
            if (admin == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            admin.removeUser(username);
        }

        public void logout(Session user)
        {
            if (user == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            user.logout();
        }

        public int createStore(Session session, String storeName, String description)
        {
            if (session == null)
            {
                return null;
            }
            return session.createStore(storeName, description);
        }

        //use case 2.5

        public List<Product> searchProducts(String name, String keywords, String category)
        {
            return DBProduct.getInstance().searchProducts(name, keywords, category);
        }

        public List<Product> filterProducts(List<int> list, int[] price_range, int minimumRank)
        {
            return DBProduct.getInstance().filterBy(list, price_range, minimumRank);

        }

        public void addToShoppingBasket(Product product, int amount, Session session)
        {
            if (session == null)
            {
                throw new NullReferenceException("error - bad session");
            }
            session.addToShoppingBasket(product, amount);
        }

        public void purchaseBasket(Session session)
        {
            session.purchaseBasket();
        }

        public int addProduct(string productName, string productCategory, int price, int rank, int quantityLeft, int store, Session session)
        {
            DBStore storeDB = DBStore.getInstance();
            
            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            Product product = new Product(productName, productCategory, price, rank, quantityLeft, store);

            sr.addProduct(product);
            return product.getProductID();
        }

        public void removeProduct(int productid, Session session)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such username");

            SubscribedUser user = session.getSubscribedUser();
            
            StoreRole sr = DBStore.getInstance().getStoreRole(product.getStore(), user);

            sr.removeProduct(product);
        }

        public void setProductPrice(int productid, int price, Session session)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such username");

            SubscribedUser user = session.getSubscribedUser();
            
            StoreRole sr = DBStore.getInstance().getStoreRole(product.getStore(), user);

            sr.setProductPrice(product, price);
        }

        public void setProductName(int productid, String name, Session session)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such username");

            SubscribedUser user = session.getSubscribedUser();
            
            StoreRole sr = DBStore.getInstance().getStoreRole(product.getStore(), user);

            sr.setProductName(product, name);
        }

        public void addToProductQuantity(int productid, int amount, Session session)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such username");

            SubscribedUser user = session.getSubscribedUser();

            StoreRole sr = DBStore.getInstance().getStoreRole(product.getStore(), user);

            sr.addToProductQuantity(product, amount);
        }

        public void decFromProductQuantity(int productid, int amount, Session session)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such username");

            SubscribedUser user = session.getSubscribedUser();
            
            StoreRole sr = DBStore.getInstance().getStoreRole(product.getStore(), user);

            sr.decFromProductQuantity(product, amount);
        }

        public void setProductDiscount(int productid, int discount, Session session)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such username");

            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            //??????????????????????????????/
            //sr.setProductDiscount(product, discount);
        }

        public void closeStore(int storeid, Session session)
        {
            Store store = DBStore.getInstance().getStore(storeid);
            if (store == null)
                throw new DoesntExistException("no such store");
            session.closeStore(store);
        }

        public void addManager(int storeid, string username,
            bool editProduct, bool editDiscount, bool editPolicy, Session session)
        {
            SubscribedUser toAdd = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (toAdd == null)
            {
                throw new DoesntExistException("no such username");
            }
            Store store = DBStore.getInstance().getStore(storeid);
            if (store == null)
            {
                throw new DoesntExistException("no such store");
            }
            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            if (sr.getStore() != store)
                throw new RoleException("this user can't appoint to this store");
            Permissions permissions = new Permissions(editProduct, editDiscount, editPolicy);
            sr.addManager(toAdd, permissions);
        }

        public void addOwner(int storeid, string username, Session session)
        {
            SubscribedUser toAdd = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (toAdd == null)
                throw new DoesntExistException("no such username");
            Store store = DBStore.getInstance().getStore(storeid);
            if (store == null)
            {
                throw new DoesntExistException("no such store");
            }
            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            if (sr.getStore() != store)
                throw new RoleException("this user can't appoint to this store");
            sr.addOwner(toAdd);
        }

        public void removeRole(int storeid, string username, Session session)
        {
            SubscribedUser toRemove = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (toRemove == null)
                throw new DoesntExistException("no such username");
            Store store = DBStore.getInstance().getStore(storeid);
            if (store == null)
            {
                throw new DoesntExistException("no such store");
            }
            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            if (sr.getStore() != store)
                throw new RoleException("this user can't remove roles from this store");
            sr.remove(toRemove);
        }

        //use case 2.7
        public Dictionary<int, ShoppingCart> getShoppingCarts(Session user)
        {
            return user.getShoppingBasket().getShoppingCarts();
        }

        public ShoppingCart getCart(Session user, Store store)
        {
            return user.getShoppingBasket().getShoppingCartByID(store.getStoreID());
        }
        //use case 2.6
        public void addToCart(Session user, Store store, Product product, int amount)
        {
            user.getShoppingBasket().addToCart(product, amount);
        }
        //use case 2.7
        public void removeFromCart(Session user, Store store, Product product)
        {
            user.getShoppingBasket().getShoppingCartByID(store.getStoreID()).removeFromCart(product);
        }
        //use case 2.7
        public void changeQuantity(Session user, Product product, Store store, int newAmount)
        {
            user.getShoppingBasket().getShoppingCartByID(store.getStoreID()).changeQuantityOfProduct(product, newAmount);
        }

        public string checkoutCart(Session user, Store store, String address, String creditCard)
        {
            return user.getShoppingBasket().getShoppingCartByID(store.getStoreID()).checkout(address, creditCard);
        }

        public String checkoutBasket(Session user, String address, String creditCard)
        {
            return user.getShoppingBasket().checkout(address, creditCard);
        }
    }
}
