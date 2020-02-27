using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using CrudMaster;
using CrudMasterApi.Models.CrudMaster;

namespace CrudMasterApi.School
{
    public class SchoolBase
    {
        public int? Id { get; set; } = 0;
        public string Name { get; set; }
        public string Mail { get; set; }
        public int DtoCityId { get; set; }
        public int NekiInt { get; set; }
        public int? NekiNullableInt { get; set; }
        public long NekiLong { get; set; }
        public bool NekiBool { get; set; }
        public decimal NekiDecimal { get; set; }
        public double NekiDouble { get; set; }
        public float NekiFloat { get; set; }
        public DateTime NekiDatum { get; set; }
    }

    public class SchoolQueryDto : SchoolBase
    {
        public CityQueryDto City { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }

    }

    public class SchoolCommandDto : SchoolBase { }
    public class SchoolMappingProfile : CrudMasterMappingProfile<SchoolQueryDto, SchoolCommandDto, Entities.School>
    {
        public override void PopulateMps(
            Dictionary<Expression<Func<SchoolQueryDto, dynamic>>, Expression<
                Action<IMemberConfigurationExpression<Entities.School, SchoolQueryDto, dynamic>>>> entityToQueryDto)
        {
            entityToQueryDto.Add(d => d.DtoCityId, o => o.MapFrom(s => s.CityId));
            entityToQueryDto.Add(d => d.CityName, o => o.MapFrom(s => s.City.Name + "ski"));
            entityToQueryDto.Add(d => d.RegionName, o => o.MapFrom(s => s.City.Region.Name));

            CommandDtoToEntity.Add(d => d.CityId, o => o.MapFrom(s => s.DtoCityId));

            var map= MapCreatorFactory<Entities.School,SchoolQueryDto>.Create();

            map
                .From(s => s.City).To(s => s.City.Id)
                .From(s => s.City).To(s => s.City.Id)
                .From(s => s.City).To(s => s.City.Id)
                .From(s => s.City).To(s => s.City.Id)
                .From(s => s.City).To(s => s.City.Id);
            var z = map._entityToQueryDto;
        }
    }

    public interface IFrom<TSource,TDestination>
    {
        ITo<TSource, TDestination> From(Expression<Func<TSource, dynamic>> fromExpression);

        Dictionary<Expression<Func<TDestination, dynamic>>,
            Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>>> _entityToQueryDto
        {
            get;
            set;
        }
    }

    public interface ITo<TSource, TDestination>
    {
        IFrom<TSource, TDestination> To(Expression<Func<TDestination, dynamic>> to);
    }

    public static class MapCreatorFactory<TSource,TDestination>
    {
        public static IFrom<TSource, TDestination> Create()=>new MapCreator<TSource, TDestination>();
    }
    public class MapCreator<TSource,TDestination> : IFrom<TSource, TDestination>, ITo<TSource, TDestination>
    {
        public Dictionary<Expression<Func<TDestination, dynamic>>, Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>>> _entityToQueryDto
        {
            get;
            set;
        }
            = new Dictionary<Expression<Func<TDestination, dynamic>>, Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>>>();

        protected Map<TSource,TDestination> Map=new Map<TSource,TDestination>();

        public ITo<TSource, TDestination> From(Expression<Func<TSource, dynamic>> sourceExpression)
        {
            Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>> fromExpression = x =>
                x.MapFrom(sourceExpression);
            Map.From = fromExpression;
            return this;
        }

        public IFrom<TSource, TDestination> To(Expression<Func<TDestination, dynamic>> to)
        {
            Map.To = to;
            AddMapToDictionary();
            return this;
        }

        private void AddMapToDictionary()
        {
            _entityToQueryDto.TryAdd(Map.To, Map.From);
            Map=new Map<TSource, TDestination>();
        }
    }

    public class Map<TSource,TDestination>
    {
        public Expression<Func<TDestination, dynamic>> To { get; set; }
        public Expression<Action<IMemberConfigurationExpression<TSource, TDestination, dynamic>>> From { get; set; }
    }
}
