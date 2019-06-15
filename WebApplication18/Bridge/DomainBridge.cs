using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication18.Domain;
using WebApplication18.Logs;
using workshop192.Domain;
using System.Security.Cryptography;


namespace workshop192.Bridge
{
    public class DomainBridge : Observer
    {
        private static DomainBridge instance;
        private static Messager messager;

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

        public void testSetup()
        {
            MarketSystem.initTestWitOutRead();
        }
        public void setup()
        {
            MarketSystem.init();
        }
        public void addAdmin(string name,string pass)
        {
            DBSubscribedUser.getInstance().addAdmin(name,encryptPassword(pass));
        }

        //use case 2.3
        public void login(int sessionid, String username, String password)
        {
            Session s = DBSession.getInstance().getSession(sessionid);
            s.login(username, password);
            SystemLogger.getEventLog().Info("User " + username + " has successfuly logged in.");

            LinkedList<string> waitingMessages = getMessagesFor(username);
            if (waitingMessages != null)
            {
                foreach (string mess in waitingMessages)
                    messager.message(username, mess);
            }
            clearMessagesFor(username);
        }

        public int payToExternal(string card, string month, string year, string holder, string ccv, string id)
        {
           return PaymentService.getInstance().checkOut(card, month, year, holder, ccv, id);
        }

        public int cancelPay(int result)
        {
            return PaymentService.getInstance().cancelPayment(result+""); 
        }

        public int deliverToExternal(string name, string address, string city, string country, string zip, string cvv)
        {
            return DeliveryService.getInstance().sendToUser(name, address, city, country, zip, cvv);
        }

        //use case 2.2
        public void register(int sessionid, String username, String password)
        {
            Session user = DBSession.getInstance().getSession(sessionid);
            user.register(username, password);
            //////
           // user.login(username,password);
            //////
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

        public int cancelDelivery(int result)
        {
            return DeliveryService.getInstance().cancelDelivery(result + "");
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

        internal LinkedList<int> getSessionByUserName(string username)
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

        internal LinkedList<string> getMessagesFor(string username)
        {
            return DBNotifications.getInstance().getMessagesFor(username);
        }

        internal void clearMessagesFor(string username)
        {
            DBNotifications.getInstance().clearMessagesFor(username);
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

        public bool handShakeDeliver()
        {
            return DeliveryService.getInstance().handShake();
        }

        public bool handShakePay()
        {
          return PaymentService.getInstance().handShake();
        }

        internal string generate()
        {
            return DBCookies.getInstance().generate();
        }

        internal void addWaitingMessage(Tuple<string, string> tuple)
        {
            DBNotifications.getInstance().addMessage(tuple);
        }

        internal int getUserByHash(string hash)
        {
            return DBCookies.getInstance().getUserByHash(hash);
        }

        internal void addSession(string hash, int session)
        {
            DBCookies.getInstance().addSession(hash, session);
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
        public void checkBasket(int session)
        {
            Session user = DBSession.getInstance().getSession(session);
             user.checkBasket();
        }

        internal void setquantityLeft(int productID, int setquantityLeft, int session)
        {
            Product p = DBProduct.getInstance().getProductByID(productID);
            p.setQuantityLeft(setquantityLeft);
        }

        public  void purchaseBasket(int sessionid, string address, string creditcard, string month, string year, string holder, string cvv)
        {
            Session session = DBSession.getInstance().getSession(sessionid);
            LinkedList<Tuple<string, string>> messages = new LinkedList<Tuple<string, string>>();
            
            ShoppingBasket basket = session.getShoppingBasket();
            foreach (KeyValuePair<int, ShoppingCart> cart in basket.getShoppingCarts())
            {
                foreach (KeyValuePair<Product, int> p in cart.Value.getProductsInCarts())
                {
                    foreach (StoreRole sr in p.Key.getStore().getRoles())
                    {
                        string username;
                        if (session.getSubscribedUser() != null)
                            username = session.getSubscribedUser().getUsername();
                        else
                            username = "A guest";
                        string message = username +
                                         " bought " + p.Key.getProductName() + " from store " +
                                         p.Key.getStore().getStoreName();
                        messages.AddFirst(new Tuple<string, string>(sr.getUser().getUsername(), message));
                    }
                }
            }

            session.purchaseBasket( address,  creditcard,  month,  year,  holder,  cvv);

            foreach (Tuple<string, string> t in messages)
                messager.message(t.Item1, t.Item2);

            SystemLogger.getEventLog().Info("A purchase has been made");
        }

        public void setProductRank(int productID, int rank, int session)
        {
            Product p = DBProduct.getInstance().getProductByID(productID);
             p.setRank(rank);
        }

        //public void addcouponToCart(int sessionID, int storeID, string couponCode)
        //{
        //    Session session = DBSession.getInstance().getSession(sessionID);
        //    ShoppingBasket shoppingBasket=  session.getShoppingBasket();
        //    shoppingBasket.addCoupon(couponCode, storeID);

        //}


        //public void removeCouponFromCart(int sessionID, int storeID)
        //{
        //    Session session = DBSession.getInstance().getSession(sessionID);
        //    ShoppingBasket shoppingBasket = session.getShoppingBasket();
        //    shoppingBasket.removeCoupon(storeID);
        //}

        public string getShoppingBasket(int sessionid)
        {
            Session session = DBSession.getInstance().getSession(sessionid);
            ShoppingBasket basket = session.getShoppingBasket();
            string response = "";
            foreach (KeyValuePair<int, ShoppingCart> cart in basket.getShoppingCarts())
            {
                foreach (KeyValuePair<Product, int> p in cart.Value.getProductsInCarts())
                {

                    response += p.Key.getProductName() + "," + p.Key.getPrice()+"," + p.Key.getActualPrice(p.Value) + "," + p.Key.getProductID() + "," + p.Value + ";";
                }
            }

            return response;
        }
        public double getShoppingBasketActualTotalPrice(int sessionid)
        {
            Session session = DBSession.getInstance().getSession(sessionid);
            return session.getShoppingBasket().getActualTotalPrice();
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
            SystemLogger.getEventLog().Info("New product:  " + product.getProductID() + " has successfuly added");
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
            if(s.getSubscribedUser() != null)
                return s.getSubscribedUser().getUsername();
            return "";
        }

        internal double getProductPrice(int productid)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);

            if (product == null)
                throw new DoesntExistException("no such product");

            return product.getPrice();
        }
        public void setProductInformation(int storeId,int productid,int price,String name,int rank,int quantityLeft,int sessionid)
        {
            Product product = DBProduct.getInstance().getProductByID(productid);
            if (product == null)
                throw new DoesntExistException("no such product");
            DBProduct.getInstance().update(product);
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
            ////
            messager.message(username, "Your role in store " + storeid + " has been removed");
        }

        //internal void addCouponToStore(int sessionID, int storeID, string couponCode, double percentage, string duration)
        //{
        //    Session user = DBSession.getInstance().getSession(sessionID);
        //    if (user == null)
        //        throw new DoesntExistException("Error: User is not logged in");
        //    Store store = DBStore.getInstance().getStore(storeID);
        //    SubscribedUser subscribedUser = user.getSubscribedUser();
        //    if (subscribedUser == null)
        //        throw new DoesntExistException("not a subscribed user");
        //    StoreRole sr = subscribedUser.getStoreRole(store);
        //    if (sr == null)
        //        throw new RoleException("no role for this user in this store");
        //    sr.addCouponToStore( couponCode, percentage, duration);
        //}

        //internal void removeCouponFromStore(int sessionID, int storeID, string couponCode)
        //{
        //    Session user = DBSession.getInstance().getSession(sessionID);
        //    if (user == null)
        //        throw new DoesntExistException("user is not logged in");
        //    Store store = DBStore.getInstance().getStore(storeID);
        //    SubscribedUser subscribedUser = user.getSubscribedUser();
        //    if (subscribedUser == null)
        //        throw new DoesntExistException("not a subscribed user");
        //    StoreRole sr = subscribedUser.getStoreRole(store);
        //    if (sr == null)
        //        throw new RoleException("no role for this user in this store");
        //    sr.removeCouponFromStore(couponCode);
        //}

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
        public void changeQuantity(int sessionid, int product, int newAmount)
        {
            if (newAmount <= 0)
            {
                throw new ArgumentException("Error: Quantity should be a positive number");
            }
            Product p = DBProduct.getInstance().getProductByID(product);
            Store store = p.getStore();

            Session user = DBSession.getInstance().getSession(sessionid);
            user.getShoppingBasket().changeQuantityOfProduct(store.getStoreID(), p, newAmount);
        }

        public  void checkoutBasket(int sessionid, string address, string creditcard, string month, string year, string holder, string cvv)
        {
            Session user = DBSession.getInstance().getSession(sessionid);

             user.getShoppingBasket().purchaseBasket(address, creditcard, month, year, holder, cvv);
        }

        public string getAllStores(int session1)
        {
            Session session = DBSession.getInstance().getSession(session1);
            string userName = session.getSubscribedUser().getUsername();
            DBStore.getInstance().initStoresAndRolesForUserName(userName);
            List<StoreRole> lst = session.getSubscribedUser().getStoreRoles();
            LinkedList<Store> stores = new LinkedList<Store>();
            foreach (StoreRole element in lst)
            {
                if(!stores.Contains(element.getStore()))
                    stores.AddLast(element.getStore());
            }
            return JsonConvert.SerializeObject(stores); 
        }

        ////////////////////////////////////////////
        public string getStoreDiscounts(int storeID, int sessionID)
        {
            Session user = DBSession.getInstance().getSession(sessionID);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            LinkedList<DiscountComponent> discounts = store.getDiscounts();
            string str = "";
            foreach(DiscountComponent dis in discounts)
            {
               
 
                    str += dis.getDiscountType() + "," + dis.description() + "," + dis.getPercentage()*100 + "," + dis.getDuration().ToString("dd/MM/yyyy") + "," + dis.getId() + ";";
        
               /* if(dis is DiscountComposite)
                {
                    DiscountComposite d = (DiscountComposite)dis;
                    str += dis.getDiscountType() + "," + dis.description() + "," + 100 + "," + 12 + "," + d.getId() + ";";
                }*/
            
            }
            return str;
        }
        public string getStorePolicies(int storeID, int sessionID)
        {
            Session user = DBSession.getInstance().getSession(sessionID);
            if (user == null)
                throw new DoesntExistException("user is not logged in");
            Store store = DBStore.getInstance().getStore(storeID);
            //bar
            //go over purchase policy
            LinkedList<DiscountComponent> discounts = store.getDiscounts();
            string str = "";
            foreach (DiscountComponent dis in discounts)
            {

                //bar instead of sending percentage, send amount
                str += dis.getDiscountType() + "," + dis.description() + "," + dis.getPercentage() * 100  + "," + dis.getId() + ";";

                /* if(dis is DiscountComposite)
                 {
                     DiscountComposite d = (DiscountComposite)dis;
                     str += dis.getDiscountType() + "," + dis.description() + "," + 100 + "," + 12 + "," + d.getId() + ";";
                 }*/

            }
            return str;
        }
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
            checkDiscoutDuration(duration);
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

        private void checkDiscoutDuration(string duration)
        {
            if (duration.Length != 10)
                throw new ArgumentException("Discount duration is not in the required foramt DD/MM/YYYY");
            char[] arr = duration.ToCharArray();
            if(arr[2] != '/' || arr[5] != '/')
                throw new ArgumentException("Discount duration is not in the required foramt DD/MM/YYYY");
            for(int i=0; i<arr.Length; i++)
            {
                if(i!= 2 && i != 5)
                {
                    if(arr[i]<'0' ||arr[i] >'9')
                        throw new ArgumentException("Discount duration is not in the required foramt DD/MM/YYYY");

                }
            }

            int day = Int32.Parse(duration.Substring(0, 2));
            int month = Int32.Parse(duration.Substring(3, 2));
            int year = Int32.Parse(duration.Substring(6, 4));
            if(day==0 || day >31)
                throw new ArgumentException("Date is not valid");
            if(month==0 || month >12)
                throw new ArgumentException("Date is not valid");
            if(year < 2019)
                throw new ArgumentException("Date is not valid");
            DateTime d = new DateTime(year, month, day);
            DateTime now = DateTime.Now;
            if(DateTime.Compare(d, now) <0)
                throw new ArgumentException("Discount duration must be future dateד");
        }

        internal void addProductVisibleDiscount(int product, double percentage, string duration, int session)
        {
            checkDiscoutDuration(duration);
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
            checkDiscoutDuration(duration);
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
            checkDiscoutDuration(duration);
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

        internal void removeStoreDiscount(int discountID, int storeID, int session)
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

            sr.removeStoreDiscount(discountID, store);

        }
        internal void removeStorePolicy(int policyID, int storeID, int session)
        {
            //bar
            //delete policy from store
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

            sr.removeStoreDiscount(policyID, store);

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
        public void complexDiscount(string discountString, int storeID,string type, double percentage, string duration, int sessionID)
        {
            checkDiscoutDuration(duration);
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
            string[] discountArray = discountString.Split(' ');
            List<DiscountComponent> discounts = new List<DiscountComponent>();
            LinkedList<DiscountComponent> storediscounts = store.getDiscounts();
            for (int i=0; i<discountArray.Length-1; i++)
            {
                int index = Int32.Parse(discountArray[i]);
                discounts.Add(storediscounts.ElementAt(index));
            }
            sr.addComplexDiscount(discounts, type, percentage, duration);
        }
        public void complexPolicy(string policyString, int storeID, string type, int sessionID)
        {
            //bar
            //change to complex policies
            //checkDiscoutDuration(duration);
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
            string[] discountArray = policyString.Split(' ');
            List<DiscountComponent> discounts = new List<DiscountComponent>();
            LinkedList<DiscountComponent> storediscounts = store.getDiscounts();
            for (int i = 0; i < discountArray.Length - 1; i++)
            {
                int index = Int32.Parse(discountArray[i]);
                discounts.Add(storediscounts.ElementAt(index));
            }
            sr.addComplexDiscount(discounts, type, 0.01, "12/12/2020");
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
            double amount = sc1.getActualTotalPrice();
            return amount;
        }
        public void setDiscountPercentage(int discountID, double percentage)
        {
            if (percentage > 1||percentage<=0)
            {
            }
            else
            {
                DiscountComponent d = DBDiscount.getInstance().getDiscountByID(discountID);
                DBDiscount.getInstance().setPercentage(d.getId(), percentage);
                if (d is Discount)
                    ((Discount)d).setPercentage(percentage);
            }
        }
        public void setPolicyAmount(int policyID, int amount)
        {
            if (amount<=0)
            {
            }
            else
            {
                //bar
                //change amount of policy
                double p = amount / 100.0; //delete 
                DiscountComponent d = DBDiscount.getInstance().getDiscountByID(policyID);
                DBDiscount.getInstance().setPercentage(d.getId(), p);
                if (d is Discount)
                    ((Discount)d).setPercentage(p);
            }
        }

        public void observe(Messager m)
        {
            messager = m;
        }

        public void addPendingOwner(int storeid, string username, int sessionid)
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
            if (store.getStoreRole(toAdd) != null || store.getPending().Contains(username))
                throw new RoleException("Error: Username " + toAdd.getUsername() +
                    " already has a role in store " +
                    store.getStoreName());
            if (store.getNumberOfOwners() == 1)
            {
                sr.addOwner(toAdd);
            }
            else
            {
                sr.addPendingOwner(toAdd);
                foreach (StoreRole role in store.getRoles()) // send messages to all the owners in the store - to approve the new owner
                {
                    string message = "User " + username + "has been offered as an Owner to the store " +
                    store.getStoreName() + ". Please approve or decline the partnership.";
                    if (role is StoreOwner && role != sr)
                    {
                        messager.message(role.getUser().getUsername(), message);
                    }
                }
            }

        }

        public void signContract(int storeid, string username, int sessionid)
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
                sr.signContract( toAdd);

        }
        public void declineContract(int storeid, string username, int sessionid)
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
                throw new RoleException("Error: You don't have permissions to decline a contract ");

            if (sr.getStore() != store)
                throw new RoleException("this user can't appoint to this store");
            sr.declineContract(toAdd);

        }

        public string getAllPending(int storeid,int sessionid)
        {
            Store store = DBStore.getInstance().getStore(storeid);
            if (store == null)
            {
                throw new DoesntExistException("no such store");
            }

            Session session = DBSession.getInstance().getSession(sessionid);

            StoreRole sr = store.getStoreRole(session.getSubscribedUser());

            if (sr == null)
                throw new RoleException("Error: You don't have permissions to decline a contract ");

            if (sr.getStore() != store)
                throw new RoleException("this user can't appoint to this store");
            List<string> myPendingOwners = new List<string>();
            LinkedList<string> pending = store.getPending();
            foreach (string pendingOwner in pending)
            {
                if (!(DBStore.getInstance().hasContract(storeid,pendingOwner,sr.getUser().getUsername())))
                    myPendingOwners.Add(pendingOwner);
            }
            string s = JsonConvert.SerializeObject(myPendingOwners, Formatting.Indented);
            return s;
        }

        public int getNumOfProductsInBasket(int session) {
            int counter = 0;
            Session s = DBSession.getInstance().getSession(session);
            Dictionary<int, ShoppingCart> d = s.getShoppingBasket().getShoppingCarts();
            foreach (KeyValuePair<int, ShoppingCart> a in d) {
                Dictionary<Product, int> d1 = a.Value.getProductsInCarts();
                    foreach(KeyValuePair<Product, int> p in d1)
                        counter++;
            }
            return counter;
        }
        public string encryptPassword(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }
}
