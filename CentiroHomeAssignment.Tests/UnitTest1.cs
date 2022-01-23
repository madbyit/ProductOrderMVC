using Microsoft.VisualStudio.TestTools.UnitTesting;
using CentiroHomeAssignment.Controllers;


namespace CentiroHomeAssignment.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new OrdersController();

            controller.StreamReadCSV();
            Assert.IsTrue(controller._orderlist.Count == 3);
        }
    }
}
