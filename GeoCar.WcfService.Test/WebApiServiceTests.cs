using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoCar.WcfService.Test
{
    [TestClass]
    public class WebApiServiceTests
    {
        [TestMethod]
        public void TestMarco()
        {
            var target = new WebApiService();

            Assert.AreEqual("polo", target.marco());
        }
    }
}
