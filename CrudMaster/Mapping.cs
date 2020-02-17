using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using CrudMaster.Extensions;
using CrudMaster.Service;
using Microsoft.VisualBasic.CompilerServices;
using X.PagedList;
using Expression = System.Linq.Expressions.Expression;
using NewArrayExpression = Castle.DynamicProxy.Generators.Emitters.SimpleAST.NewArrayExpression;

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

        public static IMapper Mapper => Lazy.Value;
    }

    public abstract class CrudMasterMappingProfile<TQueryDto, TCommandDto, TEntity> : Profile where TQueryDto : class where TEntity : class where TCommandDto : class
    {
        public Dictionary<Expression<Func<TQueryDto, dynamic>>,
            Expression<Action<IMemberConfigurationExpression<TEntity, TQueryDto, dynamic>>>> EntityToQueryDto =
            new Dictionary<Expression<Func<TQueryDto, dynamic>>,
                Expression<Action<IMemberConfigurationExpression<TEntity, TQueryDto, dynamic>>>>();

        public Dictionary<Expression<Func<TEntity, dynamic>>,
            Expression<Action<IMemberConfigurationExpression<TCommandDto, TEntity,  dynamic>>>> CommandDtoToEntity =
            new Dictionary<Expression<Func<TEntity, dynamic>>,
                Expression<Action<IMemberConfigurationExpression<TCommandDto, TEntity, dynamic>>>>();

        protected CrudMasterMappingProfile()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            PopulateMps();
            ValidateMapsFromEntityToQueryDto();
            var x = CreateMap<TEntity, TQueryDto>();
            foreach (var (key, value) in EntityToQueryDto)
            {
                x.ForMember(key, value.Compile());
            }

            var y = CreateMap<TCommandDto,TEntity>();
            foreach (var (key, value) in CommandDtoToEntity)
            {
                y.ForMember(key, value.Compile());
            }

            CreateMap<PagedList<TEntity>, StaticPagedList<TQueryDto>>()
                .ConvertUsing<PagedListConverter<TEntity, TQueryDto>>();
        }

        public abstract void PopulateMps();

        public void ValidateMapsFromEntityToQueryDto()
        {
            foreach (var (key, value) in EntityToQueryDto)
            {
                var mapFromT = ".MapFrom(";
                var x = value.ToString();

                if (x.Contains(mapFromT))
                {
                    var q = (value.Body as MethodCallExpression).Arguments[0];
                    var w = (q as UnaryExpression).Operand;//just check return type
                    CheckExpressionByNodeType(w);
                    var index = x.IndexOf(mapFromT) + mapFromT.Length;
                    var end = x.Length - index - 1;
                    var z = x.Substring(index, end);
                }
            }
        }

        private void CheckExpressionByNodeType(Expression expression)
        {
            var e = expression as LambdaExpression;
            var body = e.Body;
            switch (body.NodeType)
            {
                case ExpressionType.Call:
                    var parameters=(body as MethodCallExpression).Method.GetParameters();
                    foreach (var parameterInfo in parameters)
                    {
                        if(!IsBaseType(parameterInfo.ParameterType))
                            throw new Exception("Do not make calls");
                    }
                    break;

                case ExpressionType.MemberAccess:

                    break;
                case ExpressionType.Add:

                    break;

            }
        }

        private bool IsBaseType(Type parameterInfoParameterType)
        {
            List<Type> BaseTypes= new List<Type>()
            {
                typeof(string),
                typeof(int),
                typeof(float)
            };

            return BaseTypes.Contains(parameterInfoParameterType);
        }
    }
}
