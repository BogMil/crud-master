using CSR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrudMaster.UnitTests
{
    [TestClass]
    public class PropertyTypeExtractorTest
    {

        [TestMethod]
        [ExpectedException(typeof(MissingMemberException))]
        public void GetPropertyTypeAsString_MissingMemberExceptionWhenPassedUnknownParam()
        {
           PropertyTypeExtractor<TestClass>.GetPropertyTypeName("nestedClass!!!!!.nestedTestClassLvl2");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetPropertyTypeAsString_NullReferenceExceptionWhenLastAccessedPropertyUnknown()
        {
            PropertyTypeExtractor<TestClass>.GetPropertyTypeName("nestedClass.nestedTestClassLvl2!!!!!");
        }

        [TestMethod]
        public void GetPropertyTypeAsString_ReturnsProperTypeName()
        {
            var typeName = PropertyTypeExtractor<TestClass>.GetPropertyTypeName("nestedClass.nestedTestClassLvl2");

            Assert.AreEqual("NestedTestClassLvl2", typeName);
        }

        [TestMethod]
        public void GetPropertyInfo_ReturnsProperPropertyInfo()
        {
            var actualPI = PropertyTypeExtractor<TestClass>.GetPropertyInfo("nestedClass.nestedTestClassLvl2");

            var exprectedPI=typeof(NestedTestClass).GetProperty("nestedTestClassLvl2");

            Assert.AreEqual(exprectedPI, actualPI);
        }
    }
}
