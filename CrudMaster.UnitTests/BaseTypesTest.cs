using System;
using System.Collections.Generic;
using System.Text;
using CrudMaster.Utils;
using Xunit;

namespace CrudMaster.UnitTests
{

    public class BaseTypesTest
    {
        [Theory]
        [InlineData(typeof(Int32))]
        [InlineData(typeof(Int64))]
        [InlineData(typeof(Decimal))]
        [InlineData(typeof(String))]
        [InlineData(typeof(long))]
        [InlineData(typeof(int))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(double))]
        [InlineData(typeof(float))]
        [InlineData(typeof(DateTime))]
        public void AllProvidedTypesAreBaseTypes(Type type)
        {
            var isBaseType = BaseTypes.IsBaseType(type);
            Assert.True(isBaseType);
        }
    }
}
