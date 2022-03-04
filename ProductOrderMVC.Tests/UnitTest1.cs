using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductOrderWebApp.Controllers;

using System;

namespace ProductOrderWebApp.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new OrdersController();

            controller.GetAllData();
            Assert.IsTrue(controller._orderlist.Count == 14);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var controller = new OrdersController();

            controller.GetByOrderNumber("17642");
            Assert.IsTrue(controller._orderlist.Count == 5);
        }
    }
}
