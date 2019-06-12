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
        public SubscribedUser appointedBy;
        private Store store;
        public SubscribedUser userName;
        private List<StoreRole> appointedByMe;
        public bool isOwner = true;

        public StoreOwner(SubscribedUser appointedBy, SubscribedUser user, Store store)
        {
            this.appointedBy = appointedBy;
            this.userName = user;
            this.store = store;
            appointedByMe = new List<StoreRole>();
        }
        public int getIsOwner()
        {
            return 1;
        }
        public StoreOwner()
        {
        }

        public void addProduct(Product product)
        {
            store.addProduct(product);
            DBProduct.getInstance().addProduct(product);
        }

        public void removeProduct(Product product)
        {
            if (product.getStore() != store || !store.getProductList().Contains(product))
                throw new StoreException("product " 
                    + product.getProductName() + " doesn't belong to store " 
                    + store.getStoreName());
            store.removeProduct(product);
            DBProduct.getInstance().removeProduct(product);
        }

        public void setProductPrice(Product product, int price)
        {
            product.setPrice(price);
        }

        public void setProductName(Product product, string name)
        {
            product.setProductName(name);
        }

        public void addToProductQuantity(Product product, int amount)
        {
            product.addQuantityLeft(amount);
        }

        public void decFromProductQuantity(Product product, int amount)
        {
            int curQuan = product.getQuantityLeft();
            if (curQuan < amount)
                throw new AlreadyExistException("product "
                    + product.getProductName() + " quantity is " + product.getQuantityLeft() +
                    "so you decrease " + amount + " of its quantity");
            product.decQuantityLeft(amount);
        }

        public void setProductDiscount(Product product, DiscountComponent discount)
        {
            //product.setDiscount(discount);
        }

        public void removeRoleAppointedByMe(StoreRole role)
        {
            appointedByMe.Remove(role);
        }

        public void addManager(SubscribedUser manager, Permissions permissions)
        {
            StoreRole newManager = new StoreManager(this.userName, store, manager, permissions);
            DBStore.getInstance().addStoreRole(newManager);
            if (store.getStoreRole(manager) != null)
                throw new RoleException("Error: Username "  + manager.getUsername() + 
                    " already has a role in store " + 
                    store.getStoreName());
            store.addStoreRoleFromInitOwner(newManager);
            manager.addStoreRole(newManager);
            appointedByMe.Add(newManager);
        }
        
        public void addOwner(SubscribedUser owner)
        {
            StoreRole newOwner = new StoreOwner(this.userName, owner, store);
            if (store.getStoreRole(owner) != null)
                throw new RoleException("Error: Username " + owner.getUsername() +
                    " already has a role in store " +
                    store.getStoreName());
            store.addStoreRoleFromInitOwner(newOwner);
            owner.addStoreRole(newOwner);
            appointedByMe.Add(newOwner);
            DBStore.getInstance().addStoreRole(newOwner);
        }

        public void remove(SubscribedUser role)
        {
            StoreRole sr = role.getStoreRole(store);
            DBStore.getInstance().removeStoreRole(sr);
            if (sr == null)
                throw new RoleException("user " + role.getUsername() + 
                    " doesn't have a role in store " 
                    + store.getStoreName());
            if (sr.getAppointedBy() != this.userName)
                throw new RoleException("Error: User " + userName.getUsername() + 
                    " didn't appoint " + 
                    role.getUsername());
            sr.removeAllAppointedBy();
            role.removeStoreRole(sr);
            store.removeStoreRole(sr);
        }

        public void closeStore()
        {
            DBStore storeDB = DBStore.getInstance();
            storeDB.closeStore(store);
        }

        public SubscribedUser getUser()
        {
            return userName;
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

        

        public void addProductVisibleDiscount(Product product, double percentage, string duration)
        {
            VisibleDiscount discount = new VisibleDiscount(percentage, duration, "ProductVisibleDiscount");
            discount.setProduct(product);
            product.setDiscount(discount);
            Store store = product.getStore();
            //store.addDiscount(discount);
            DBDiscount.getInstance().addDiscount(discount);

        }
        public void removeProductDiscount(Product product)
        {
            product.removeDiscount();
        }

        public void addStoreVisibleDiscount(double percentage, string duration)
        {
            VisibleDiscount v = new VisibleDiscount(percentage, duration, "StoreVisibleDiscount");
            store.addDiscount(v);
            DBDiscount.getInstance().addDiscount(v);

        }

        public void addReliantDiscountSameProduct(double percentage, String duration, int numOfProducts, Product product)
        {
            ReliantDiscount r = new ReliantDiscount(percentage, duration, numOfProducts, product);
            store.addDiscount(r);
            product.setReliantDiscountSameProduct(r);
            DBDiscount.getInstance().addDiscount(r);

        }

        public void addReliantDiscountTotalAmount(double percentage, String duration, int amount)
        {
            ReliantDiscount r = new ReliantDiscount(percentage, duration, amount);
            store.addDiscount(r);
            DBDiscount.getInstance().addDiscount(r);

        }

        public void removeStoreDiscount(int discountID, Store store)
        {
           // DBDiscount.getInstance().removeDiscount(discountID);
            store.removeDiscount(discountID);

        }
        public void addComplexDiscount(List<DiscountComponent> list, string type, double percentage, string duration)
        {
            DiscountComposite composite = new DiscountComposite(list, type, percentage, duration);
            store.addDiscount(composite);
            foreach (DiscountComponent d in list)
            {
                store.removeDiscount(d.getId());
                if (d is Discount)
                {
                    Discount di = (Discount)d;
                    di.setIsPartOfComplex(true);
                }
            }
            //DBDiscount.getInstance().addDiscount(composite);

        }

        
        public void removeCouponFromStore(string couponCode)
        {
            store.removeCoupon(couponCode);
        }

        /* public void addCouponToStore(string couponCode, int percentage, string duration)
        {
            store.addCoupon(couponCode, percentage, duration);
        }
        */
        public void addCouponToStore(string couponCode, double percentage, string duration)
        {
            store.addCoupon(couponCode, percentage, duration);
        }

        public void addPendingOwner(SubscribedUser pending)
        {
            store.addPendingOwner(userName.getUsername(), pending);
        }
        public void signContract(string owner, SubscribedUser pending)
        {
            store.signContract(owner, pending);
            HashSet<string> approvedOwners = store.getApproved(pending);
            if (approvedOwners.Count == store.getNumberOfOwners())
            {
                store.removePendingOwner(pending);
                addOwner(pending);
            }
        }
        public void declineContract(string owner, SubscribedUser pending)
        {
            store.removePendingOwner(pending);
        }
        public Permissions GetPermissions()
        {
            return new Permissions(true, true, true);
        }

        public void addComplexPolicy(int index1, int index2, string type)
        {

            store.addComplexPurchasePolicy(index1, index2, type);
        }

        public void removePolicy(int index)
        {
            store.removePolicyByindex(index);
        }

        public void setPolicyByIndex(int newAmount, int index)
        {
            store.setPolicyByID(newAmount, index);
        }
        public void addMinPurchasePolicy(int amount)
        {
            store.addMinAmountPolicy(amount);
        }

        public void addMaxPurchasePolicy(int amount)
        {
            store.addMaxAmountPolicy(amount);
        }

        public void addTotalPricePurchasePolicy(int amount)
        {
            store.addTotalAmountPolicy(amount);
        }
    }
}











