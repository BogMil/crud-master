using AutoMapper;
using CrudMaster.Repository;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;

namespace CrudMasterApi.Repositories
{
    public interface ICityRepository : IGenericRepository<City> { }
    public class CityRepository : GenericRepository<City, AccountingContext>, ICityRepository
    {
        public CityRepository(AccountingContext context) : base(context)
        {
        }
    }
}