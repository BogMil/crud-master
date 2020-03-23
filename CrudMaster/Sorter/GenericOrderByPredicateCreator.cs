using CrudMaster.Utils;

namespace CrudMaster.Sorter

{
    public class GenericOrderByPredicateCreator<TEntity, TQueryDto> :
        IOrderByProperties, IOrderByPredicateCreator where TEntity : class
    {
        private readonly IMappingService _mappingService;
        public string OrderDirection { get; set; }


        //public Func<TEntity, dynamic> OrderByColumnFunc { get; set; }

        public string OrderByProperty { get; set; }

        public GenericOrderByPredicateCreator()
        {
            _mappingService = new MappingService();
        }

        public IOrderByProperties GetPropertyObject(OrderByProperties orderByProperties)
        {
            if (orderByProperties.OrderByColumn == null) return null;

            OrderByProperty = _mappingService.GetPropertyPathInSourceType(orderByProperties.OrderByColumn,
                typeof(TQueryDto), typeof(TEntity));

            OrderDirection = orderByProperties.OrderDirection ?? SortDirections.Ascending;

            return this;

        }
    }
}