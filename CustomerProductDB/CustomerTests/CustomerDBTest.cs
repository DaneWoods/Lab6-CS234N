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

namespace CustomerTests
{
    [TestFixture]
    public class CustomerDBTest
    {
        CustomerDB db;
        private string dataSource = "Data Source=1912019-C18067;Initial Catalog=MMABooksUpdated;Integrated Security=True";
        [SetUp]
        public void TestResetDatabase()
        {
            db = new CustomerDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            CustomerProps props = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual(props.name, "Molunguri, A");
            Assert.AreEqual(props.address, "1108 Johanna Bay Drive");
            Assert.AreEqual(props.city, "Birmingham");
            Assert.AreEqual(props.state, "AL");
            Assert.AreEqual(props.zipcode, "35216-6909");
        }

        [Test]
        public void TestRetrieveInvalidKey()
        {
            Assert.Throws<Exception>(() => db.Retrieve(800));
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<CustomerProps> props = (List<CustomerProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(props.Count, 696);
        }

        [Test]
        public void TestCreate()
        {
            CustomerProps props = new CustomerProps();
            props.name = "Blingy, Lingy";
            props.address = "1234 Earth";
            props.city = "Eugene";
            props.state = "OR";
            props.zipcode = "97402";
            CustomerProps newprops = (CustomerProps)db.Create(props);
            props = (CustomerProps)db.Retrieve(newprops.ID);
            Assert.AreEqual(newprops.ID, props.ID);
            Assert.AreEqual(newprops.name, props.name);
        }

        [Test]
        public void TestDelete()
        {
            CustomerProps props = (CustomerProps)db.Retrieve(696);
            db.Delete(props);
            List<CustomerProps> propsList = (List<CustomerProps>)db.RetrieveAll(db.GetType());
            Assert.AreEqual(695, propsList.Count);
        }

        [Test]
        public void TestUpdate()
        {
            CustomerProps props = (CustomerProps)db.Retrieve(696);
            props.name = "Jeff Mgee";
            db.Update(props);
            Assert.AreEqual(props.name, )
        }
    }
}
