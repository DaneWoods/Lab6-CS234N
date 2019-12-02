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
            Customer c = new Customer(1, dataSource);
            c.Name = "Mario, Mario";
            c.Address = "Bowser's Kingdom";
            c.Save();

            c = new Customer(1, dataSource);
            Assert.AreEqual(c.ID, 1);
            Assert.AreEqual(c.Name, "Mario, Mario");
            Assert.AreEqual(c.Address, "Bowser's Kingdom");
        }

        [Test]
        public void TestDelete()
        {
            Customer c = new Customer(2, dataSource);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(2, dataSource));
        }

        // *** I added this
        [Test]
        public void TestGetList()
        {
            Customer c = new Customer(dataSource);
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(1, customers[0].ID);
            Assert.AreEqual("1108 Johanna Bay Drive", customers[0].Address);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "Mario, Mario";
            Assert.Throws<Exception>(() => c.Save());
            c.Address = "Mushroom Kingdom";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertyNameSet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Name = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        [Test]
        public void TestInvalidPropertyAddressSet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<ArgumentException>(() => c.Address = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        [Test]
        public void TestInvalidPropertyCitySet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<ArgumentException>(() => c.City = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        [Test]
        public void TestInvalidPropertyStateSet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<ArgumentException>(() => c.State = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        [Test]
        public void TestInvalidPropertyZipcodeSet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<ArgumentException>(() => c.ZipCode = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        // *** I added this
        [Test]
        public void TestConcurrencyIssue()
        {
            Customer e1 = new Customer(1, dataSource);
            Customer e2 = new Customer(1, dataSource);

            e1.Address = "Mushroom Kingdom";
            e1.Save();

            e2.Address = "Bowser's Kingdom";
            Assert.Throws<Exception>(() => e2.Save());
        }
    }
}
