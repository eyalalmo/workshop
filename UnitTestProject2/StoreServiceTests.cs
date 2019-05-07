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
        int session1, session2, session3;
        int storeid;
        int productid;

        [TestInitialize()]
        public void TestInitialize()
        {
            try
            {
                UserService.getInstance().setup();

                session1 = userService.startSession();
                userService.register(session1, "anna", "banana"); //first owner
                userService.login(session1, "anna", "banana");

                session2 = userService.startSession();
                userService.register(session2, "dani1", "123");
                userService.login(session2, "dani1", "123");

                session3 = userService.startSession();
                userService.register(session3, "eva2", "123");
                userService.login(session3, "eva2", "123");

                storeid = storeService.addStore("myStore", "the best store ever", session1);
            }
            catch (Exception)
            {
                throw new ExecutionEngineException();
            }
        }

        //4.1.1
        [TestMethod()]
        public void addProductTest()
        {
            try
            {
                productid = storeService.addProduct("banana1", "green bananas", 100, 2, 6, storeid, session1);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            /*****
            * checked if product exist
            */
        }

        //4.1.2
        [TestMethod()]
        public void RemoveProductTest()
        {
            addProductTest();

            try
            {
                storeService.removeProduct(productid, session1);
            }
            catch (Exception)
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
                storeService.addToProductQuantity(productid, 3, session1);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }

        //4.5
        [TestMethod()]
        public void addMannagerByAnOwner1()
        {
            storeid = storeService.addStore("myStore", "the best store ever", session1);
            storeService.addManager(storeid, "dani1", true, true, true, session1);
            productid = storeService.addProduct("myProduct", "some category", 10, 0, 10, storeid, session2);

            try
            {
                storeService.addToProductQuantity(productid, 10, session2);
                storeService.decFromProductQuantity(productid, 10, session2);
                storeService.setProductDiscount(productid, 0, session2);
                try
                {
                    storeService.addManager(storeid, "yaniv", false, false, false, session1);
                    Assert.Fail();
                }
                catch (DoesntExistException)
                {
                    try
                    {
                        storeService.addToProductQuantity(productid, 10, session3);
                        Assert.Fail();

                    }
                    catch (RoleException)
                    {
                        try
                        {
                            storeService.decFromProductQuantity(productid, 10, session3);
                            Assert.Fail();
                        }
                        catch (RoleException)
                        {
                            try
                            {
                                storeService.setProductDiscount(productid, 0, session3);
                                Assert.Fail();
                            }
                            catch (RoleException)
                            {
                                Assert.IsTrue(true);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        //4.5
        [TestMethod()]
        public void addMannagerByAnOwner2()
        {
            try
            {
                storeService.addManager(storeid, "dani1", true, true, true, session1);
            }
            catch (StoreException)
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
                storeService.addManager(storeid, "dani1", true, true, true, session1);
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }

        }

        // 4.6
        [TestMethod()]
        public void removeManagerTest()
        {

            storeid = storeService.addStore("myStore", "the best store ever", session1);
            try
            {
                storeService.addManager(storeid, "dani1", true, true, true, session1);
                storeService.removeRole(storeid, "dani1", session1);

            }
            catch (StoreException)
            {
                Assert.Fail();
            }

        }

        // 4.6
        [TestMethod()]
        public void removeManagerTestFaild1()
        {
            storeid = storeService.addStore("myStore", "the best store ever", session1);
            try
            {
                storeService.addManager(storeid, "eva2", true, true, true, session1);
                try
                {
                    storeService.removeRole(storeid, "dani1", session3);
                    Assert.Fail();
                }
                catch (RoleException)
                {
                    Assert.IsTrue(true);
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        // 4.6
        [TestMethod()]
        public void removeManagerTestFaild2()
        {
            storeid = storeService.addStore("myStore", "the best store ever", session1);
            try
            {
                storeService.removeRole(storeid, "dani1", session1);
                Assert.Fail();
            }
            catch (RoleException)
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
                storeService.addOwner(storeid, "dani1", session1);
                /************/
                //  need to check if he is an owner now
                Assert.IsTrue(true);
            }
            catch (Exception)
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
                storeService.addOwner(storeid, "nouser", session1);
                Assert.Fail();
            }
            catch (DoesntExistException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
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
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        // 4.4
        [TestMethod()]
        public void removeOwnerSuccTest()
        {
            try
            {
                addOwnerByAnOwnerSuccTest();
                storeService.removeRole(storeid, "dani1", session1);
                /*************/
                // check that is totaly removed
                Assert.IsTrue(true);
            }
            catch (Exception)
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
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        // 4.4
        [TestMethod()]
        public void removeOwnerFailTest2()
        {
            try
            {
                addOwnerByAnOwnerSuccTest();
                storeService.addOwner(storeid, "eva2", session2);
                storeService.removeRole(storeid, "eva2", session1);
                Assert.Fail();
            }
            catch (RoleException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}