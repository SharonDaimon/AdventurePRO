// Author: Anastasia Mukalled
// Дата: 12.03.2016
// This file contains QPX class methods test

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdventurePRO.Tests;

namespace AdventurePRO.Model.APIs.ApiClients.Tests
{
    [TestClass()]
    public class QPXTests
    {
        [TestMethod()]
        public void RequestTicketsAsyncTest()
        {
            QPX qpx = new QPX(QPX.DEFAULT_API_KEY);
            
            var tickets = qpx.RequestTicketsAsync("MOW", "LED", TestUtils.From, TestUtils.To, 1, 0).Result;

            Assert.IsNotNull(tickets);
        }
    }
}