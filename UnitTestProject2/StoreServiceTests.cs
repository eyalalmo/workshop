using Microsoft.VisualStudio.TestTools.UnitTesting;
using workshop192.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;

namespace workshop192.ServiceLayer.Tests
{
    [TestClass()]
    public class StoreServiceTests
    {
        private StoreService storeService = StoreService.getInstance();
        private UserService userService = UserService.getInstance();
        Session session1, session2, session3;
        Store store;
        Product product;

        [TestInitialize()]
        public void TestInitialize()
        {
            session1 = userService.startSession();
            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            session2 = userService.startSession();
            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            session3 = userService.startSession();
            userService.register(session3, "eva2", "123");
            userService.login(session3, "eva2", "123");

            store = storeService.addStore("myStore", "the best store ever", session1);
        }

        //4.1.1
        [TestMethod()]
        public void addProductTest()
        {
            store = userService.createStore(session1, "Bananas", "all types of bananas");
            try
            {
                product = storeService.addProduct("banana1", "green bananas", 100, 2, 6, store, session1);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

            /*************
            * checked if product exist
            ***/
        }

        //4.1.2
        [TestMethod()]
        public void RemoveProductTest()
        {
            addProductTest();

            try
            {
                storeService.removeProduct(product, session1);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
            //Assert.IsFalse(storeService.isProductExist(store, product));
            Assert.IsTrue(true);

        }






        //4.1.2
        [TestMethod()]
        public void RemoveProductTest1()
        {
            addProductTest();

            try
            {
                storeService.addToProductQuantity(product, 3, session1);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }


        //4.5
        [TestMethod()]
        public void addMannagerByAnOwner1()
        {
            store = storeService.addStore("myStore", "the best store ever", session1);
            storeService.addManager(store, "dani", true, true, true, session1);
            product = storeService.addProduct("myProduct", "some category", 10, 0, 10, store, session2);

            try
            {
                storeService.addToProductQuantity(product, 10, session2);
                storeService.decFromProductQuantity(product, 10, session2);
                storeService.setProductDiscount(product, null, session2);
                try
                {
                    storeService.addManager(store, "yaniv", false, false, false, session1);
                    Assert.Fail();
                }
                catch (StoreException e0)
                {
                    try
                    {
                        storeService.addToProductQuantity(product, 10, session3);
                        Assert.Fail();

                    }
                    catch (StoreException e1)
                    {
                        try
                        {
                            storeService.decFromProductQuantity(product, 10, session3);
                            Assert.Fail();
                        }
                        catch (StoreException e2)
                        {
                            try
                            {
                                storeService.setProductDiscount(product, null, session3);
                                Assert.Fail();
                            }
                            catch (StoreException e3)
                            {
                                Assert.IsTrue(true);
                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        //4.5
        [TestMethod()]
        public void addMannagerByAnOwner2()
        {
            store = storeService.addStore("myStore", "the best store ever", session1);
            try
            {
                storeService.addManager(store, "dani", true, true, true, session1);
            }
            catch (StoreException e)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }


        //4.5
        [TestMethod()]
        public void addMannagerByAnOwner3()
        {
            addMannagerByAnOwner2();
            try
            {
                storeService.addManager(store, "dani", true, true, true, session1);
                Assert.Fail();
            }
            catch (StoreException e)
            {
                Assert.IsTrue(true);
            }

        }

        // 4.6
        [TestMethod()]
        public void removeManagerTest()
        {

            store = storeService.addStore("myStore", "the best store ever", session1);
            try
            {
                storeService.addManager(store, "dani1", true, true, true, session1);
                storeService.removeRole(store, "dani1", session1);

            }
            catch (StoreException e)
            {
                Assert.Fail();
            }

        }

        // 4.6
        [TestMethod()]
        public void removeManagerTestFaild1()
        {

            store = storeService.addStore("myStore", "the best store ever", session1);
            try
            {
                storeService.addOwner(store, "dani2", session1);
                storeService.addManager(store, "dani1", true, true, true, session1);
                try
                {
                    storeService.removeRole(store, "dani1", session3);
                    Assert.Fail();
                }
                catch (StoreException e2)
                {
                    Assert.IsTrue(true);
                }
            }
            catch (StoreException e1)
            {
                Assert.Fail();
            }
        }


        // 4.6
        [TestMethod()]
        public void removeManagerTestFaild2()
        {
            store = storeService.addStore("myStore", "the best store ever", session1);
            try
            {
                storeService.removeRole(store, "dani1", session1);
                Assert.Fail();
            }
            catch (StoreException e)
            {
                Assert.IsTrue(true);
            }
        }








        //4.3
        [TestMethod()]
        public void addOwnerByAnOwnerSuccTest()
        {
            try
            {
                storeService.addOwner(store, "dani1", session1);
                /********************************/
                //  need to check if he is an owner now
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        //4.3
        [TestMethod()]
        public void addOwnerByAnOwnerFailTest()
        {
            try
            {
                storeService.addOwner(store, "nouser", session1);
                Assert.Fail();
            }
            catch (UserException re)
            {
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        //4.3
        [TestMethod()]
        public void addOwnerByAnOwnerFailTest2()
        {
            try
            {
                addOwnerByAnOwnerSuccTest();
                addOwnerByAnOwnerSuccTest();
                Assert.Fail();
            }
            catch (RoleException re)
            {
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        // 4.4
        [TestMethod()]
        public void removeOwnerSuccTest()
        {
            try
            {
                addOwnerByAnOwnerSuccTest();
                storeService.removeRole(store, "dani1", session1);
                /***********************************/
                // check that is totaly removed
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        // 4.4
        [TestMethod()]
        public void removeOwnerFailTest()
        {
            try
            {
                removeOwnerSuccTest();
                Assert.Fail();
            }
            catch (RoleException re)
            {
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        // 4.4
        [TestMethod()]
        public void removeOwnerFailTest2()
        {
            try
            {
                addOwnerByAnOwnerSuccTest();
                storeService.addOwner(store, "eva2", session2);
                storeService.removeRole(store, "eva2", session1);
                Assert.Fail();
            }
            catch (RoleException re)
            {
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
    }
}