using System;
using System.Linq.Expressions;
using System.Reflection;
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

        [Theory]
        [InlineData("Int")]
        [InlineData("String")]
        [InlineData("NestedTestClass.Int")]
        public void LambdaExpression_isAsExpected(string propertyPath)
        {
            var expressionFromPath = new LambdaExpressionFromPath<TestClass>(propertyPath).LambdaExpression.Body.ToString();
            var expected = Constants.PARAMETER_EXPRESSION_NAME + "." + propertyPath;
            Assert.Equal(expected, expressionFromPath);
        }

        [Theory]
        [InlineData("Int")]
        [InlineData("String")]
        [InlineData("NestedTestClass.Int")]
        public void ParameterExpression_isAsExpected(string propertyPath)
        {
            var actual  = new LambdaExpressionFromPath<TestClass>(propertyPath).ParameterExpression;
            var expected=Expression.Parameter(typeof(NestedTestClass), Constants.PARAMETER_EXPRESSION_NAME);
            Assert.Equal(expected.NodeType, actual.NodeType);
            Assert.Equal(expected.ToString(), actual.ToString());
        }
        [Theory]
        [InlineData("Int")]
        [InlineData("String")]
        [InlineData("NestedTestClass.Int")]
        public void FullPropertyPath_isAsProvided(string propertyPath)
        {
            var actual = new LambdaExpressionFromPath<TestClass>(propertyPath);
            Assert.Equal(propertyPath, actual.FullPropertyPath);
        }

        [Fact]
        public void HasPropertyLambdaExpression()
        {
            var fieldInfo = typeof(LambdaExpressionFromPath<>).GetField("LambdaExpression");
            Assert.NotNull(fieldInfo);
        }
    }
}
