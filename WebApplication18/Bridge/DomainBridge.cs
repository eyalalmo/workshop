using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication18.Domain;
using WebApplication18.Logs;
using workshop192.Domain;

namespace workshop192.Bridge
{
    public class DomainBridge
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
        public int startSession()
        {
            return DBSession.getInstance().generate();
        }

        public void setup()
        {
            MarketSystem.init();
        }

        //use case 2.3
        public void login(int sessionid, String username, String password)
        {
            Session s = DBSession.getInstance().getSession(sessionid);
            
            s.login(username, password);
            SystemLogger.getEventLog().Info("User " + username + " has successfuly logged in.");
        }

        //use case 2.2
        public void register(int sessionid, String username, String password)
        {
            Session user = DBSession.getInstance().getSession(sessionid);
            user.register(username, password);
            SystemLogger.getEventLog().Info("User " + username + " has successfuly registered");
        }

        public bool isOwner(int storeId, int session)
        {
            Session user = DBSession.getInstance().getSession(session);
            Store s = DBStore.getInstance().getStore(storeId);
            StoreRole role=  s.getStoreRole(user.getSubscribedUser());
            if(role is StoreOwner)
            {
                return true;
            }
            return false;
        }
        public bool isManager(object v, int session)
        {
            Session user = DBSession.getInstance().getSession(session);
            Store s = DBStore.getInstance().getStore((int)v);
            StoreRole role = s.getStoreRole(user.getSubscribedUser());
            if (role is StoreManager)
            {
                return true;
            }
            return false;

        }

            public LinkedList<Product> getProducts(int id)
        {
            return DBStore.getInstance().getStore(id).getProductList();
        }
        public string getProductsString(int id)
        {
            return DBStore.getInstance().getStore(id).getProductsString();
        }
        //use case 6.2
        public void removeUser(int sessionid, String username)
        {
            Session admin = DBSession.getInstance().getSession(sessionid);
            admin.removeUser(username);
            SystemLogger.getEventLog().Info("User " + username + " has been successfuly removed");
        }

        internal int getSessionByUserName(string username)
        {
            return DBSession.getInstance().getSessionOfUserName(username);
        }

        internal void setWaitingMessages(LinkedList<Tuple<string, string>> remains)
        {
            DBNotifications.getInstance().setWaitingNotifications(remains);
        }

        public void logout(int sessionid)
        {
            Session user = DBSession.getInstance().getSession(sessionid);
            user.logout();
            SystemLogger.getEventLog().Info("User " + user.getSubscribedUser().getUsername() + " has logged out");
        }

        public string getAllProducts()
        {
            return DBProduct.getInstance().AllproductsToJson();
        }
        public int createStore(int sessionId, String storeName, String description)
        {
            Session session = DBSession.getInstance().getSession(sessionId);
            Store s = session.createStore(storeName, description);
            SystemLogger.getEventLog().Info("User " + session.getSubscribedUser().getUsername() + " has successfuly created a store");
            return s.getStoreID();
        }

        public string getProductName(int productID)
        {
        
            Product p = DBProduct.getInstance().getProductByID(productID);
            return p.getProductName();
        }

        public int getProductRank(int productID)
        {
            Product p = DBProduct.getInstance().getProductByID(productID);
            return p.getRank();
        }

        public int getProductQuantityLeft(int productID)
        {
            Product p = DBProduct.getInstance().getProductByID(productID);
            return p.getQuantityLeft();
        }

        public bool isAllowedToEditProduct(int storeId, int session)
        {
            Session user = DBSession.getInstance().getSession(session);
            Store s = DBStore.getInstance().getStore(storeId);
            StoreRole role = s.getStoreRole(user.getSubscribedUser());
            if (role is StoreOwner)
            {
                return true;
            }
            if(role is StoreManager && ((StoreManager)role).getPermissions().editProduct() == true)
            {
                return true;
            }
            return false;
        }

        internal string generate()
        {
            return DBCookies.getInstance().generate();
        }

        internal int getUserByHash(string hash)
        {
            return DBCookies.getInstance().getUserByHash(hash);
        }

        internal string addSession(string hash, int session)
        {
            return DBCookies.getInstance().addSession(hash, session);

        }

        public bool isAllowedToEditPolicy(int storeId, int session)
        {
            Session user = DBSession.getInstance().getSession(session);
            Store s = DBStore.getInstance().getStore(storeId);
            StoreRole role = s.getStoreRole(user.getSubscribedUser());
            if (role is StoreOwner)
            {
                return true;
            }
            if (role is StoreManager && ((StoreManager)role).getPermissions().editPolicy() == true)
            {
                return true;
            }
            return false;
        }
        public bool isAllowedToEditDiscount(int storeId, int session)
        {
            Session user = DBSession.getInstance().getSession(session);
            Store s = DBStore.getInstance().getStore(storeId);
            StoreRole role = s.getStoreRole(user.getSubscribedUser());
            if (role is StoreOwner)
            {
                return true;
            }
            if (role is StoreManager && ((StoreManager)role).getPermissions().editDiscount() == true)
            {
                return true;
            }
            return false;
        }



        //use case 2.5

        public string searchProducts(String name, String keywords, String category)
        {
            return JsonConvert.SerializeObject(DBProduct.getInstance().searchProducts(name, keywords, category));
        }

        public List<Product> filterProducts(List<Product> list, int[] price_range, int minimumRank)
        {
            return DBProduct.getInstance().filterBy(list, price_range, minimumRank);

        }

        public void addToShoppingBasket(int product, int amount, int sessionid)
        {
            Product toAdd = DBProduct.getInstance().getProductByID(product);
            Session session = DBSession.getInstance().getSession(sessionid);
            session.addToShoppingBasket(toAdd, amount);
        }

        internal void setquantityLeft(int productID, int setquantityLeft, int session)
        {
            Product p = DBProduct.getInstance().getProductByID(productID);
            p.setQuantityLeft(setquantityLeft);
        }

        public void purchaseBasket(int sessionid, string address, string creditCard)
        {
            Session session = DBSession.getInstance().getSession(sessionid);
            session.purchaseBasket(address, creditCard);
            SystemLogger.getEventLog().Info("A purchase has been made");
        }

        public void setProductRank(int productID, int rank, int session)
        {
            Product p = DBProduct.getInstance().getProductByID(productID);
             p.setRank(rank);
        }

        public void addcouponToCart(int sessionID, int storeID, string couponCode)
        {
            Session session = DBSession.getInstance().getSession(sessionID);
            ShoppingBasket shoppingBasket=  session.getShoppingBasket();
            shoppingBasket.addCoupon(couponCode, storeID);

        }


        public void removeCouponFromCart(int sessionID, int storeID)
        {
            Session session = DBSession.getInstance().getSession(sessionID);
            ShoppingBasket shoppingBasket = session.getShoppingBasket();
            shoppingBasket.removeCoupon(storeID);
        }

        public string getShoppingBasket(int sessionid)
        {
            Session session = DBSession.getInstance().getSession(sessionid);
            ShoppingBasket basket = session.getShoppingBasket();
            string response = "";
            foreach (KeyValuePair<int, ShoppingCart> cart in basket.getShoppingCarts())
            {
                foreach (KeyValuePair<Product, int> p in cart.Value.getProductsInCarts())
                {
                    response += p.Key.getProductName() + "," + p.Key.getActualPrice() + "," + p.Key.getProductID() + "," + p.Value + ";";
                }
            }

            return response;
        }
        public double getShoppingBasketTotalPrice(int sessionid)
        {
            Session session = DBSession.getInstance().getSession(sessionid);
            return session.getShoppingBasket().getTotalPrice();
        }

        public int addProduct(string productName, string productCategory, int price, int rank, int quantityLeft, int storeID, int sessionid)
        {
            DBStore storeDB = DBStore.getInstance();
            Store store = storeDB.getStore(storeID);
            Session session = DBSession.getInstance().getSession(sessionid);
            StoreRole sr = store.getStoreRole(session.getSubscribedUser());
            Product product = new Product(productName, productCategory, price, rank, quantityLeft, store);

            if (sr == null)
                throw new RoleException("Error: You have no permission to add a product");

            sr.addProduct(product);
            return product.getProductID();
        }

        internal string getState(string hash)
        {
            int sessionid = DBCookies.getInstance().getUserByHash(hash);
            return DBSession.getInstance().getSession(sessionid).getState().getStateName();
        }

        public void removeProduct(int productid, int sessionid)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such product");

            Session session = DBSession.getInstance().getSession(sessionid);

            SubscribedUser user = session.getSubscribedUser();

            StoreRole sr = DBStore.getInstance().getStoreRole(product.getStore(), user);

            if (sr == null)
                throw new RoleException("Error: You have no permission to remove a product");

            sr.removeProduct(product);
        }

        internal LinkedList<Tuple<string, string>> getWaitingNotifications()
        {
            return DBNotifications.getInstance().getWaitingNotifications();
        }

        internal string getUserNameBySession(int session)
        {
            Session s = DBSession.getInstance().getSession(session);
            return s.getSubscribedUser().getUsername();
        }

        internal double getProductPrice(int productid)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such product");

            return product.getPrice();
        }

        public void setProductPrice(int productid, int price, int sessionid)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such product");

            Session session = DBSession.getInstance().getSession(sessionid);

            SubscribedUser user = session.getSubscribedUser();

            StoreRole sr = DBStore.getInstance().getStoreRole(product.getStore(), user);

            if (sr == null)
                throw new RoleException("Error: You have no permission to edit a product");

            sr.setProductPrice(product, price);
        }

        public void setProductName(int productid, String name, int sessionid)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such product");

            Session session = DBSession.getInstance().getSession(sessionid);

            SubscribedUser user = session.getSubscribedUser();

            StoreRole sr = DBStore.getInstance().getStoreRole(product.getStore(), user);

            if (sr == null)
                throw new RoleException("Error: You have no permission to edit a product");

            sr.setProductName(product, name);
        }

        public string getStore(int id)
        {
            return JsonConvert.SerializeObject(DBStore.getInstance().getStore(id));
        }

        public string getRoles(int id)
        {
            return JsonConvert.SerializeObject(DBStore.getInstance().getRoles(id));
        }

        public void addToProductQuantity(int productid, int amount, int sessionid)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such product");

            Session session = DBSession.getInstance().getSession(sessionid);

            SubscribedUser user = session.getSubscribedUser();

            StoreRole sr = DBStore.getInstance().getStoreRole(product.getStore(), user);

            if (sr == null)
                throw new RoleException("Error: You have no permission to edit a product");

            sr.addToProductQuantity(product, amount);
        }

        public void decFromProductQuantity(int productid, int amount, int sessionid)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such product");

            Session session = DBSession.getInstance().getSession(sessionid);

            SubscribedUser user = session.getSubscribedUser();

            StoreRole sr = DBStore.getInstance().getStoreRole(product.getStore(), user);

            if (sr == null)
                throw new RoleException("Error: You have no permission to edit a product");

            sr.decFromProductQuantity(product, amount);
        }

        

        public void closeStore(int storeid, int sessionid)
        {
            Store store = DBStore.getInstance().getStore(storeid);
            if (store == null)
                throw new DoesntExistException("no such store");

            Session session = DBSession.getInstance().getSession(sessionid);
            SystemLogger.getEventLog().Info("Store "+store.getStoreName()+" has been closed by Admin");
            session.closeStore(store);
        }

        public void addManager(int storeid, string username,
            bool editProduct, bool editDiscount, bool editPolicy, int sessionid)
        {
            SubscribedUser toAdd = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (toAdd == null)
            {
                throw new DoesntExistException("Error: No such username");
            }
            Store store = DBStore.getInstance().getStore(storeid);
            if (store == null)
            {
                throw new DoesntExistException("no such store");
            }

            Session session = DBSession.getInstance().getSession(sessionid);

            StoreRole sr = store.getStoreRole(session.getSubscribedUser());

            if (sr == null)
                throw new RoleException("Error: You have no permission to add a manager");

            if (sr.getStore() != store)
                throw new RoleException("this user can't appoint to this store");
            Permissions permissions = new Permissions(editProduct, editDiscount, editPolicy);
            sr.addManager(toAdd, permissions);
        }
        public void addOwner(int storeid, string username, int sessionid)
        {
            SubscribedUser toAdd = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (toAdd == null)
                throw new DoesntExistException("Error: No such username");
            Store store = DBStore.getInstance().getStore(storeid);
            if (store == null)
            {
                throw new DoesntExistException("no such store");
            }

            Session session = DBSession.getInstance().getSession(sessionid);

            StoreRole sr = store.getStoreRole(session.getSubscribedUser());

            if (sr == null)
                throw new RoleException("Error: You don't have permissions to appoint an owner");

            if (sr.getStore() != store)
                throw new RoleException("this user can't appoint to this store");
            sr.addOwner(toAdd);
        }

        public void removeRole(int storeid, string username, int sessionid)
        {
            SubscribedUser toRemove = DBSubscribedUser.getInstance().getSubscribedUser(username);
            if (toRemove == null)
                throw new DoesntExistException("no such username");
            Store store = DBStore.getInstance().getStore(storeid);
            if (store == null)
            {
                throw new DoesntExistException("no such store");
            }

            Session session = DBSession.getInstance().getSession(sessionid);

            StoreRole sr = store.getStoreRole(session.getSubscribedUser());

            if (sr == null)
                throw new RoleException("no role for this user in this store");

            if (sr.getStore() != store)
                throw new RoleException("this user can't remove roles from this store");
            sr.remove(toRemove);
        }

        internal void addCouponToStore(int sessionID, int storeID, string couponCode, double percentage, string duration)
        {
            Session user = DBSession.getInstance().getSession(sessionID);
            if (user == null)
                throw new DoesntExistException("Error: User is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");
            sr.addCouponToStore( couponCode, percentage, duration);
        }

        internal void removeCouponFromStore(int sessionID, int storeID, string couponCode)
        {
            Session user = DBSession.getInstance().getSession(sessionID);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");
            sr.removeCouponFromStore(couponCode);
        }

        //use case 2.7

        public string BasketToJson(ShoppingBasket basket)
        {
            string s = JsonConvert.SerializeObject(basket);
            return s;
        }
        public Dictionary<int, ShoppingCart> getShoppingCarts(int sessionid)
        {
            Session user = DBSession.getInstance().getSession(sessionid);

            return user.getShoppingBasket().getShoppingCarts();
        }

        public ShoppingCart getCart(int sessionid, int store)
        {
            Session user = DBSession.getInstance().getSession(sessionid);

            return user.getShoppingBasket().getShoppingCartByID(store);
        }
        //use case 2.6
        public void addToCart(int sessionid, int product, int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Error : Quantity should be a positive number");
            }
            Product p = DBProduct.getInstance().getProductByID(product);

            Session s = DBSession.getInstance().getSession(sessionid);
            
            s.getShoppingBasket().addToCart(p, amount);
        }

        //use case 2.7
        public void removeFromCart(int sessionid, int product)
        {
            Session user = DBSession.getInstance().getSession(sessionid);

            user.removeFromCart(product);
        }
        //use case 2.7
        public void changeQuantity(int sessionid, int product, int store, int newAmount)
        {
            if (newAmount <= 0)
            {
                throw new ArgumentException("Error: Quantity should be a positive number");
            }
            Product p = DBProduct.getInstance().getProductByID(product);

            Session user = DBSession.getInstance().getSession(sessionid);

            user.getShoppingBasket().getShoppingCartByID(store).changeQuantityOfProduct(p, newAmount);
        }

       /* public void checkoutCart(int sessionid, int store, String address, String creditCard)
        {
            Session user = DBSession.getInstance().getSession(sessionid);

            user.getShoppingBasket().getShoppingCartByID(store).checkout(address, creditCard);
        }*/

        public void checkoutBasket(int sessionid, String address, String creditCard)
        {
            Session user = DBSession.getInstance().getSession(sessionid);

            user.getShoppingBasket().purchaseBasket(address, creditCard);
        }
        public string getAllStores(int session1)
        {
            Session session = DBSession.getInstance().getSession(session1);
            List<StoreRole> lst = session.getSubscribedUser().getStoreRoles();
            LinkedList<Store> stores = new LinkedList<Store>();
            foreach (StoreRole element in lst)
            {
                stores.AddLast(element.getStore());
            }
            return JsonConvert.SerializeObject(stores); 
        }

        ////////////////////////////////////////////

        internal void removeProductDiscount(int product, int session)
        {
            Session user = DBSession.getInstance().getSession(session);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Product p = DBProduct.getInstance().getProductByID(product);
            Store store =p.getStore();
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");

            sr.removeProductDiscount(p);
        }

        internal void addStoreVisibleDiscount(int storeID, double percentage, string duration, int session)
        {
            Session user = DBSession.getInstance().getSession(session);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");

            sr.addStoreVisibleDiscount(percentage, duration);


        }

        internal void addProductVisibleDiscount(int product, double percentage, string duration, int session)
        {
            Session user = DBSession.getInstance().getSession(session);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Product p = DBProduct.getInstance().getProductByID(product);
            Store store = p.getStore();
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");

            sr.addProductVisibleDiscount(p, percentage, duration);
        }
        internal void addReliantdiscountSameProduct(int storeID, int product, double percentage, int numOfProducts, string duration, int session)
        {
            Session user = DBSession.getInstance().getSession(session);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Product p = DBProduct.getInstance().getProductByID(product);
            Store store = p.getStore();
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");
            sr.addReliantDiscountSameProduct(percentage, duration, numOfProducts, p);

        }

        internal void addReliantdiscountTotalAmount(int storeID, double percentage, int amount, string duration, int session)
        {

            Session user = DBSession.getInstance().getSession(session);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");

            sr.addReliantDiscountTotalAmount(percentage, duration, amount);
        }

        internal void removeStoreDiscount(int storeID, int session)
        {
            Session user = DBSession.getInstance().getSession(session);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");

            sr.removeStoreDiscount(store);

        }

        public void setProductDiscount(int productid, int discount, int sessionid)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("Product does not exist");

            Session session = DBSession.getInstance().getSession(sessionid);

            SubscribedUser user = session.getSubscribedUser();

            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(product.getStore(), user);

            if (sr == null)
                throw new RoleException("no role for this user in this store");

            //sr.setProductDiscount(product, discount);



        }

        public void removeMaxAmountPolicy(int storeID, int sessionID)
        {
            Session user = DBSession.getInstance().getSession(sessionID);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");
            sr.removeMaxAmountPolicy();
        }
        public void removeMinAmountPolicy(int storeID, int sessionID)
        {
            Session user = DBSession.getInstance().getSession(sessionID);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");
            sr.removeMinAmountPolicy();
        }


        public void setMinAmountPolicy(int storeID, int sessionID, int newMinAmount)
        {
            Session user = DBSession.getInstance().getSession(sessionID);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");
            sr.setMinAmountPolicy(newMinAmount);
        }

       
        internal void setMaxAmountPolicy(int storeID, int sessionID, int newMinAmount)
        {
            Session user = DBSession.getInstance().getSession(sessionID);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            SubscribedUser subscribedUser = user.getSubscribedUser();
            if (subscribedUser == null)
                throw new DoesntExistException("not a subscribed user");
            StoreRole sr = subscribedUser.getStoreRole(store);
            if (sr == null)
                throw new RoleException("no role for this user in this store");
            sr.setMaxAmountPolicy(newMinAmount);
        }



        /////////////////////////////////////////////////////////////
        
   
   
 
    public bool hasMinPurchasePolicy(int storeID, int sessionID)
    {
        Session user = DBSession.getInstance().getSession(sessionID);
        if (user == null)
            throw new DoesntExistException("user is not logged in");
        Store store = DBStore.getInstance().getStore(storeID);
        SubscribedUser subscribedUser = user.getSubscribedUser();
        if (subscribedUser == null)
            throw new DoesntExistException("not a subscribed user");
        StoreRole sr = subscribedUser.getStoreRole(store);
        if (sr == null)
            throw new RoleException("no role for this user in this store");
        return store.hasMinPurchasePolicy();
    }
    public bool hasMaxPurchasePolicy(int storeID, int sessionID)
    {
        Session user = DBSession.getInstance().getSession(sessionID);
        if (user == null)
            throw new DoesntExistException("user is not logged in");
        Store store = DBStore.getInstance().getStore(storeID);
        SubscribedUser subscribedUser = user.getSubscribedUser();
        if (subscribedUser == null)
            throw new DoesntExistException("not a subscribed user");
        StoreRole sr = subscribedUser.getStoreRole(store);
        if (sr == null)
            throw new RoleException("no role for this user in this store");
        return store.hasMinPurchasePolicy();
    }

    public MinAmountPurchase getMinAmountPolicy(int storeID, int sessionID)
    {
        Session user = DBSession.getInstance().getSession(sessionID);
        if (user == null)
            throw new DoesntExistException("user is not logged in");
        Store store = DBStore.getInstance().getStore(storeID);
        SubscribedUser subscribedUser = user.getSubscribedUser();
        if (subscribedUser == null)
            throw new DoesntExistException("not a subscribed user");
        StoreRole sr = subscribedUser.getStoreRole(store);
        if (sr == null)
            throw new RoleException("no role for this user in this store");
        return store.getMinAmountPolicy();
    }

        public string getMinAmountPolicyString(int storeID, int sessionID)
        {
            return getMinAmountPolicy(storeID, sessionID).getAmount()+"";
        }
        public string getMaxAmountPolicyString(int storeID, int sessionID)
        {
            return getMaxAmountPolicy(storeID, sessionID).getAmount()+"";
        }


        public MaxAmountPurchase getMaxAmountPolicy(int storeID, int sessionID)
    {
        Session user = DBSession.getInstance().getSession(sessionID);
        if (user == null)
            throw new DoesntExistException("user is not logged in");
        Store store = DBStore.getInstance().getStore(storeID);
        SubscribedUser subscribedUser = user.getSubscribedUser();
        if (subscribedUser == null)
            throw new DoesntExistException("not a subscribed user");
        StoreRole sr = subscribedUser.getStoreRole(store);
        if (sr == null)
            throw new RoleException("no role for this user in this store");
        return store.getMaxAmountPolicy();
    }

        public double getAmountByCart(int storeID, int sessionID)
        {
            ShoppingCart sc1 = getCart(sessionID, storeID);
            double amount = sc1.getTotalPrice();
            return amount;
        }



    }
}
