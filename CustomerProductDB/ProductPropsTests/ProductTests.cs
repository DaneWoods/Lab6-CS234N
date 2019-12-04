using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CustomerProductPropsClasses;
using CustomerProductClasses;

using CustomerProductDBClasses;
using ToolsCSharp;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace ProductPropsTests
{
    [TestFixture]
    class ProductTests
    {
        private string dataSource = "Data Source=1912019-C18067;Initial Catalog=MMABooksUpdated;Integrated Security=True";
        // *** I added changed this.  It calls the stored procedure to reset the db
        [SetUp]
        public void TestResetDatabase()
        {
            ProductDB db = new ProductDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestPropsRetrieve()
        {
            ProductDB db = new ProductDB(dataSource);
            ProductProps props = (ProductProps)db.Retrieve(2);
            Assert.AreEqual(props.productID, 2);
            Console.WriteLine(props.GetState());
        }

        [Test]
        public void TestNewCustomerConstructor()
        {
            // not in Data Store - no id
            Product p = new Product(dataSource);
            Console.WriteLine(p.ToString());
            Assert.Greater(p.ToString().Length, 1);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Product p = new Product(1, dataSource);
            Assert.AreEqual(p.ProductID, 1);
            Assert.AreEqual(p.ProductCode.Trim(), "A4CS");
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Product p = new Product(dataSource);
            p.ProductCode = "N823K";
            p.Description = "Wow";
            p.UnitPrice = 25.99m;
            p.OnHandQuantity = 2;
            p.Save();
            Assert.AreEqual(17, p.ProductID);
        }

        [Test]
        public void TestUpdate()
        {
            Product p = new Product(1, dataSource);
            p.ProductCode = "GJ5K";
            p.Description = "The Infinity Stones";
            p.Save();

            p = new Product(1, dataSource);
            Assert.AreEqual(p.ProductID, 1);
            Assert.AreEqual(p.ProductCode.Trim(), "GJ5K");
            Assert.AreEqual(p.Description, "The Infinity Stones");
        }

        [Test]
        public void TestDelete()
        {
            Product p = new Product(2, dataSource);
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(2, dataSource));
        }

        // *** I added this
        [Test]
        public void TestGetList()
        {
            Product p = new Product(dataSource);
            List<Product> products = (List<Product>)p.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual(1, products[0].ProductID);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", products[0].Description);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
            p.ProductCode = "GK38";
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "Piece Coal";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestInvalidPropertyNameSet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        [Test]
        public void TestInvalidPropertyAddressSet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentException>(() => p.Description = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        [Test]
        public void TestInvalidPropertyCitySet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentException>(() => p.UnitPrice = -1);
        }

        [Test]
        public void TestInvalidPropertyStateSet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentException>(() => p.OnHandQuantity = -1);
        }

        // *** I added this
        [Test]
        public void TestConcurrencyIssue()
        {
            Product e1 = new Product(1, dataSource);
            Product e2 = new Product(1, dataSource);

            e1.UnitPrice = 20;
            e1.Save();

            e2.UnitPrice = 19;
            Assert.Throws<Exception>(() => e2.Save());
        }
    }
}
