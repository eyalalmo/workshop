using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workshop192.Domain;


namespace workshop192.ServiceLayer.Tests
{
    [TestClass()]
    public class ExternalServiceTests
    {
        private UserService userService = UserService.getInstance();

        [TestInitialize()]
        public void Initialize()
        {
            userService.testSetup();
        }

        //1
        [TestMethod]
        public void initialTest()
        {

            try
            {
                bool pay = userService.handShakePay();
                bool deliver = userService.handShakeDeliver();
                if (pay == false || deliver == false)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(true, true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);


        }

        //7
        [TestMethod]
        public void payTest()
        {

            try
            {
                int result = userService.payToExternal("343434","11", "2020", "etay", "111", "23232323");
               
                if (result<10000 || result>100000)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(true, true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);


        }

        //7
        [TestMethod]
        public void cancelPayTest()
        {

            try
            {
                int result = userService.payToExternal("343434", "11", "2020", "etay", "111", "23232323");
                int resultOfCancel = userService.cancelPay(result);
             
                 if (resultOfCancel == 1)
                    Assert.AreEqual(true, true);
                 else
                    Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);


        }

        //8
        [TestMethod]
        public void deliverTest()
        {
            
           try
            {
                int result = userService.deliverToExternal("etay", "hamarganit", "ramt gan", "il", "111", "132");

                if (result < 10000 || result > 100000)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(true, true);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);


        }
        [TestMethod]
        public void cancelDeliveryTest()
        {
            try
            {
                int result = userService.deliverToExternal("etay", "hamarganit", "ramt gan", "il", "111", "132");
                int resultOfCancel = userService.cancelDelivery(result);

                if (resultOfCancel == 1)
                    Assert.AreEqual(true, true);
                else
                    Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);


        }

    }
}
