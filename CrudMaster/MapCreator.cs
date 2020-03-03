using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;

namespace CrudMaster
{
    public interface IMapFrom<TSource, TDestination>
    {
        IMapTo<TSource, TDestination> From<T>(Expression<Func<TSource, T>> fromExpression);
    }

    public interface IMapTo<TSource, TDestination>
    {
        IMapFrom<TSource, TDestination> To(Expression<Func<TDestination, dynamic>> to);
    }

    internal interface IMapCreator<TSource, TDestination>
    {
        Dictionary<Expression<Func<TDestination, dynamic>>,
            Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>>> GetMappings();

        IMapTo<TSource, TDestination> ToConfigurable();
    }

    internal static class MapCreatorFactory<TSource, TDestination>
    {
        public static IMapCreator<TSource, TDestination> Create() => new MapCreator<TSource, TDestination>();
    }
    public class MapCreator<TSource, TDestination> : IMapFrom<TSource, TDestination>, IMapTo<TSource, TDestination>, IMapCreator<TSource, TDestination>
    {
        private readonly Dictionary<Expression<Func<TDestination, dynamic>>, Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>>> _entityToQueryDto
            = new Dictionary<Expression<Func<TDestination, dynamic>>, Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>>>();
        public Dictionary<Expression<Func<TDestination, dynamic>>, Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>>> GetMappings() => _entityToQueryDto;
        protected Map<TSource, TDestination> Map = new Map<TSource, TDestination>();
        public IMapTo<TSource, TDestination> ToConfigurable() => this;

        public IMapTo<TSource, TDestination> From<T>(Expression<Func<TSource, T>> sourceExpression)
        {
            Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>> fromExpression = x => x.MapFrom(sourceExpression);
            Map.From = fromExpression;
            AddMapToDictionary();
            return this;
        }

        public IMapFrom<TSource, TDestination> To(Expression<Func<TDestination, dynamic>> to)
        {
            Map.To = to;
            return this;
        }

        private void AddMapToDictionary()
        {
            _entityToQueryDto.TryAdd(Map.To, Map.From);
            Map = new Map<TSource, TDestination>();
        }


    }

    public class Map<TSource, TDestination>
    {
        public Expression<Func<TDestination, dynamic>> To { get; set; }
        public Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>> From { get; set; }
    }
}
