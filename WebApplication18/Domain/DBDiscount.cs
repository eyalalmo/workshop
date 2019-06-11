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
        private Dictionary<int, DiscountComponent> discounts;
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
            discounts = new Dictionary<int, DiscountComponent>();
            nextID = 1;
        }

        public void init()
        {
            /*try
            {
                connection.Open();
                var d = connection.Query<Discount>("SELECT * FROM [dbo].[Discount]");

                if (d.Count() == 0)
                {
                    connection.Close();
                    return;
                }

                foreach (Discount discount in d)
                {
                    discounts.Add(discount.getId(), discount);
                    if (discount.getId() > nextID)
                        nextID = discount.getId();
                }

                connection.Close();
            }

            catch (Exception e)
            {
                connection.Close();
                throw e;
            }

            nextID++;*/
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
                    if(d is VisibleDiscount)
                    {
                        VisibleDiscount v = (VisibleDiscount)d;
                        addVisibleDiscount(v);
                    }
                    if(d is ReliantDiscount)
                    {
                        ReliantDiscount r = (ReliantDiscount)d;
                        addReliantDiscount(r);
                    }
                }
                if(d is DiscountComposite)
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
                    foreach(DiscountComponent child in composite.getChildren())
                    {
                        string sql2 = "INSERT INTO [dbo].[DiscountComposite] (id,childid,type)" +
                                " VALUES (@id, @childid,@type)";
                        string t;
                        if (child is DiscountComposite)
                            t = "Composite";
                        else
                            t = "Discount";

                        connection.Execute(sql, new
                        {
                            id = d.getId(),
                            childid = child.getId(),
                            type = t
                        });
                    }
                }
                /*
                string sql = "INSERT INTO [dbo].[Discount] (id, percentage, duration)" +
                                 " VALUES (@id, @percentage, @duration)";
                connection.Execute(sql, new
                {
                    id = d.getId(),
                    percentage = d.getPercentage(),
                    duration = d.getDuration()
                });
                */
                connection.Close();
                discounts.Add(d.getId(), d);

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
            
            LinkedList<Product> result = new LinkedList<Product>();
            try
            {
              //  connection.Open();
                //connection.Execute("DELETE FROM Discount WHERE id=@id ", new { id = d.getId() });
                discounts.Remove(d.getId());
                //connection.Close();
            }

            catch (Exception e)
            {
                //connection.Close();
                throw e;
            }
        }

        public static int getNextDiscountID()
        {
            int id = nextID;
            nextID++;
            return id;
        }
        public void addVisibleDiscount(VisibleDiscount v)
        {
            int id = v.getId();
            string type = "Visible";
            bool isPartOfComplex = v.getIsPartOfComplex();
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
        public void addReliantDiscount(ReliantDiscount r)
        {
            int id = r.getId();
            string type = "Reliant";
            bool isPartOfComplex = r.getIsPartOfComplex();
            string reliantType;
            string visibleType="";
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
        internal VisibleDiscount getDiscount(int discountID)
        {
            return (VisibleDiscount)discounts[discountID];
        }

        internal void update(Discount discount)
        {
            try
            {
               /* connection.Open();

                connection.Execute("UPDATE Discount SET " +
                                          "id = @id, " +
                                          "percentage = @percentage, " +
                                          "duration = @duration " +
                                          "WHERE id=@id",
                      new
                      {
                          id = discount.getId(),
                          percentage = discount.getPercentage(),
                          duration = discount.getDuration()
                      });

                connection.Close();*/
            }
            catch (Exception e)
            {
                //connection.Close();
                throw e;
            }
        }
        public DiscountComponent getDiscountByID(int id)
        {
            discounts.TryGetValue(id, out DiscountComponent value);
            return value;
        }
        public LinkedList<DiscountComponent> getStoreDiscounts(int storeId)
        {

        }
        public Discount getProductDiscount(int storeId, int productId)
        {
            var discountEntry = connection.Query<DiscountEntry>("SELECT * FROM [dbo].[Discounts] WHERE storeId=@storeId AND productId=@productId", new { storeId = storeId , productId = productId});
            DiscountEntry d = (DiscountEntry)discountEntry;
            int discountId = d.getId();
            DiscountComponentEntry component = (DiscountComponentEntry)connection.Query <DiscountComponentEntry> ("SELECT * FROM [dbo].[DiscountComponent] WHERE id=@id", new { id = discountId });
            if (d.getType() == "Visible")
            {
                VisibleDiscount v = new VisibleDiscount(component.getId(), component.getPercentage(), component.getDuration(), d.getVisibleType(), component.getStoreId());
                if (d.getProductId() != -1)
                {
                    Product p = DBProduct.getInstance().getProductByID(d.getProductId());
                    v.setProduct(p);
                }
                return v;

            }
            else
            {
                ReliantDiscount v = new ReliantDiscount(component.getId(), component.getPercentage(), component.getDuration(), d.getVisibleType(), component.getStoreId());
                if (d.getProductId() != -1)
                {
                    Product p = DBProduct.getInstance().getProductByID(d.getProductId());
                    v.setProduct(p);
                }
                return v;
            }

        }

        

    }
}

