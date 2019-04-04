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
        private List<StoreRole> appointedByMe;

        public StoreOwner(SubscribedUser appointedBy, SubscribedUser user, Store store)
        {
            this.appointedBy = appointedBy;
            this.user = user;
            this.store = store;
            appointedByMe = new List<StoreRole>();
        }

        public string addProduct(Product product)
        {
            store.addProduct(product);
            return "";
        }

        public string removeProduct(Product product)
        {
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
            //product.setDiscount(discount);
            return "";
        }

        public string addManager(SubscribedUser manager, Permissions permissions)
        {
            if (store.getStoreRole(manager)!=null)
                return "user " + manager.getUsername() + " already have a role in store " + store.getStoreName();
            StoreRole newManeger = new StoreManager(this.user, store, manager, permissions);
            store.addStoreRole(newManeger);
            manager.addStoreRole(newManeger);
            appointedByMe.Add(newManeger);
            return "";
        }
        
        public string addOwner(SubscribedUser owner)
        {
            if (store.getStoreRole(owner) != null)
                return "user " + owner.getUsername() + " already have a role in store " + store.getStoreName();
            StoreRole newOwner = new StoreOwner(this.user, owner, store);
            store.addStoreRole(newOwner);
            owner.addStoreRole(newOwner);
            appointedByMe.Add(newOwner);
            return "";
        }

        public string remove(SubscribedUser role)
        {
            StoreRole sr = role.getStoreRole(store);
            //if (sr == null || sr is StoreManager)
            //    return "user " + owner.getUsername() + " is not an owner in store " + store.getStoreName();
            if (sr.getAppointedBy() != this.user)
                return "user " + user.getUsername() + " didn't appoint " + role.getUsername();
            sr.removeAllAppointedBy();
            role.removeStoreRole(sr);
            store.removeStoreRole(sr);
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

        public void removeAllAppointedBy()
        {
            foreach (StoreRole sr in appointedByMe)
                remove(sr.getUser());
        }

    }
}
