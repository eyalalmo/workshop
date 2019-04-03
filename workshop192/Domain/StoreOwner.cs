using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.Domain
{


    public class StoreOwner : StoreRole

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
            store.addProduct(new Product(name, category, price, 0, quantity, store));
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
            product.setProductName(name);
            return "";
        }

        public string addToProductQuantity(Product product, int amount)
        {
            product.addQuantityLeft(amount);
            return "";
        }

        public string decFromProductQuantity(Product product, int amount)
        {
            int curQuan = product.getQuantityLeft();
            if (curQuan < amount)
                return "current quantity is " + curQuan + " and it can't be decreased by " + amount;
            product.decQuantityLeft(amount);
            return "";
        }

        public string setProductDiscount(Product product, Discount discount)
        {
            product.setDiscount(discount);
            return "";
        }

        public string addManager(SubscribedUser manager, Dictionary<string, bool> permissions)
        {
            DBStore storeDB = DBStore.getInstance();
            if (storeDB.getStoreRole(store, manager) == null)
                return "user " + manager.getUsername() + " already have a role in store " + store.getStoreName();
            storeDB.addStoreRole(new StoreManager(this.user, store, manager, permissions));
            return "";
        }

        public String removeManager(SubscribedUser manager)
        {
            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(store, manager);
            if (sr == null)
                return "user " + manager.getUsername() + " doesn't have a role in store " + store.getStoreName();
            if (sr is StoreOwner)
                return "user " + manager.getUsername() + " is an owner of " + store.getStoreName();
            if (sr.getAppointedBy() != this.user)
                return "user " + user.getUsername() + " didn't appointed " + manager.getUsername();
            storeDB.removeStoreRole(store, manager);
            return "";
        }

        public string addOwner(SubscribedUser owner)
        {
            DBStore storeDB = DBStore.getInstance();
            if (storeDB.getStoreRole(store, owner) == null)
                return "user " + owner.getUsername() + " already have a role in store " + store.getStoreName();
            storeDB.addStoreRole(new StoreOwner(this.user, owner, store));
            return "";
        }

        public string removeOwner(SubscribedUser owner)
        {
            DBStore storeDB = DBStore.getInstance();
            StoreRole sr = storeDB.getStoreRole(store, owner);
            if (sr == null)
                return "user " + owner.getUsername() + " doesn't have a role in store " + store.getStoreName();
            if (sr is StoreManager)
                return "user " + owner.getUsername() + " is a manager of " + store.getStoreName();
            if (sr.getAppointedBy() != this.user)
                return "user " + user.getUsername() + " didn't appointed " + owner.getUsername();
            List<SubscribedUser> appointedByOwner = storeDB.getAppointedByList(owner);
            foreach (SubscribedUser cur in appointedByOwner)
                sr.removeOwner(cur);
            storeDB.removeStoreRole(store, owner);
            return "";
        }

        public string closeStore()
        {
            DBStore storeDB = DBStore.getInstance();
            storeDB.closeStore(store);
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

        public SubscribedUser getAppointedBy()
        {
            return appointedBy;
        }
    }
}
