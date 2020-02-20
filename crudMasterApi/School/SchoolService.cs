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
        List<Entities.School> List();
    }

    public class SchoolService : GenericService<SchoolQueryDto, SchoolCommandDto, ISchoolRepository, Entities.School>, ISchoolService
    {
        private readonly AccountingContext _db;
        public SchoolService(ISchoolRepository repository, AccountingContext db) : base(repository)
        {
            _db = db;
        }
        public List<Entities.School> List()
        {
            var cityNameMapping = MappingService.GetMappingExpressionFromDestinationPropToSourceProp("CityName", typeof(SchoolQueryDto),
                typeof(Entities.School));

            var nekiIntNameMapping = MappingService.GetMappingExpressionFromDestinationPropToSourceProp("NekiInt", typeof(SchoolQueryDto),
                typeof(Entities.School));

            ParameterExpression parameterExpression = Expression.Parameter(typeof(Entities.School), "s");

            ConstantExpression value = Expression.Constant("Kovinski", typeof(string));
            var cityNameMappingReplaced = ParameterExpressionReplacer.Replace(cityNameMapping, parameterExpression) as LambdaExpression;
            BinaryExpression equation = Expression.Equal(cityNameMappingReplaced.Body, value);

            ConstantExpression intValue = Expression.Constant(1, typeof(int));
            var nekiIntNameMappingReplaced = ParameterExpressionReplacer.Replace(nekiIntNameMapping, parameterExpression) as LambdaExpression;
            BinaryExpression nekiIntEquation = Expression.Equal(nekiIntNameMappingReplaced.Body, intValue);

            BinaryExpression res = Expression.And(nekiIntEquation, equation);
            //BinaryExpression test = Expression.Equal(cityNameMapping.Body, value);

            Expression<Func<Entities.School, bool>> lambda1 =
                Expression.Lambda<Func<Entities.School, bool>>(
                    res,
                    new ParameterExpression[] { parameterExpression });

            var l = _db.Schools.Include("City").Where(lambda1).ToList();
            return l;
        }

    }

    public static class ParameterExpressionReplacer
    {
        public static Expression Replace(Expression expression, ParameterExpression parameter)
        {
            return new ReplaceVisitor().Modify(expression, parameter);
        }
    }

    class ReplaceVisitor : ExpressionVisitor
    {
        private ParameterExpression parameter;

        public Expression Modify(Expression expression, ParameterExpression parameter)
        {
            this.parameter = parameter;
            return Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return parameter;
        }
    }
}