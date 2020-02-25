using System.Linq;
using AutoMapper;
using CrudMaster;
using CrudMaster.Repository;
using CrudMaster.Sorter;
using CrudMasterApi.Entities;
using Microsoft.AspNetCore.SignalR;
using X.PagedList;

namespace CrudMasterApi.School
{
    public interface ISchoolRepository : IGenericRepository<Entities.School> { }

    public class SchoolRepository : GenericRepository<Entities.School, AccountingContext>, ISchoolRepository
	{
        public SchoolRepository(AccountingContext context) : base(context) { }
    }
}