using Accounting;
using Accounting.Models;
using Accounting.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrudMaster.UnitTests
{
    [TestClass]
    public class RepositoryTest
    {
        public InMemoryDbTester<AccountingContext> DbTester = new InMemoryDbTester<AccountingContext>();

        [TestMethod]
        public void GetPrimaryKey_ReturnsProperValue()
        {
            var city = new City()
            {
                Id = 1,
                Name = "Name",
                PostalCode = "postCode"
            };

            DbTester.UsingContext(context =>
            {
                var cityRepository = new CityRepository(context);
                var primiaryKey = cityRepository.GetPrimaryKeyValue(city);

                Assert.AreEqual(1, primiaryKey);
            });

        }

        [TestMethod]
        public void Delete()
        {
            DbTester.UsingContext(context =>
            {
                var cityRepository = new CityRepository(context);
                cityRepository.Delete(1);
                var x = context.Cities.ToList();
                Assert.AreEqual(4, x.Count);

            });

        }

        [TestMethod]
        public void DeleteAndReturn()
        {
            DbTester.UsingContext(context =>
            {
                var cityRepository = new CityRepository(context);
                var returnedId = cityRepository.DeleteAndReturn(1);
                var x = context.Cities.ToList();

                Assert.AreEqual(4, x.Count);
                Assert.AreEqual(1, returnedId);
            });

        }

        [TestMethod]
        public void Create()
        {
            var city = new City()
            {
                Id = 6,
                Name = "Name",
                PostalCode = "postCode",
                SchoolId = 1
            };

            DbTester.UsingContext(context =>
            {
                var cityRepository = new CityRepository(context);
                cityRepository.Create(city);
                var records = context.Cities.ToList();

                Assert.AreEqual(6, records.Count);
            });

        }

        [TestMethod]
        public void CreateAndReturn()
        {
            var city = new City()
            {
                Id = 6,
                Name = "Name",
                PostalCode = "postCode",
                SchoolId = 1
            };

            DbTester.UsingContext(context =>
            {
                var cityRepository = new CityRepository(context);
                var returnedCity = cityRepository.CreateAndReturn(city);
                var records = context.Cities.ToList();

                Assert.AreEqual(6, records.Count);
                Assert.AreEqual(returnedCity, city);
            });

        }


    }
}
