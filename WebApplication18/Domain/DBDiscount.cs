using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WebApplication18.DAL;
using WebApplication18.Domain;

namespace workshop192.Domain
{
    public class DBDiscount : Connector
    {
        private static DBDiscount instance;
        private LinkedList<DiscountComponent> discounts;
        private static int nextID;

        public static DBDiscount getInstance()
        {
            if (instance == null)
            {
                instance = new DBDiscount();
            }
            return instance;
        }

        private DBDiscount()
        {
            discounts = new LinkedList<DiscountComponent>();
            nextID = 1;
        }

        public void init()
        {

        }

        public void initTests()
        {
            init();
        }

        public void addDiscount(DiscountComponent d)
        {
            try
            {
                connection.Open();
                string sql = "INSERT INTO [dbo].[DiscountComponent] (id, percentage, duration, type, storeId)" +
                                " VALUES (@id,@percentage, @duration, @type, @storeId)";
                if (d is Discount)
                {
                    connection.Execute(sql, new
                    {
                        id = d.getId(),
                        percentage = d.getPercentage(),
                        duration = d.getDuration(),
                        type = "Discount",
                        storeId = d.getStoreId()
                    });
                    if (d is VisibleDiscount)
                    {
                        VisibleDiscount v = (VisibleDiscount)d;
                        addVisibleDiscount(v);
                    }
                    if (d is ReliantDiscount)
                    {
                        ReliantDiscount r = (ReliantDiscount)d;
                        addReliantDiscount(r);
                    }
                }
                if (d is DiscountComposite)
                {
                    DiscountComposite composite = (DiscountComposite)d;
                    connection.Execute(sql, new
                    {
                        id = d.getId(),
                        percentage = d.getPercentage(),
                        duration = d.getDuration(),
                        type = "Composite",
                        storeId = d.getStoreId()

                    });
                    foreach (DiscountComponent child in composite.getChildren())
                    {
                        string sql2 = "INSERT INTO [dbo].[DiscountComposite] (id,childid,type)" +
                                " VALUES (@id, @childid,@type)";

                        connection.Execute(sql, new
                        {
                            id = d.getId(),
                            childid = child.getId(),
                            type = composite.getType()
                        });
                    }
                }

                connection.Close();
                discounts.AddFirst(d);

            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }
        public void removeDiscount(DiscountComponent d)
        {
            //if (!discounts.ContainsKey(d))
            //  throw new DoesntExistException("Error: Discount does not exist");

            try
            {
                connection.Open();
                connection.Execute("DELETE FROM DiscountComponent WHERE id=@id ", new { id = d.getId() });
                if (d is Discount)
                {
                    connection.Execute("DELETE FROM Discount WHERE id=@id ", new { id = d.getId() });
                }
                else
                    connection.Execute("DELETE FROM DiscountComposite WHERE id=@id ", new { id = d.getId() });

                connection.Execute("DELETE FROM Discount WHERE childid=@childid ", new { childid = d.getId() });
                discounts.Remove(d);
                connection.Close();
            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }

        public static int getNextDiscountID()
        {
            int id = nextID;
            nextID++;
            return id;
        }
        private void addVisibleDiscount(VisibleDiscount v)
        {
            int id = v.getId();
            string type = "Visible";
            int isPartOfComplex;
            if (v.getIsPartOfComplex())
            {
                isPartOfComplex = 1;
            }
            else
            {
                isPartOfComplex = 0;
            }
            string reliantType = "";
            string visibleType;
            int productId;
            int storeId = v.getStoreId();
            if (v.getProduct() == null)
            {
                visibleType = "StoreVisibleDiscount";
                productId = -1;
            }
            else
            {
                visibleType = "ProductVisibleDiscount";
                productId = v.getProduct().getProductID();
            }
            //not reliantdiscount
            int numOfProducts = -1;
            int totalAmount = -1;
            string sql = "INSERT INTO [dbo].[Discount] (id, type, isPartOfComplex, reliantType, visibleType, productId, storeId, numOfProducts, totalAmount)" +
                                " VALUES (@id, @type, @isPartOfComplex, @reliantType, @visibleType, @productId, @storeId, @numOfProducts, @totalAmount)";
            connection.Execute(sql, new
            {
                id,
                type,
                isPartOfComplex,
                reliantType,
                visibleType,
                productId,
                storeId,
                numOfProducts,
                totalAmount
            });
        }
        private void addReliantDiscount(ReliantDiscount r)
        {
            int id = r.getId();
            string type = "Reliant";
            bool isPartOfComplex = r.getIsPartOfComplex();
            string reliantType;
            string visibleType = "";
            int productId;
            int storeId = r.getStoreId();
            int numOfProducts;
            int totalAmount;

            if (r.getProduct() == null)
            {
                reliantType = "totalAmount";
                productId = -1;
                totalAmount = r.getTotalAmount();
                numOfProducts = -1;
            }
            else
            {
                reliantType = "sameProduct";
                productId = r.getProduct().getProductID();
                totalAmount = -1;
                numOfProducts = r.getMinNumOfProducts();
            }
            string sql = "INSERT INTO [dbo].[Discount] (id, type, isPartOfComplex, reliantType, visibleType, productId, numOfProducts, totalAmount)" +
                                " VALUES (@id, @type, @isPartOfComplex, @reliantType, @visibleType, @productId, @numOfProducts, @totalAmount)";
            connection.Execute(sql, new
            {
                id,
                type,
                isPartOfComplex,
                reliantType,
                visibleType,
                productId,
                storeId,
                numOfProducts,
                totalAmount
            });
        }

        public void setPercentage(int id, double percentage)
        {
            try
            {
                connection.Open();
                string sql = "UPDATE DiscountComponent SET percentage=@percentage" +
                                " WHERE id=@id";
                connection.Execute(sql, new
                {
                    percentage,
                    id
                });
                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
            }
        }
                
        public DiscountComponent getDiscountByID(int id)
        {
            foreach(DiscountComponent d in discounts)
            {
                if (d.getId() == id)
                    return d;
            }
            return null;
        }
        public Discount getStoreDiscount(int storeId)
        {
            try
            {
                connection.Open();
                var discountEntry = connection.Query<DiscountEntry>("SELECT * FROM [dbo].[Discount] WHERE storeId=@storeId", new { storeId = storeId });
                DiscountEntry d = (DiscountEntry)discountEntry;
                int discountId = d.getId();
                bool isPartOfComplex = false;
                if (d.getIsPartOfComplex() == 1)
                    isPartOfComplex = true;
                DiscountComponentEntry component = (DiscountComponentEntry)connection.Query<DiscountComponentEntry>("SELECT * FROM [dbo].[DiscountComponent] WHERE id=@id", new { id = discountId });
                if (d.getType() == "Visible")
                {

                    VisibleDiscount v = new VisibleDiscount(component.getId(), isPartOfComplex, component.getPercentage(), component.getDuration(), d.getVisibleType(), component.getStoreId());
                    connection.Close();
                    return v;

                }
                else
                {
                    ReliantDiscount r = null;
                    if (d.getReliantType() == "totalAmount")
                    {
                        r = new ReliantDiscount(component.getId(),isPartOfComplex, component.getPercentage(), component.getDuration(), d.getTotalAmount(), component.getStoreId());
                    }
                    connection.Close();
                    return r;
                }

            }
            catch (Exception)
            {
                connection.Close();
                return null;
            }
        }
    
        public LinkedList<DiscountComponent> getStoreDiscountsList(int storeId)
        {
            try
            {
                connection.Open();
                LinkedList<DiscountComponent> storeDiscounts = new LinkedList<DiscountComponent>();
                var c = connection.Query<DiscountComponentEntry>("SELECT * FROM [dbo].[DiscountComponent] WHERE storeId=@storeId AND type=@type", new { storeId = storeId, type = "Discount"}).ToList<DiscountComponentEntry>();
                List<DiscountComponentEntry> discountList = (List<DiscountComponentEntry>)c;
                foreach(DiscountComponentEntry d in discountList)
                {
                        var discountEntry = connection.Query<DiscountEntry>("SELECT * FROM [dbo].[Discounts] WHERE id=@id", new { id = d.getId() });
                        DiscountEntry de = (DiscountEntry)discountEntry;
                         
                        if (de.getProductId()!=-1)//productDiscount
                        {
                            Discount dis = getProductDiscount(d.getStoreId(), de.getProductId());
                            
                            if (dis.getIsPartOfComplex() == false) //add to store discounts only if it is not part of complex discount
                                storeDiscounts.AddFirst(dis);
                        }
                        else//StoreDiscount
                        {
                            Discount dis = getStoreDiscount(d.getStoreId());
                            if (dis.getIsPartOfComplex() == false)
                                storeDiscounts.AddFirst(dis);
                        }
                    }
                var compositeEntryList= connection.Query<DiscountComponentEntry>("SELECT * FROM [dbo].[DiscountComponent] WHERE storeId=@storeId AND type=@type", new { storeId = storeId, type = "Composite" }).ToList<DiscountComponentEntry>();
                List<DiscountComponentEntry> compositeEntryL = (List<DiscountComponentEntry>)compositeEntryList;
                int i = 0;
                while (compositeEntryL.Count != 0)
                {
                    DiscountComponentEntry di = compositeEntryL.ElementAt(i);
                    List<DiscountComponent> children = new List<DiscountComponent>();
                    var discountChildList = connection.Query<DiscountCompositeEntry>("SELECT * FROM [dbo].[DiscountComposite] WHERE id=@id", new { id = di.getId() }).ToList<DiscountCompositeEntry>();
                    List<DiscountCompositeEntry> de = (List<DiscountCompositeEntry>)discountChildList;
                    string type = de.ElementAt(0).getType();
                    bool childrenPulledFromDB = true;
                    foreach(DiscountCompositeEntry en in de)
                    {
                        if (getDiscountByID(en.getchildid()) == null)
                        {
                            childrenPulledFromDB = false;
                            break;
                        }
                    }
                    if (childrenPulledFromDB)
                    {
                        foreach (DiscountCompositeEntry en in de)
                        {
                            DiscountComponent disc = getDiscountByID(en.getId());
                            children.Add(disc);
                        }
                        DiscountComposite compos = new DiscountComposite(di.getId(), children, type, di.getPercentage(), di.getDuration(), di.getStoreId());
                        discounts.AddFirst(compos);
                        storeDiscounts.AddFirst(compos);
                        compositeEntryL.Remove(di);
                        
                    }
                    i = (i + 1) % compositeEntryL.Count;
               }
                return storeDiscounts;
                
            }
            catch (Exception)
            {
                connection.Close();
                return new LinkedList<DiscountComponent>();

            }
        }
        public Discount getProductDiscount(int storeId, int productId)
        {

            try
            {
                connection.Open();
                var discountEntry = connection.Query<DiscountEntry>("SELECT * FROM [dbo].[Discounts] WHERE storeId=@storeId AND productId=@productId", new { storeId = storeId, productId = productId });
                DiscountEntry d = (DiscountEntry)discountEntry;
                bool isPartOfComplex = false;
                if (d.getIsPartOfComplex() == 1)
                    isPartOfComplex = true;

                int discountId = d.getId();
                DiscountComponentEntry component = (DiscountComponentEntry)connection.Query<DiscountComponentEntry>("SELECT * FROM [dbo].[DiscountComponent] WHERE id=@id", new { id = discountId });
                if (d.getType() == "Visible")
                {
                    VisibleDiscount v = new VisibleDiscount(component.getId(), isPartOfComplex,component.getPercentage(), component.getDuration(), d.getVisibleType(), component.getStoreId());
                    Product p = DBProduct.getInstance().getProductByID(d.getProductId());
                    v.setProduct(p);
                    connection.Close();
                    return v;

                }
                else
                {
                    int productID = d.getProductId();
                    ReliantDiscount r = null;
                    if (d.getReliantType() == "sameProduct")
                    {
                        Product p = DBProduct.getInstance().getProductByID(productID);
                        bool isPartOfCompex = false;
                        if (d.getIsPartOfComplex() == 1)
                            isPartOfComplex = true;
                        r = new ReliantDiscount(component.getId(),isPartOfComplex, component.getPercentage(), component.getDuration(), d.getNumOfProducts(), p, component.getStoreId());
                    }
                    connection.Close();
                    return r;
                }

            }
            catch (Exception)
            {
                connection.Close();
                return null;
            }
        }
    }
}

        


