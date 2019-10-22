using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSR;

namespace CrudMaster.UnitTests
{
    [TestClass]
    public class EfExtensionsTest
    {
        IQueryable<TestClass> testData = new List<TestClass>() {
                new TestClass{Name="C",Id=2, NestedProp = new NestedTestClass(){Id=3,Name="B"}},
                new TestClass{Name="B",Id=4, NestedProp = new NestedTestClass(){Id=4,Name="D"}},
                new TestClass{Name="D",Id=3, NestedProp = new NestedTestClass(){Id=2,Name="C"}},
                new TestClass{Name="A",Id=1, NestedProp = new NestedTestClass(){Id=5,Name="E"}},
                new TestClass{Name="E",Id=5, NestedProp = new NestedTestClass(){Id=1,Name="A"}}
            }.AsQueryable();

        List<string> expectedByStringAscResult = new List<string>() { "A", "B", "C", "D", "E" };
        List<int> expectedByIntAscResult = new List<int>() { 1, 2, 3, 4, 5 };
        List<string> expectedByNestedPropStringAscResult = new List<string>() { "E", "C", "D", "B", "A" };
        List<int> expectedByNestedPropIntAscResult = new List<int>() { 5, 3, 2, 4, 1 };

        [TestMethod]
        public void OrderByTest()
        {
            var actualByStringResult = testData.OrderBy("Name").Select(x => x.Name).ToList();
            var actualByIntResult = testData.OrderBy("Id").Select(x => x.Id).ToList();
            var actualByNestedPropStringResult = testData.OrderBy("NestedProp.Name").Select(x => x.Name).ToList();
            var actualByNestedPropIntResult = testData.OrderBy("NestedProp.Id").Select(x => x.Id).ToList();

            CollectionAssert.AreEquivalent(expectedByStringAscResult, actualByStringResult);
            CollectionAssert.AreEquivalent(expectedByIntAscResult, actualByIntResult);
            CollectionAssert.AreEquivalent(expectedByNestedPropStringAscResult, actualByNestedPropStringResult);
            CollectionAssert.AreEquivalent(expectedByNestedPropIntAscResult, actualByNestedPropIntResult);
        }

        [TestMethod]
        public void OrderByDescendingTest()
        {
            var actualByStringResult = testData.OrderByDescending("Name").Select(x => x.Name).ToList();
            var actualByIntResult = testData.OrderByDescending("Id").Select(x => x.Id).ToList();
            var actualByNestedPropStringResult = testData.OrderByDescending("NestedProp.Name").Select(x => x.Name).ToList();
            var actualByNestedPropIntResult = testData.OrderByDescending("NestedProp.Id").Select(x => x.Id).ToList();

            CollectionAssert.AreEquivalent(expectedByStringAscResult.Reverse<string>().ToList(), actualByStringResult);
            CollectionAssert.AreEquivalent(expectedByIntAscResult.Reverse<int>().ToList(), actualByIntResult);
            CollectionAssert.AreEquivalent(expectedByNestedPropStringAscResult.Reverse<string>().ToList(), actualByNestedPropStringResult);
            CollectionAssert.AreEquivalent(expectedByNestedPropIntAscResult.Reverse<int>().ToList(), actualByNestedPropIntResult);
        }

        public class TestClass
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public NestedTestClass NestedProp { get; set; }
        }

        public class NestedTestClass
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
    }
}
