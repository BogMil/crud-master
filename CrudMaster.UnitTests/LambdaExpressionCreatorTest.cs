using CSR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CrudMaster.UnitTests
{
    [TestClass]
    public class LambdaExpressionCreatorTest
    {
        [TestMethod]
        [ExpectedException(typeof(MissingFieldException))]
        public void LambdaExpressionCreator_ThrowsExceptionWhenPassedUnknownParameter()
        {
            var lambdaExpressionCreator = new LambdaExpressionCreator<TestClass>("a");
        }

        [TestMethod]
        public void LambdaExpressionCreator_CreatesProperEntityType()
        {
            var lambdaExpressionCreator = new LambdaExpressionCreator<TestClass>("nestedClass.nestedTestClassLvl2.someIntInLvl2");
            Assert.AreEqual(typeof(TestClass), lambdaExpressionCreator.EntityType);
        }

        [TestMethod]
        public void LambdaExpressionCreator_CreatesProperExpressionsFuncReturnType_ReferenceType()
        {
            var lambdaExpressionCreator = new LambdaExpressionCreator<TestClass>("nestedClass.nestedTestClassLvl2");
            Assert.AreEqual(typeof(NestedTestClassLvl2), lambdaExpressionCreator.ExpressionsFuncReturnType);
        }

        [TestMethod]
        public void LambdaExpressionCreator_CreatesProperExpressionsFuncReturnType_ValueType()
        {
            var lambdaExpressionCreator = new LambdaExpressionCreator<TestClass>("nestedClass.nestedTestClassLvl2.someIntInLvl2");
            Assert.AreEqual(typeof(int), lambdaExpressionCreator.ExpressionsFuncReturnType);
        }

        [TestMethod]
        public void LambdaExpressionCreator_CreatesProperExpression()
        {
            var lambdaExpressionCreator = new LambdaExpressionCreator<TestClass>("nestedClass.nestedTestClassLvl2.someIntInLvl2");

            Expression<Func<TestClass, int>> expectedExpression = x => x.nestedClass.nestedTestClassLvl2.someIntInLvl2;

            Assert.AreEqual(expectedExpression.Body.ToString(), lambdaExpressionCreator.LambdaExpression.Body.ToString());
        }
    }
}
