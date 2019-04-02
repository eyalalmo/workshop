using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workshop192.Domain
{
    class StoreManager : StoreRole
    {
        private SubscribedUser appointedBy;
        private Store store;
        private SubscribedUser user;
        private Dictionary<string, bool> permissions;

        public StoreManager(SubscribedUser appointedBy, Store store,
            SubscribedUser user, Dictionary<string, bool> permissions) {
            this.appointedBy = appointedBy;
            this.store = store;
            this.user = user;
            this.permissions = permissions;
        }

        public string addManager(SubscribedUser manager, Dictionary<string, bool> permissions)
        {
            return user.getName() + " you're not is not allowed to add manager to " + store.getName();
        }

        public string addOwner(SubscribedUser owner)
        {
            return user.getName() + " you're not is not allowed to add owner to " + store.getName();
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

        public string addProduct(string name, string category, int price, int quantity)
        {
            if (!permissions["addProduct"])
                return user.getName() + " has no permission to add products in store " + store.getName();
            store.addProduct(new Product(name, category, price, 0, quantity));
            return "";
        }

        public string removeProduct(Product product)
        {
            if (!permissions["removeProduct"])
                return user.getName() + " has no permission to remove products in store " + store.getName();
            if (!store.productExists(product))
                return "product doesn't exsits";
            store.removeProduct(product);
            return "";
        }

        public string setProductPrice(Product product, int price)
        {
            if (!permissions["setProductPrice"])
                return user.getName() + " has no permission to set product's price in store " + store.getName();
            product.setPrice(price);
            return "";
        }

        public string setProductName(Product product, string name)
        {
            if (!permissions["setProductName"])
                return user.getName() + " has no permission to set product's name in store " + store.getName();
            product.setName(name);
            return "";
        }

        public string addToProductQuantity(Product product, int amount)
        {
            if (!permissions["addToProductQuantity"])
                return user.getName() + " has no permission to add to product's quantity in store " + store.getName();
            product.addQuantityLeft(amount);
            return "";
        }

        public string decFromProductQuantity(Product product, int amount)
        {
            if (!permissions["decFromProductQuantity"])
                return user.getName() + " has no permission to decrease from product's quantity in store " + store.getName();
            int curQuan = product.getQuantityLeft();
            if (curQuan < amount)
                return "-current quantity is " + curQuan + " and it can't be decreased by " + amount;
            product.decQuantityLeft(amount);
            return "";
        }

        public string setProductDiscount(Product product, Discount discount)
        {
            if (!permissions["setProductDiscount"])
                return user.getName() + " has no permission to set product's discount in store " + store.getName();
            product.setDiscount(discount);
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
    class StoreManager
    {
    }
}
