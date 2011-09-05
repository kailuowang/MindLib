using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using MindHarbor.ZipLocator;
using NUnit.Framework;

namespace ZipLocator.Test
{
    [TestFixture]
    public class SpecialTests
    {
        [Test]
        public void TestInitialData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnecString"].ConnectionString;
            new ZipSearch("SpaMember","ZipCode",connectionString).EnsureDBElements();
        }
    }
}
