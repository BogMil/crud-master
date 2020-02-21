using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CrudMaster
{
    public interface IMappingExpression
    {
        LambdaExpression LambdaExpression { get; }
        Expression Body { get; }
        void ReplaceParameter(ParameterExpression parameterExpression);
        Type ReturnType { get; }
    }
    public class MappingExpression<TDestination,TSource>:IMappingExpression
    {
        public MappingExpression(string destinationField)
        {
            var ms=new MappingService();
            LambdaExpression = ms.GetPropertyMappingExpression(destinationField, typeof(TDestination), typeof(TSource));
        }

        public LambdaExpression LambdaExpression { get; private set; }
        public Expression Body => LambdaExpression.Body;

        public void ReplaceParameter(ParameterExpression parameterExpression)
        {
            LambdaExpression = ParameterExpressionReplacer.Replace(LambdaExpression, parameterExpression) as LambdaExpression;
        }

        public Type ReturnType => LambdaExpression.ReturnType;
    }
}
