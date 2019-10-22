using System;
using System.Linq.Expressions;
using System.Reflection;
using CrudMaster.PropertyMapper;

namespace CrudMaster.Sorter

{
    public abstract class GenericOrderByPredicateCreator<TEntity,TPropertyMapper> : 
        IOrderByProperties<TEntity>, IOrderByPredicateCreator<TEntity> where TPropertyMapper : class,IPropertyMapper<TEntity>, new() 
        where TEntity : class
    {
        Func<TEntity, dynamic> _orderByColumnFunc;
        protected readonly TPropertyMapper PropertyMapper;
        public string OrderDirection { get; set; }

        private PropertyInfo _testPropertyInfo { get; set; }
        public PropertyInfo TestPropertyInfo
        {
            get => _testPropertyInfo ?? GetDefaultPropertyInfo();
            set => _testPropertyInfo = value;
        }

        public Func<TEntity, dynamic> OrderByColumnFunc
        {
            get => _orderByColumnFunc ?? GetDefaultOrderByColumnFunc();
            set => _orderByColumnFunc = value;
        }

        private string _orderByProperty;
        public string OrderByProperty {
            get => _orderByProperty ?? GetDefaultOrderByProperty();
            set => _orderByProperty = value;
        }

        protected GenericOrderByPredicateCreator()
        {
            PropertyMapper = new TPropertyMapper();
        }

        public IOrderByProperties<TEntity> GetPropertyObject(OrderByProperties orderByProperties)
        {
            if (orderByProperties.OrderByColumn != null)
            {

                var x = PropertyMapper.GetPathInEfForDtoFieldExpression(orderByProperties.OrderByColumn);
                OrderByProperty= x.GetExpressionBodyAsString();
                var propName = x.GetExpressionBodyAsString();
                TestPropertyInfo = PropertyTypeExtractor<TEntity>.GetPropertyInfo(propName);

                OrderByColumnFunc = x.Compile();
            }

            OrderDirection = orderByProperties.OrderDirection ?? SortDirections.Ascending;

            return this;
        }

        private Func<TEntity, dynamic> GetDefaultOrderByColumnFunc()
        {
            var defaultOrderByColumn = GetDefaultOrderByColumn();
            var defaultOrderByColumnPath = defaultOrderByColumn.GetExpressionBodyAsString();
            return PropertyMapper.GetPathInEfForDtoFieldExpression(defaultOrderByColumnPath).Compile();
        }

        private PropertyInfo GetDefaultPropertyInfo()
        {
            var defaultOrderByColumn = GetDefaultOrderByColumn();
            var defaultOrderByColumnPath = defaultOrderByColumn.GetExpressionBodyAsString();
            _orderByProperty = defaultOrderByColumnPath;
            return PropertyTypeExtractor<TEntity>.GetPropertyInfo(defaultOrderByColumnPath);
        }

        private string GetDefaultOrderByProperty()
        {
            var defaultOrderByColumn = GetDefaultOrderByColumn();
            return defaultOrderByColumn.GetExpressionBodyAsString();
        }

        protected abstract Expression<Func<TEntity, dynamic>> GetDefaultOrderByColumn();
    }
}