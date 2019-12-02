using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using CustomerProductDBClasses;
using CustomerProductPropsClasses;
using System.Data;
using DBCommand = System.Data.SqlClient.SqlCommand;

namespace ProductPropsTests
{
    class ProductDBTests
    {
        ProductDB db;
        private string dataSource = "Data Source=1912019-C18067;Initial Catalog=MMABooksUpdated;Integrated Security=True";
        [SetUp]
        public void TestResetDatabase()
        {
            db = new ProductDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            ProductProps props = (ProductProps)db.Retrieve(1);
            Assert.AreEqual(props.productCode.Trim(), "A4CS");
            Assert.AreEqual(props.description, "Murach's ASP.NET 4 Web Programming with C# 2010");
            Assert.AreEqual(props.unitPrice, 56.50);
            Assert.AreEqual(props.onHandQuantity, 4637);
        }

        [Test]
        public void TestRetrieveInvalidKey()
        {
            Assert.Throws<Exception>(() => db.Retrieve(800));
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> props = (List<ProductProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(props.Count, 16);
        }

        [Test]
        public void TestCreate()
        {
            ProductProps props = new ProductProps();
            props.productCode = "A52V";
            props.description = "A Reslly Expensive Book";
            props.unitPrice = 1000000;
            props.onHandQuantity = 20;
            ProductProps newprops = (ProductProps)db.Create(props);
            props = (ProductProps)db.Retrieve(newprops.productID);
            Assert.AreEqual(newprops.productID, props.productID);
            Assert.AreEqual(newprops.productID, props.productID);
        }

        [Test]
        public void TestDelete()
        {
            ProductProps props = (ProductProps)db.Retrieve(16);
            db.Delete(props);
            List<ProductProps> propsList = (List<ProductProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(15, propsList.Count);
        }

        [Test]
        public void TestInvalidDelete()
        {
            ProductProps props = (ProductProps)db.Retrieve(16);
            props.ConcurrencyID += 1;
            Assert.Throws<Exception>(() => db.Delete(props));
        }

        [Test]
        public void TestUpdate()
        {
            ProductProps props = (ProductProps)db.Retrieve(16);
            Console.WriteLine(props.productCode.Trim());
            props.productCode = "AM27";
            db.Update(props);
            ProductProps updatedProps = (ProductProps)db.Retrieve(16);
            Assert.AreEqual(props.productCode.Trim(), updatedProps.productCode.Trim());
            Assert.AreEqual(2, props.ConcurrencyID);
            Console.WriteLine(props.productCode.Trim());
        }

        [Test]
        public void TestInvalidUpdate()
        {
            ProductProps props = (ProductProps)db.Retrieve(16);
            Console.WriteLine(props.productCode.Trim());
            props.productCode = "AM27";
            props.ConcurrencyID += 1;
            Assert.Throws<Exception>(() => db.Update(props));
        }
    }
}
