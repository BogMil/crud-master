using CSR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CrudMaster.UnitTests
{
    [TestClass]
    public class ExpressionExtensionsTest
    {
        Expression<Func<TestObject, string>> expThatReturnsString = x => x.nestedProp.lvl2Nested.test;
        Expression<Func<TestObject, NestedTestObject>> expThatReturnsObject = x => x.nestedProp;
        Expression<Func<TestObject, int>> expThatReturnsInt = x => x.someInt;
        Expression<Func<TestObject, TestObject>> expThatReturnsSelf = x => x;
        Expression<Func<TestObject, string>> expThatReturnsConvert = x => x.someInt.ToString();

        [TestMethod]
        public void GetExpressionBodyAsStringTest()
        {
            Assert.AreEqual("nestedProp.lvl2Nested.test", expThatReturnsString.GetExpressionBodyAsString());
            Assert.AreEqual("nestedProp", expThatReturnsObject.GetExpressionBodyAsString());
            Assert.AreEqual("someInt", expThatReturnsInt.GetExpressionBodyAsString());
            Assert.AreEqual("", expThatReturnsSelf.GetExpressionBodyAsString());
            Assert.ThrowsException<Exception>(()=>expThatReturnsConvert.GetExpressionBodyAsString());
        }
    }
        
    public class TestObject
    {
        public int someInt { get; set; }
        public NestedTestObject nestedProp { get; set; }
    }

    public class NestedTestObject
    {
        public string someString { get; set; }
        public int someInt{ get; set; }
        public Level2NestedTestObject lvl2Nested { get; set; }
        
    }

    public class Level2NestedTestObject
    {
        public string test { get; set; }
    }
}
