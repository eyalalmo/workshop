using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                        string sql = "INSERT INTO [dbo].[DiscountComponent] (id, percentage, duration, type, storeId, isPartOfComplex)" +
                                        " VALUES (@id,@percentage, @duration, @type, @storeId, @isPartOfComplex)";
                        int isPartOfComplex;
                        if (d.getIsPartOfComplex())
                            isPartOfComplex = 1;
                        else
                            isPartOfComplex = 0;
                        if (d is Discount)
                        {

                            connection.Execute(sql, new
                            {
                                id = d.getId(),
                                percentage = d.getPercentage(),
                                duration = d.getDuration(),
                                type = "Discount",
                                storeId = d.getStoreId(),
                                isPartOfComplex

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
                                storeId = d.getStoreId(),
                                isPartOfComplex

                            });
                            foreach (DiscountComponent child in composite.getChildren())
                            {
                                string sql2 = "INSERT INTO [dbo].[DiscountComposite] (id, childid, type)" +
                                        " VALUES (@id, @childid, @type)";

                                connection.Execute(sql2, new
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
            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
        }
        public void removeDiscount(DiscountComponent d)
        {
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                        connection.Execute("DELETE FROM DiscountComponent WHERE id=@id ", new { id = d.getId() });
                        if (d is Discount)
                        {
                            connection.Execute("DELETE FROM Discount WHERE id=@id ", new { id = d.getId() });
                        }
                        else
                        {
                            connection.Execute("DELETE FROM DiscountComposite WHERE id=@id ", new { id = d.getId() });
                            DiscountComposite composite = (DiscountComposite)d;
                            connection.Close();
                            foreach (DiscountComponent component in composite.getChildren())
                            {
                                removeDiscount(component);
                            }
                        
                    }
                    discounts.Remove(d);
                }
            }
            catch (Exception e)
            {
                if(connection.State == ConnectionState.Open)
                     connection.Close();
                throw e;
            }
        }

        public int getNextDiscountID()
        {
            try
            {
                lock (connection)
                {
                    connection.Open();
                    //SqlConnection connection = Connector.getInstance().getSQLConnection();
                    int idNum = connection.Query<int>("SELECT id FROM [dbo].[IDS] WHERE type=@type ", new { type = "discount" }).First();
                    int next = idNum + 1;
                    connection.Execute("UPDATE [dbo].[IDS] SET id = @id WHERE type = @type", new { id = next, type = "discount" });
                    connection.Close();
                    return idNum;
                }
            }
            catch (Exception)
            {
                connection.Close();
                return -1;
            }
        }
        private void addVisibleDiscount(VisibleDiscount v)
        {
            //SqlConnection connection = Connector.getInstance().getSQLConnection();
            int id = v.getId();
            string type = "Visible";
            string reliantType = "-1";
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
                string sql = "INSERT INTO [dbo].[Discount] (id, type, reliantType, visibleType, productId, storeId, numOfProducts, totalAmount)" +
                            " VALUES (@id, @type, @reliantType, @visibleType, @productId, @storeId, @numOfProducts, @totalAmount)";
                connection.Execute(sql, new
                {
                    id,
                    type,
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
            //SqlConnection connection = Connector.getInstance().getSQLConnection();
            int id = r.getId();
            string type = "Reliant";
            bool isPartOfComplex = r.getIsPartOfComplex();
            string reliantType;
            string visibleType = "-1";
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
                string sql = "INSERT INTO [dbo].[Discount] (id, type, reliantType, visibleType, productId, storeId, numOfProducts, totalAmount)" +
                            " VALUES (@id, @type, @reliantType, @visibleType, @productId, @storeId, @numOfProducts, @totalAmount)";
                connection.Execute(sql, new
                {
                    id,
                    type,
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
                // SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
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
            }
            catch (Exception)
            {
                connection.Close();
            }
        }
        public void setIsPartOfComplex(int id, bool b)
        {
            int isPartOfComplex;
            if (b)
                isPartOfComplex = 1;
            else
                isPartOfComplex = 0;
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    string sql = "UPDATE DiscountComponent SET isPartOfComplex=@isPartOfComplex" +
                            " WHERE id=@id";
                    connection.Execute(sql, new
                    {
                        isPartOfComplex,
                        id
                    });
                }
                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
            }
        }

        public DiscountComponent getDiscountByID(int id)
        {
            foreach (DiscountComponent d in discounts)
            {
                if (d.getId() == id)
                    return d;
            }
            return null;
        }
        public Discount getStoreDiscount(int id, int storeId)
        {
            //SqlConnection connection = Connector.getInstance().getSQLConnection();
                var discountEntry = connection.Query<DiscountEntry>("SELECT * FROM [dbo].[Discount] WHERE id=@id AND storeId=@storeId", new { id, storeId = storeId }).First();

                DiscountEntry d = (DiscountEntry)discountEntry;
                int discountId = d.getId();


                DiscountComponentEntry component = (DiscountComponentEntry)connection.Query<DiscountComponentEntry>("SELECT * FROM [dbo].[DiscountComponent] WHERE id=@id", new { id = discountId }).First();
                connection.Close();
                bool isPartOfComplex = false;
                if (component.getIsPartOfComplex() == 1)
                    isPartOfComplex = true;
                if (d.getType() == "Visible")
                {

                    VisibleDiscount v = new VisibleDiscount(component.getId(), isPartOfComplex, component.getPercentage(), component.getDuration(), d.getVisibleType(), component.getStoreId());
                    return v;

                }
                else
                {
                    ReliantDiscount r = null;
                    if (d.getReliantType() == "totalAmount")
                    {
                        r = new ReliantDiscount(component.getId(), isPartOfComplex, component.getPercentage(), component.getDuration(), d.getTotalAmount(), component.getStoreId());
                    }
                    return r;
                }
        }


        public LinkedList<DiscountComponent> getStoreDiscountsList(int storeId)
        {
            try
            {
               
                    //SqlConnection connection = Connector.getInstance().getSQLConnection();
                    LinkedList<DiscountComponent> storeDiscounts = new LinkedList<DiscountComponent>();
                lock (connection)
                {
                    connection.Open();
                    var c = connection.Query<DiscountComponentEntry>("SELECT * FROM [dbo].[DiscountComponent] WHERE storeId=@storeId AND type=@type", new { storeId = storeId, type = "Discount" }).ToList<DiscountComponentEntry>();
                    connection.Close();
                    List<DiscountComponentEntry> discountList = (List<DiscountComponentEntry>)c;
                    foreach (DiscountComponentEntry d in discountList)
                    {

                        connection.Open();
                        var discountEntry = connection.Query<DiscountEntry>("SELECT * FROM [dbo].[Discount] WHERE id=@id", new { id = d.getId() }).First();
                        connection.Close();

                        DiscountEntry de = (DiscountEntry)discountEntry;


                        if (de.getProductId() != -1)//productDiscount
                        {
                            Discount dis = getProductDiscount(d.getStoreId(), de.getProductId());

                            if (dis.getIsPartOfComplex() == false) //add to store discounts only if it is not part of complex discount
                                storeDiscounts.AddFirst(dis);
                        }
                        else//StoreDiscount
                        {
                            Discount dis = getStoreDiscount(d.getId(), d.getStoreId());
                            if (dis.getIsPartOfComplex() == false)
                                storeDiscounts.AddFirst(dis);
                            discounts.AddFirst(dis);
                        }


                    }
                    connection.Open();
                    var compositeEntryList = connection.Query<DiscountComponentEntry>("SELECT * FROM [dbo].[DiscountComponent] WHERE storeId=@storeId AND type=@type", new { storeId = storeId, type = "Composite" }).ToList<DiscountComponentEntry>();
                    List<DiscountComponentEntry> compositeEntryL = (List<DiscountComponentEntry>)compositeEntryList;
                    int i = 0;
                    while (compositeEntryL.Count != 0)
                    {
                        DiscountComponentEntry di = compositeEntryL.ElementAt(i);
                        List<DiscountComponent> children = new List<DiscountComponent>();
                        var discountChildList = connection.Query<DiscountCompositeEntry>("SELECT * FROM [dbo].[DiscountComposite] WHERE id=@id", new { id = di.getId() }).ToList<DiscountCompositeEntry>();
                        connection.Close();
                        List<DiscountCompositeEntry> de = (List<DiscountCompositeEntry>)discountChildList;
                        string type = de.ElementAt(0).getType();
                        bool childrenPulledFromDB = true;
                        foreach (DiscountCompositeEntry en in de)
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
                                DiscountComponent disc = getDiscountByID(en.getchildid());
                                children.Add(disc);
                            }
                            bool isPartOfComplex = false;
                            if (di.getIsPartOfComplex() == 1)
                                isPartOfComplex = true;
                            DiscountComposite compos = new DiscountComposite(di.getId(), children, type, di.getPercentage(), di.getDuration(), di.getStoreId(), isPartOfComplex);
                            discounts.AddFirst(compos);
                            storeDiscounts.AddFirst(compos);
                            compositeEntryL.Remove(di);

                        }
                        if (compositeEntryL.Count != 0)
                            i = (i + 1) % compositeEntryL.Count;
                    }
                    connection.Close();
                    return storeDiscounts;
                }
            }
            catch (Exception)
            {
                if(connection.State != ConnectionState.Closed)
                    connection.Close();
                return new LinkedList<DiscountComponent>();
            }
        }
        public Discount getProductDiscount(int storeId, int productId)
        {
            try
            {
                //SqlConnection connection = Connector.getInstance().getSQLConnection();
                lock (connection)
                {
                    connection.Open();
                    var discountEntry = connection.Query<DiscountEntry>("SELECT * FROM [dbo].[Discount] WHERE storeId=@storeId AND productId=@productId", new { storeId = storeId, productId = productId }).First();
                    connection.Close();
                    DiscountEntry d = (DiscountEntry)discountEntry;
                    DiscountComponent dis = getDiscountByID(d.getId());
                    if (dis != null)
                        return (Discount)dis;
                    int discountId = d.getId();
                    DiscountComponentEntry component = (DiscountComponentEntry)connection.Query<DiscountComponentEntry>("SELECT * FROM [dbo].[DiscountComponent] WHERE id=@id", new { id = discountId }).First();
                    bool isPartOfComplex = false;
                    if (component.getIsPartOfComplex() == 1)
                        isPartOfComplex = true;
                    if (d.getType() == "Visible")
                    {
                        VisibleDiscount v = new VisibleDiscount(component.getId(), isPartOfComplex, component.getPercentage(), component.getDuration(), d.getVisibleType(), component.getStoreId());
                        Product p = DBProduct.getInstance().getProductByID(d.getProductId());
                        v.setProduct(p);
                        discounts.AddFirst(v);
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
                            r = new ReliantDiscount(component.getId(), isPartOfComplex, component.getPercentage(), component.getDuration(), d.getNumOfProducts(), p, component.getStoreId());
                            discounts.AddFirst(r);
                        }
                        return r;
                    }
                }
            }
            catch (Exception)
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                return null;
            }
        }
    }
}

        


