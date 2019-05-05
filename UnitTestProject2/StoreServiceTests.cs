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
        int session1, session2, session3, store;


        [TestInitialize()]
        public void TestInitialize()
        {
            session1 = userService.createSession();
            userService.register(session1, "anna", "banana"); //first owner
            userService.login(session1, "anna", "banana");

            session2 = userService.createSession();
            userService.register(session2, "dani1", "123");
            userService.login(session2, "dani1", "123");

            session3 = userService.createSession();
            userService.register(session3, "eva2", "123");
            userService.login(session3, "eva2", "123");

            store = storeService.addStore("myStore", "the best store ever", session1);
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
    }s
}