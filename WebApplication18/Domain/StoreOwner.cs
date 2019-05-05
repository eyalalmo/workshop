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
            DBProduct.getInstance().addProduct(product);
            return "";
        }

        public string removeProduct(Product product)
        {
            if (product.getStore() != store || !store.getProductList().Contains(product))
                return "product doesn't belong to this store";
            store.removeProduct(product);
            DBProduct.getInstance().removeProduct(product);
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
            //product.se tDiscount(discount);
            return "";
        }
        public void removeRoleAppointedByMe(StoreRole role)
        {
            appointedByMe.Remove(role);
        }
        public string addManager(SubscribedUser manager, Permissions permissions)
        {
            StoreRole newManager = new StoreManager(this.user, store, manager, permissions);
            DBStore.getInstance().addStoreRole(newManager);
            if (store.getStoreRole(manager) != null)
                return "user " + manager.getUsername() + " already have a role in store " + store.getStoreName();
            if (permissions == null)
                return "wrong permissions";
            store.addStoreRole(newManager);
            manager.addStoreRole(newManager);
            appointedByMe.Add(newManager);
            return "";
        }
        
        public string addOwner(SubscribedUser owner)
        {
            StoreRole newOwner = new StoreOwner(this.user, owner, store);
            if (store.getStoreRole(owner) != null)
                return "user " + owner.getUsername() + " already have a role in store " + store.getStoreName();
            store.addStoreRole(newOwner);
            owner.addStoreRole(newOwner);
            appointedByMe.Add(newOwner);
            DBStore.getInstance().addStoreRole(newOwner);
            return "";
        }

        public string remove(SubscribedUser role)
        {
            StoreRole sr = role.getStoreRole(store);
            DBStore.getInstance().removeStoreRole(sr);
            if (sr == null)
                return "user " + role.getUsername() + " doesn't have a role in store " + store.getStoreName();
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
