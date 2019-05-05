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

        public void setup()
        {

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
            Store s = session.createStore(storeName, description);
            return s.getStoreID();
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

        public int addProduct(string productName, string productCategory, int price, int rank, int quantityLeft, int storeID, Session session)
        {
            DBStore storeDB = DBStore.getInstance();
            Store store = storeDB.getStore(storeID);
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
            user.getShoppingBasket().getShoppingCartByID(store).removeFromCart(DBProduct.getInstance().getProductByID(product));
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
