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

    public class SchoolService : GenericService<SchoolQueryDto, SchoolCommandDto, ISchoolRepository, Entities.School>, ISchoolService
    {
        private readonly AccountingContext _db;
        public SchoolService(ISchoolRepository repository, AccountingContext db) : base(repository)
        {
            _db = db;
        }
        public List<SchoolQueryDto> List()
        {
            var cityNameMapping = MappingService.GetMappingExpressionFromDestinationPropToSourceProp("CityName", typeof(SchoolQueryDto),
                typeof(Entities.School));

            var nekiIntNameMapping = MappingService.GetMappingExpressionFromDestinationPropToSourceProp("NekiInt", typeof(SchoolQueryDto),
                typeof(Entities.School));

            ParameterExpression parameterExpression = Expression.Parameter(typeof(Entities.School), "s");

            //ConstantExpression value = Expression.Constant("Kovinski", typeof(string));
            //BinaryExpression equation = Expression.Equal(cityNameMapping.Body, value);

            ConstantExpression intValue = Expression.Constant(1, typeof(int));
            var newExp = ParameterExpressionReplacer.Replace(nekiIntNameMapping, parameterExpression);
            var x = newExp.Parameters[0] == parameterExpression;
            BinaryExpression nekiIntEquation = Expression.Equal(newExp.Body, intValue);
           
            //BinaryExpression res = Expression.And(nekiIntEquation, equation);
            //BinaryExpression test = Expression.Equal(cityNameMapping.Body, value);

            Expression<Func<Entities.School, bool>> lambda1 =
                Expression.Lambda<Func<Entities.School, bool>>(
                    nekiIntEquation,
                    new ParameterExpression[] { parameterExpression });

            var l = _db.Schools.Include("City").Where(lambda1).ToList();
            return null;
        }

    }

    public static class ParameterExpressionReplacer
    {
        public static LambdaExpression Replace(LambdaExpression expression, ParameterExpression parameter)
        {
            return new ReplaceVisitor().Modify(expression, parameter);
        }
    }

    public class ReplaceVisitor : ExpressionVisitor
    {
        private ParameterExpression parameter;

        public LambdaExpression Modify(LambdaExpression expression, ParameterExpression parameter)
        {
            this.parameter = parameter;
            return Visit(expression) as LambdaExpression;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node;
        }

        //protected override Expression VisitLambda<T>(Expression<T> node)
        //{
        //    return Expression.Lambda<Func<svc_JobAudit, bool>>(Visit(node.Body), Expression.Parameter(typeof(svc_JobAudit)));
        //}

        //protected override Expression VisitParameter(ParameterExpression node)
        //{

        //    return Expression.Property(parameter, "s");
        //}
    }
}