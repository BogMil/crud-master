// ReSharper disable InconsistentNaming

using CrudMaster.PropertyMapper;
using ExpressionBuilder.Generics;

namespace CrudMaster.Filter
{
    public abstract class GenericWherePredicateCreator<TEntity, TPropertyMapper>: IWherePredicateCreator<TEntity>
        where TEntity : class where TPropertyMapper : class, IPropertyMapper<TEntity>, new()
    {
        private Filter<TEntity> WherePredicate { get; set; } = new Filter<TEntity>();
        private readonly FilterCreator<TEntity,TPropertyMapper> _filterCreator;

        protected GenericWherePredicateCreator()
        {
            _filterCreator = new FilterCreator<TEntity, TPropertyMapper>();
        }

        public Filter<TEntity> GetWherePredicate(string filters)
        {
            if (filters == null)
                return WherePredicate;

            var jsonFilters = filters.TryParseJToken();
            WherePredicate = _filterCreator.Create(jsonFilters);

            return WherePredicate;
        }

    }
}