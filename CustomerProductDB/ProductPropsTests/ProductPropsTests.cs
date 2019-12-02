using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CustomerProductPropsClasses;

namespace ProductPropsTests
{
    public class ProductPropsTests
    {
        ProductProps props;

        [SetUp]
        public void SetUp()
        {
            props = new ProductProps();
            props.productID = 1;
            props.productCode = "Apples";
            props.description = "Tasty";
            props.unitPrice = 5.99m;
            props.onHandQuantity = 25000;
            props.ConcurrencyID = 4;
        }

        [Test]
        public void GetStateTest()
        {
            string output = props.GetState();
            Console.WriteLine(output);
        }

        [Test]
        public void SetStateTest()
        {
            ProductProps props2 = new ProductProps();
            props2.SetState(props.GetState());
            Assert.AreEqual(props.productID, props2.productID);
            Assert.AreEqual(props.productCode, props2.productCode);
            Assert.AreEqual(props.description, props2.description);
            Assert.AreEqual(props.unitPrice, props2.unitPrice);
            Assert.AreEqual(props.onHandQuantity, props2.onHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, props2.ConcurrencyID);
            string output1 = props.GetState();
            string output2 = props2.GetState();
            Console.WriteLine(output1);
            Console.WriteLine(output2);
        }

        [Test]
        public void CloneTest()
        {
            ProductProps props2 = new ProductProps();
            props2 = (ProductProps)props.Clone();
            Assert.AreEqual(props.productID, props2.productID);
            Assert.AreEqual(props.productCode, props2.productCode);
            Assert.AreEqual(props.description, props2.description);
            Assert.AreEqual(props.unitPrice, props2.unitPrice);
            Assert.AreEqual(props.onHandQuantity, props2.onHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, props2.ConcurrencyID);
        }
    }
}
