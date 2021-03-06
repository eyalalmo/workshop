﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        int session1, session2, session3, session4, session5;
        int storeid;
        int productid;

        [TestInitialize()]
        public void TestInitialize()
        {
            userService.testSetup();

            session1 = userService.startSession();
            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            session2 = userService.startSession();
            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            session3 = userService.startSession();
            userService.register(session3, "eva2", "123");
            userService.login(session3, "eva2", "123");

            session4 = userService.startSession();
            userService.register(session4, "u15", "123");
            userService.login(session4, "u15", "123");

            session5 = userService.startSession();
            userService.register(session5, "u16", "123");
            userService.login(session5, "u16", "123");

            storeid = storeService.addStore("myStore", "the best store ever", session1);

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
        }
        //4.3
        [TestMethod()]
        public void addOwnerByAnOwnerSuccTest()
        {
            try
            {
                storeService.addOwner(storeid, "dani1", session1);
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
                storeService.addOwner(storeid, "dani1", session1);
                Assert.Fail();
            }
            catch (RoleException)
            {
                Assert.IsTrue(true);
            }
        }

        //4.3
        [TestMethod()]
        public void addPendingOwnerTest1()
        {
            try
            {
                addOwnerByAnOwnerSuccTest();
                storeService.addOwner(storeid, "u15", session2);

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        //4.3
        [TestMethod()]
        public void addPendingOwnerTestFail1()
        {
            try
            {
                addOwnerByAnOwnerSuccTest();
                storeService.addManager(storeid, "u15", true, true, true, session2);
                storeService.addOwner(storeid, "u15", session2);
                Assert.Fail();
            }
            catch (RoleException)
            {
            }
        }

        //4.3
        [TestMethod()]
        public void addPendingOwnerTestFail2()
        {
            try
            {
                addOwnerByAnOwnerSuccTest();
                storeService.addOwner(storeid, "u15", session2);
                storeService.addOwner(storeid, "u15", session2);
                Assert.Fail();
            }
            catch (RoleException)
            {
            }
        }


        //4.3
        [TestMethod()]
        public void addTwoOwnersTest()
        {
            try
            {
                addOwnerByAnOwnerSuccTest();
                storeService.addOwner(storeid, "u15", session2);
                storeService.signContract(storeid, "u15", session1);

            }
            catch (RoleException)
            {
                Assert.Fail();
            }

            try
            {
                storeService.addOwner(storeid, "u16", session4); // checking that the owner was added successfuly

            }
            catch (RoleException)
            {
                Assert.Fail();
            }
        }

        //4.3
        [TestMethod()]
        public void declineContractTest1()
        {
            try
            {
                addOwnerByAnOwnerSuccTest();
                storeService.addOwner(storeid, "u15", session2);
                storeService.declineContract(storeid, "u15", session1);

            }
            catch (RoleException)
            {
                Assert.Fail();
            }

            try
            {
                storeService.addOwner(storeid, "u16", session4); // checking that the owner was added successfuly
                Assert.Fail();

            }
            catch (RoleException)
            {
                
            }
        }

        //4.3
        [TestMethod()]
        public void declineContractTest2()
        {
            try
            {
                addTwoOwnersTest();
                storeService.signContract(storeid, "u16", session1);
                storeService.declineContract(storeid, "u16", session4);

            }
            catch (RoleException)
            {
                Assert.Fail();
            }

            try
            {
                storeService.addOwner(storeid, "eva2", session5); // checking that the owner was added successfuly
                Assert.Fail();

            }
            catch (RoleException)
            {

            }
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
                //userService.setProductDiscount(productid, 0, session2);
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
                            Assert.IsTrue(true);
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

      
    }
}