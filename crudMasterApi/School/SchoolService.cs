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
            var cityNameMapping=MappingService.GetMappingExpressionFromDestinationPropToSourceProp("CityName", typeof(SchoolQueryDto),
                typeof(Entities.School));

            var nekiIntNameMapping = MappingService.GetMappingExpressionFromDestinationPropToSourceProp("NekiInt", typeof(SchoolQueryDto),
                typeof(Entities.School));

            ParameterExpression cityNameMapingParameter1 = cityNameMapping.Parameters[0];
            ParameterExpression cityNameMapingParameter = Expression.Parameter(typeof(Entities.School), "s");
            var x = cityNameMapingParameter == cityNameMapingParameter1;

            ConstantExpression value = Expression.Constant("Kovinski", typeof(string));
            BinaryExpression equation = Expression.Equal(cityNameMapping.Body, value);

            ConstantExpression intValue = Expression.Constant(1, typeof(int));

            var newExp = PredicateRewriter.Rewrite(nekiIntNameMapping, "s");

            BinaryExpression nekiIntEquation = Expression.Equal(newExp.Body, intValue);

            BinaryExpression res = Expression.And(nekiIntEquation,equation);


            BinaryExpression test = Expression.Equal(cityNameMapping.Body, value);
            Expression<Func<Entities.School, bool>> lambda1 =
                Expression.Lambda<Func<Entities.School, bool>>(
                    nekiIntEquation,
                    new ParameterExpression[] { cityNameMapingParameter });

            var l=_db.Schools.Include("City").Where(lambda1).ToList();
            return null;
        }

    }

    public class PredicateRewriter
    {
        public static LambdaExpression Rewrite(LambdaExpression exp, string newParamName)
        {
            var param = Expression.Parameter(exp.Parameters[0].Type, newParamName);
            var newExpression = new PredicateRewriterVisitor(param).Visit(exp);

            return (LambdaExpression)newExpression;
        }

        private class PredicateRewriterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _parameterExpression;

            public PredicateRewriterVisitor(ParameterExpression parameterExpression)
            {
                _parameterExpression = parameterExpression;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _parameterExpression;
            }
        }
    }
}