using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CrudMaster.Service;
using CrudMasterApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CrudMasterApi.School
{
    public interface ISchoolService : IGenericService<SchoolQueryDto, SchoolCommandDto>
    {
        List<SchoolQueryDto> List();
    }

    public class SchoolService : GenericService<SchoolQueryDto,SchoolCommandDto,ISchoolRepository,Entities.School>,ISchoolService
	{
        private readonly AccountingContext _db;
        public SchoolService(ISchoolRepository repository, AccountingContext db) : base(repository)
        {
            _db = db;
        }
        public List<SchoolQueryDto> List()
        {
            var x=MappingService.GetMappingExpressionFromDestinationPropToSourceProp("CityName", typeof(SchoolQueryDto),
                typeof(Entities.School));
            var z = x.Compile();

            //ParameterExpression numParam = Expression.Parameter(typeof(Entities.School), "s");
            ParameterExpression numParam = x.Parameters[0];
            ConstantExpression value = Expression.Constant("Kovinski", typeof(string));
            BinaryExpression equation = Expression.Equal(x.Body, value);
            Expression<Func<Entities.School, bool>> lambda1 =
                Expression.Lambda<Func<Entities.School, bool>>(
                    equation,
                    new ParameterExpression[] { numParam });


            var l=_db.Schools.Include("City").Where(lambda1).ToList();
            return null;
        }

    }
}