using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using AutoMapper;
using CrudMaster.Service;

namespace CrudMaster
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {

            var appAssembly = Config.AppAssembly;
            var mappingTypes= appAssembly.GetTypes()
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.IsSubclassOfGenericType(typeof(CrudMasterMappingProfile<,,>)))
                .Select(t=>t)
                .ToList();

            var config = new MapperConfiguration(cfg => {
                mappingTypes.ForEach(cfg.AddProfile);
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }

    public abstract class CrudMasterMappingProfile<TQueryDto, TCommandDto, TEntity> : Profile where TQueryDto : class where TEntity : class where TCommandDto : class
    {
        public Dictionary<Expression<Func<TQueryDto, dynamic>>,
            Action<IMemberConfigurationExpression<TEntity, TQueryDto, dynamic>>> EntityToQueryDto =
            new Dictionary<Expression<Func<TQueryDto, dynamic>>,
                Action<IMemberConfigurationExpression<TEntity, TQueryDto, dynamic>>>();

        protected CrudMasterMappingProfile()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            PopulateMps();
            var x = CreateMap<TEntity, TQueryDto>();
            foreach (var (key, value) in EntityToQueryDto)
            {
                x.ForMember(key, value);
            }
        }

        public abstract void PopulateMps();

    }
}
