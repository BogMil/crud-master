using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CrudMaster.Extensions;
using CrudMaster.Utils;
using X.PagedList;
using Expression = System.Linq.Expressions.Expression;

namespace CrudMaster
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {

            var appAssembly = Config.AppAssembly;
            var mappingTypes = appAssembly.GetTypes()
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.IsSubclassOfGenericType(typeof(CrudMasterMappingProfile<,,>)))
                .Select(t => t)
                .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                mappingTypes.ForEach(cfg.AddProfile);

            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper GetMapper() => Lazy.Value;
    }

    public abstract class CrudMasterMappingProfile<TQueryDto, TCommandDto, TEntity> : Profile where TQueryDto : class where TEntity : class where TCommandDto : class
    {

        protected CrudMasterMappingProfile()
        {
            ValidateMapsFromEntityToQueryDto();
            CreateMappingToQueryDtoFromEntity();

            CreateMappingToEntityFromCommandDto();

            CreateMap<PagedList<TEntity>, StaticPagedList<TQueryDto>>()
                .ConvertUsing<PagedListConverter<TEntity, TQueryDto>>();
        }

        private void CreateMappingToEntityFromCommandDto()
        {
            var commandDtoToEntityMapperCreator = MapCreatorFactory<TCommandDto,TEntity>.Create();
            MapToEntityFromCommandDto(commandDtoToEntityMapperCreator.ToConfigurable());

            var commandDtoToEntityMap = CreateMap<TCommandDto, TEntity>();
            IgnoreReferenceTypesWhenMappingFromCommandDtoToEntity(commandDtoToEntityMap);
            foreach (var (key, value) in commandDtoToEntityMapperCreator.GetMappings())
            {
                commandDtoToEntityMap.ForMember(key, value.Compile());
            }
        }

        private void CreateMappingToQueryDtoFromEntity()
        {
            var entityToQueryDtoMapper = MapCreatorFactory<TEntity, TQueryDto>.Create();
            MapToQueryDtoFromEntity(entityToQueryDtoMapper.ToConfigurable());

            var entityToQueryDtoMap = CreateMap<TEntity, TQueryDto>();
            foreach (var (key, value) in entityToQueryDtoMapper.GetMappings())
            {
                entityToQueryDtoMap.ForMember(key, value.Compile());
            }
        }

        private void IgnoreReferenceTypesWhenMappingFromCommandDtoToEntity(IMappingExpression<TCommandDto, TEntity> mapping)
        {
            var propertiesOfEntity = typeof(TEntity).GetProperties().ToList();
            var propertiesToIgnore = propertiesOfEntity.GetPropertiesThatAreNotBaseTypes();

            propertiesToIgnore.ForEach(property =>
            {
                var expressionCreator = new LambdaExpressionFromPath<TEntity>(property.Name);
                mapping.ForMember(expressionCreator.FullPropertyPath, o => o.Ignore());
            });
        }

        public abstract void MapToQueryDtoFromEntity(IMapTo<TEntity, TQueryDto> map);
        public abstract void MapToEntityFromCommandDto(IMapTo<TCommandDto, TEntity> map);

        public void ValidateMapsFromEntityToQueryDto()
        {
            //foreach (var (key, value) in _entityToQueryDto)
            //{
            //    var mapFromT = ".MapFrom(";
            //    var x = value.ToString();

            //    if (x.Contains(mapFromT))
            //    {
            //        var q = (value.Body as MethodCallExpression).Arguments[0];
            //        var w = (q as UnaryExpression).Operand;//just check return type
            //        CheckExpressionByNodeType(w);
            //        var index = x.IndexOf(mapFromT) + mapFromT.Length;
            //        var end = x.Length - index - 1;
            //        var z = x.Substring(index, end);
            //    }
            //}
        }

        private void CheckExpressionByNodeType(Expression expression)
        {
            var e = expression as LambdaExpression;
            var body = e.Body;
            switch (body.NodeType)
            {
                case ExpressionType.Call:
                    var parameters = (body as MethodCallExpression).Method.GetParameters();
                    foreach (var parameterInfo in parameters)
                    {
                        if (!BaseTypes.IsBaseType(parameterInfo.ParameterType))
                            throw new Exception("Do not make calls");
                    }
                    break;

                case ExpressionType.MemberAccess:

                    break;
                case ExpressionType.Add:

                    break;

            }
        }
    }
}
