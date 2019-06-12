using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication18;
using Dapper.Contrib;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication18.Domain;

namespace workshop192.Domain
{
    public class Store
    {
        private int storeId;
        private string name;
        private string description;
        private LinkedList<Product> productList;
        private List<StoreRole> roles;
        private int numOfOwners;
        private bool active;
        /* private MinAmountPurchase minPurchasePolicy;
         private MaxAmountPurchase maxPurchasePolicy;
         private TotalPricePolicy minTotalprice;
        private ComplexPurchasePolicy complexPurchase;
             * */
        private LinkedList<PurchasePolicy> policies;
        

        private LinkedList<InvisibleDiscount> invisibleDiscountList;
        [JsonIgnore]
        public LinkedList<DiscountComponent> discountList;
        public Dictionary<string, HashSet<string>> pendingOwners;

        public Store(string storeName, string description)
        {
            this.storeId = DBStore.getInstance().getNextStoreID();
            this.name = storeName;
            this.description = description;
            productList = new LinkedList<Product>();
            roles = new List<StoreRole>();
            numOfOwners = 0;
            active = true;
            discountList = new LinkedList<DiscountComponent>();
            policies = new LinkedList<PurchasePolicy>();
           /* maxPurchasePolicy = null;
            minPurchasePolicy = null;
            minTotalprice = null;
            pendingOwners = new Dictionary<string, HashSet<string>>();
            complexPurchase = null;
            */
        }
        public Store(int storeId,string name, string description)
        {
            this.storeId = storeId;
            this.name = name;
            this.description = description;
            productList = new LinkedList<Product>();
            roles = new List<StoreRole>();
            numOfOwners = 0;
            active = true;
            pendingOwners = new Dictionary<string, HashSet<string>>();
            discountList = new LinkedList<DiscountComponent>();
            invisibleDiscountList = new LinkedList<InvisibleDiscount>();
            /*
            maxPurchasePolicy = null;
            minPurchasePolicy = null;
            minTotalprice = null;
            complexPurchase = null;
            */
        }

        public void addProduct(Product p)
        {
            productList.AddFirst(p);
        }
        public LinkedList<DiscountComponent> getDiscounts()
        {
            return this.discountList;
        }
        public bool isActive()
        {
            return this.active;
        }
        public void closeStore()
        {
            active = false;
        }
        public void openStore()
        {
            active = true;
        }
        public void removeProduct(Product p)
        {
            productList.Remove(p);
        }
        public bool productExists(int productID)
        {
            foreach (Product p in productList)
            {
                if (p.getProductID() == productID)
                    return true;
            }
            return false;
        }

        public void addStoreRoleFromInitOwner(StoreOwner so)
        {
            roles.Add(so);
            numOfOwners++;
        }

        public string getProductsString()
        {
            string s = JsonConvert.SerializeObject(this.productList);
            return s;
        }

        public bool productExists(Product product)
        {
            foreach (Product p in productList)
            {
                if (product.Equals(p))
                    return true;
            }
            return false;
        }
        public LinkedList<Product> getProductList()
        {
            return this.productList;
        }

        public int getStoreID()
        {
            return this.storeId;
        }
        public void setStoreID(int id)
        {
            this.storeId = id;
        }

        public String getStoreName()
        {
            return this.name;
        }

        public void setStoreName(String storeName)
        {
            this.name = storeName;
        }

        public void addStoreRoleFromInitManager(StoreRole so)
        {
            roles.Add(so);
        }

        public String getDescription()
        {
            return this.description;
        }

        public void setDescription(String description)
        {
            this.description = description;
        }

        public void addStoreRoleFromInitOwner(StoreRole toAdd)
        {
            if (toAdd is StoreOwner)
            {
                numOfOwners++;
                DBStore.getInstance().addownerNumerByOne(storeId, numOfOwners);
            }
            roles.Add(toAdd);
        }

        public void addCoupon(string coupon, double percentage, string duration)
        {
            foreach (InvisibleDiscount d1 in invisibleDiscountList)
            {
                string c = d1.getCoupon();
                if (c == coupon)
                    throw new AlreadyExistException("Error: A Store cannot have two identical coupons");

            }
            InvisibleDiscount d = new InvisibleDiscount(percentage, coupon, duration);
            invisibleDiscountList.AddLast(d);
        }

        public void removeCoupon(string coupon)
        {
            bool found = false;
            foreach (InvisibleDiscount d1 in invisibleDiscountList)
            {
                string c = d1.getCoupon();
                if (c == coupon)
                {
                    found = true;
                    invisibleDiscountList.Remove(d1);
                    break;
                }
            }

            if (!found)
                throw new DoesntExistException("Error:Coupon does not exist");
        }

        public void checkCouponCode(string coupon)
        {
            bool found = false;
            foreach (InvisibleDiscount d in invisibleDiscountList)
            {
                string c = d.getCoupon();
                if (c == coupon)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                throw new ArgumentException("Error:Coupon does not exist");
        }


        public Dictionary<Product, double> updatePrice(Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            foreach (DiscountComponent d in discountList)
            {
                if (d.checkCondition(productList, productsActualPrice))
                {
                    productsActualPrice = d.updatePrice(productList, productsActualPrice);
                }
            }
            return productsActualPrice;

        }


        public Dictionary<Product, double> updatePriceAfterCoupon(string coupon, Dictionary<Product, int> productList, Dictionary<Product, double> productsActualPrice)
        {
            foreach (InvisibleDiscount d in invisibleDiscountList)
            {
                if (coupon == d.getCoupon())
                {
                    productsActualPrice = d.updatePrice(productList, productsActualPrice);
                    break;
                }
            }
            return productsActualPrice;

        }



        public int getNumberOfOwners()
        {
            return numOfOwners;
        }

        public List<StoreRole> getRoles()
        {
            return roles;
        }

        public StoreRole getStoreRole(SubscribedUser user)
        {
            foreach (StoreRole sr in roles)
            {
                if (sr.getUser() == user)
                {
                    return sr;
                }

            }
            return null;
        }

       
        public void removeStoreRole(StoreRole toRemove)
        {
            if (toRemove is StoreOwner)
            {
                numOfOwners--;
                DBStore.getInstance().removeOwnerNumerByOne(storeId, numOfOwners);

            }
            roles.Remove(toRemove);
            DBStore.getInstance().removeStoreRole(toRemove);
        }


        public void addDiscount(DiscountComponent d)
        {
            discountList.AddLast(d);
        }

        public void removeDiscount(int discountID)
        {
            foreach (DiscountComponent d in discountList)
            {
                if (d.getId() == discountID)
                {
                    discountList.Remove(d);
                    break;
                }
            }
        }
 
        public void setMinPurchasePolicy(int minAmount, int index)
        {
            PurchasePolicy p = policies.ElementAt(index);

            if (getMaxPolicy() == null)
            {
                minPurchasePolicy = new MinAmountPurchase(minAmount);
            }
            else
            {
                if (maxPurchasePolicy.getAmount() < minAmount)
                {
                    throw new ArgumentException("contradiction! maximum amount can not be smaller than minimum amount Purchase Policy");
                }
                else
                {
                    minPurchasePolicy = new MinAmountPurchase(minAmount);
                }
            }

        }

        private object getMaxPolicy()
        {
            foreach()

        }

        public void setMaxPurchasePolicy(int maxAmount)
        {
            if (minPurchasePolicy == null)
            {
                maxPurchasePolicy = new MaxAmountPurchase(maxAmount);
            }
            else
            {
                if (minPurchasePolicy.getAmount() > maxAmount)
                {
                    throw new ArgumentException("contradiction! maximum amount can not be smaller than minimum amount Purchase Policy");
                }
                else
                {
                    maxPurchasePolicy = new MaxAmountPurchase(maxAmount);
                }

            }
        }

        public bool checkStorePolicy(int amount, double cartTotalPrice)
        {
            bool ans = true;
            if (maxPurchasePolicy != null)
            {
                ans = ans & maxPurchasePolicy.checkPolicy(cartTotalPrice, amount);
            }
            if (minPurchasePolicy != null)
            {
                ans = ans & minPurchasePolicy.checkPolicy(cartTotalPrice, amount);
            }
            if(minTotalprice != null)
            {
                ans = ans & minTotalprice.checkPolicy(cartTotalPrice, amount);
            }
            if(complexPurchase != null)
            {
                ans = ans & complexPurchase.checkPolicy(cartTotalPrice, amount);
            }
            return ans;
        }

        public void removeMaxAMountPolicy()
        {
            maxPurchasePolicy = null;

        }

        public void removeMinAmountPolicy()
        {
            minPurchasePolicy = null;
        }

        public void removeTotalPricePolicy()
        {
            minTotalprice = null;
        }

        public void removeComplexPolicy()
        {
            complexPurchase = null;
        }
        public bool hasMinPurchasePolicy()
        {
            return (minPurchasePolicy != null);
        }
        public bool hasMaxPurchasePolicy()
        {
            return (maxPurchasePolicy != null);
        }
        
        public addComplexPurchasePolicy(PurchasePolicy p1, PurchasePolicy p2, string type)
        {
            complexPurchase  = new ComplexPurchasePolicy()
        }

    
        public VisibleDiscount getVisibleDiscount()
        {
            foreach(Discount d in discountList)
            {
                if (d is VisibleDiscount)
                    return (VisibleDiscount)d;
            }
            return null;
        }
        public void addPendingOwner(string appointer,SubscribedUser pending)
        {
            if (pendingOwners.ContainsKey(pending.username))
            {
                throw new AlreadyExistException("Owner already waiting for approval");
            }
            HashSet<string> toAdd = new HashSet<string>();
            toAdd.Add(appointer);
            pendingOwners.Add(pending.getUsername(), toAdd);

        }
        public void removePendingOwner(SubscribedUser pending)
        {
            if (!pendingOwners.ContainsKey(pending.getUsername()))
            {
                throw new DoesntExistException("the username is not in the owners pending list");
            }
            pendingOwners.Remove(pending.getUsername());
        }

        public void signContract(string owner,SubscribedUser pending)
        {
            if (!pendingOwners.ContainsKey(pending.getUsername()))
            {
                throw new DoesntExistException("the username is not in the owners pending list");
            }
            HashSet<string> temp = new HashSet<string>();
            pendingOwners.TryGetValue(pending.getUsername(), out temp);
            temp.Add(owner);
            pendingOwners[pending.getUsername()] = temp;
        }

        public HashSet<string> getApproved(SubscribedUser pending)
        {
            HashSet<string> output;
            if(pendingOwners.TryGetValue(pending.getUsername(), out output))
            {
                return output;
            }
            throw new DoesntExistException("User is not a pending owner");
        }

        public Dictionary<string,HashSet<string>> getPending()
        {
            return pendingOwners;
        }

    }

}
