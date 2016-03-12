// Author: Anastasia Mukalled
// Дата: 12.03.2016
// This file contains test utils

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventurePRO.Tests
{
    [TestClass]
    public class TestUtils
    {
        public static DateTime From
        {
            get
            {
                return DateTime.Now.AddDays(3);
            }
        }

        public static DateTime To
        {
            get
            {
                return DateTime.Now.AddDays(5);
            }
        }

    }
}
