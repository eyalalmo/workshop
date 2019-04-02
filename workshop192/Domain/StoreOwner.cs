using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class StoreOwner : StoreRole
    {
        private SubscribedUser appointedBy;
        private Store store;
        private SubscribedUser user;

        public StoreOwner(SubscribedUser appointedBy, SubscribedUser user, Store store)
        {
            this.appointedBy = appointedBy;
            this.user = user;
            this.store = store;
        }

        public string addProduct(string name, string category, int price, int quantity)
        {
            store.addProduct(new Product(name, category, price, 0, quantity));
            return "";
        }

        public string removeProduct(Product product)
        {
            if (!store.productExists(product))
                return "product doesn't exsits";
            store.removeProduct(product);
            return "";
        }

        public string setProductPrice(Product product, int price)
        {
            product.setPrice(price);
            return "";
        }

        public string setProductName(Product product, string name)
        {
            product.setName(name);
        }

        public string addToProductQuantity(Product product, int amount)
        {
            product.addQuantityLeft(amount);
        }

        public string decFromProductQuantity(Product product, int amount)
        {
            int curQuan = product.getQuantityLeft();
            if (curQuan < amount)
                return "-current quantity is " + curQuan + " and it can't be decreased by " + amount;
            product.decQuantityLeft(amount);
            return "";
        }

        public string setProductDiscount(Product product, Discount discount)
        {
            product.setDiscount(discount);
        }

        public string addManager(SubscribedUser manager, Dictionary<string, bool> permissions)
        {
            DBStore storeDB = storeDatabase.getInstance();
            if (storeDB.getStoreRole(store, manager) == null)
                return "user " + manager.getUserName() + " already have a role in store " + store.getName();
            storeDB.addStoreRole(new StoreManager(this.user, store, manager, permissions));
            return "";
        }

        public String removeManager(SubscribedUser manager)
        {
            DBStore storeDB = storeDatabase.getInstance();
            StoreRole sr = storeDB.getStoreRole(store, manager);
            if (sr == null)
                return "user " + manager.getUserName() + " doesn't have a role in store " + store.getName();
            if (sr is StoreOwner)
                return "user " + manager.getUserName() + " is an owner of " + store.getName();
            storeDB.removeStoreRole(manager, store);
            return "";
        }

        public string addOwner(SubscribedUser owner)
        {
            DBStore storeDB = storeDatabase.getInstance();
            if (storeDB.getStoreRole(store, owner) == null)
                return "user " + owner.getUserName() + " already have a role in store " + store.getName();
            storeDB.addStoreRole(new StoreOwner(this.user, owner, store));
            return "";
        }

        public string removeOwner(SubscribedUser owner)
        {
            DBStore storeDB = storeDatabase.getInstance();
            StoreRole sr = storeDB.getStoreRole(store, owner);
            if (sr == null)
                return "user " + owner.getUserName() + " doesn't have a role in store " + store.getName();
            if (sr is StoreManager)
                return "user " + owner.getUserName() + " is a manager of " + store.getName();
            storeDB.removeStoreRole(owner, store);
            return "";
        }

        public string closeStore()
        {
            store.closeStore();
            return "";
        }

        public SubscribedUser getUser()
        {
            return user;
        }

        public Store getStore()
        {
            return store;
        }
<<<<<<< HEAD

=======
        
    class StoreOwner
    {
>>>>>>> origin/Stores_and_Products
    }
}
