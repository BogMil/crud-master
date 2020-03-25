using System;
using System.Linq.Expressions;
using CrudMaster.Utils;

namespace CrudMaster.Sorter

{
    public class GenericOrderByPredicateCreator<TEntity, TQueryDto> :
        IOrderByProperties, IOrderByPredicateCreator where TEntity : class
    {
        private readonly IMappingService _mappingService;
        private readonly ParameterExpression _parameterExpression;
        public string OrderDirection { get; set; }


        //public Func<TEntity, dynamic> OrderByColumnFunc { get; set; }

        public string OrderByProperty { get; set; }
        public LambdaExpression x { get; set; }

        public GenericOrderByPredicateCreator()
        {
            _mappingService = new MappingService();
            _parameterExpression= Expression.Parameter(typeof(TEntity), Constants.PARAMETER_EXPRESSION_NAME);
        }

        public IOrderByProperties GetPropertyObject(OrderByProperties orderByProperties)
        {
            x = new MappingExpression<TQueryDto, TEntity>("NekiInt").LambdaExpression;
            var sourceTypeLambdaExpressionCreatorType = typeof(Func<,>).MakeGenericType(typeof(TEntity), x.ReturnType);

            if (orderByProperties.OrderByColumn == null) return this;

            OrderByProperty = _mappingService.GetPropertyPathInSourceType(orderByProperties.OrderByColumn,
                typeof(TQueryDto), typeof(TEntity));

            OrderDirection = orderByProperties.OrderDirection ?? OrderDirections.Ascending;

            
            

            return this;

        }
    }
}