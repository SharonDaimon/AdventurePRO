// Author: Anastasia Mukalled
// Дата: 12.03.2016
// This file contains Fixer class methods test

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventurePRO.Model.APIs.ApiClients.Tests
{
    [TestClass()]
    public class FixerTests
    {
        [TestMethod()]
        public void GetRatesAsyncTest()
        {
            Fixer fixer = new Fixer();

            var rates = fixer.GetRatesAsync().Result;

            Assert.IsNotNull(rates);
            Assert.IsTrue(rates.Count > 1);
            Assert.IsTrue(rates.ContainsKey("RUB"));
        }
    }
}