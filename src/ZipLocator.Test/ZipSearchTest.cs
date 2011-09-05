using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading;
using MindHarbor.ZipLocator;
using NUnit.Framework;

namespace ZipLocator.Test
{
    [TestFixture]
    public class ZipSearchTest
    {

        private static ZipSearch searcher = InitializeSearch();

        private static ZipSearch InitializeSearch()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnecString"].ConnectionString;
            ZipSearch zp = new ZipSearch("tbl_UNSpaMembers_detail", "zip", connectionString);
            zp.AsynchronizedInitialization();
            return zp;
        }

        public void Search()
        {

            Thread.Sleep(6000);


            if (!searcher.Initialized)
                Console.Write("Sorry,it is initializing,");
            else
            {
                DataTable dt=searcher.Search("18702",20);
                Assert.AreEqual(3,dt.Rows.Count);
            }
                
            
        }



        [Test]
        public void TestInitializeSearch()
        {
            Search();
        }

        [Test]
        public void TestSearch()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnecString"].ConnectionString;
            ZipSearch zs = new ZipSearch("Mindharbor_ZipLocator", connectionString);
            DataTable dt = zs.Search("10337", 20);
            Assert.AreEqual(3, dt.Rows.Count);
        }
    }
}
