using System;
using CrudMaster.Utils;
using Xunit;

namespace CrudMaster.UnitTests
{
    public class LambdaExpressionFromPathTest
    {
        public class TestClass
        {
            public int Int { get; set; }
            public string String{ get; set; }
            public NestedTestClass NestedTestClass{ get; set; }
        }

        public class NestedTestClass
        {
            public int Int { get; set; }
            public string String { get; set; }

        }


        [Theory]
        [InlineData("Int")]
        [InlineData("String")]
        [InlineData("NestedTestClass.Int")]
        public void Expression_ToStringIsAsExprected(string propertyPath)
        {
            var expressionFromPath = new LambdaExpressionFromPath<TestClass>(propertyPath);
            Assert.Equal(Constants.PARAMETER_EXPRESSION_NAME + "." + propertyPath, expressionFromPath.Expression.ToString());
        }

        [Theory]
        [InlineData("Int",typeof(int))]
        [InlineData("String", typeof(string))]
        [InlineData("NestedTestClass.Int", typeof(int))]
        public void ExpressionFuncReturnType_isAsExpected(string propertyPath,Type propertyType)
        {
            var expressionFromPath = new LambdaExpressionFromPath<TestClass>(propertyPath);
            Assert.Equal(propertyType, expressionFromPath.ExpressionFuncReturnType);
        }
    }
}
