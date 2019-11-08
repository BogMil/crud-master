using System.Linq;

namespace CrudMaster.Extensions
{
    public static class EfExtensions
    {
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(
            this IQueryable<TEntity> source,
            string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(
            this IQueryable<TEntity> source,
            string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(
            this IOrderedQueryable<TEntity> source,
            string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }

        public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(
            this IOrderedQueryable<TEntity> source,
            string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }

        private static IOrderedQueryable<TEntity> ApplyOrder<TEntity>(
        IQueryable<TEntity> source,
        string fullPropertyPath,
        string methodName)
        {
            var lambdaExpressionCreator = new LambdaExpressionCreator<TEntity>(fullPropertyPath);
            var x = lambdaExpressionCreator.LambdaExpression;
            x = null;

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(lambdaExpressionCreator.EntityType, lambdaExpressionCreator.ExpressionsFuncReturnType)
                    .Invoke(null, new object[] { source, lambdaExpressionCreator.LambdaExpression });

            return (IOrderedQueryable<TEntity>)result;
        }
    }
}
