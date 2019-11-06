using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using CrudMaster.Filter;
using CrudMaster.Repository;
using CrudMaster.Sorter;
using ExpressionBuilder.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore.Infrastructure;
using X.PagedList;
using Expression = System.Linq.Expressions.Expression;
using ExpressionBuilder.Generics;
using ExpressionBuilder.Interfaces;
using Newtonsoft.Json.Linq;

namespace CrudMaster.Service
{
    public abstract class GenericService<TQueryDto, TCommandDto, TRepository, TEntity>
        : IGenericService<TQueryDto, TCommandDto>
        where TQueryDto : class
        where TCommandDto : class
        where TRepository : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected TRepository Repository { get; private set; }
        protected IMapper Mapper { get; private set; }

        protected GenericService(TRepository repository, IMapper mapper)
        {
            Repository = repository;
            //Mapper = mapper;
            Mapper = Mapping.Mapper;
        }

        public IEnumerable<TQueryDto> GetListOfDto(Pager pager, string filters, OrderByProperties orderByProperties)
        {
            var entities = GetListOfEntites(pager, filters, orderByProperties);
            var listOfDto = Mapper.Map<List<TEntity>, List<TQueryDto>>(entities.ToList());
            return listOfDto;
        }

        public virtual StaticPagedList<TQueryDto> GetJqGridData(Pager pager, string filters, OrderByProperties orderByProperties)
        {

            var entities = GetListOfEntites(pager, filters, orderByProperties);
            return Mapper.Map<IPagedList<TEntity>, StaticPagedList<TQueryDto>>((PagedList<TEntity>)entities);
        }
        public virtual StaticPagedList<TQueryDto> GetJqGridDataTest(Pager pager, string filters, OrderByProperties orderByProperties)
        {
            var wherePredicate= new Filter<TEntity>();
            if (filters != null)
            {
                var jsonFilters = filters.TryParseJToken();

                var filterCreator = new FilterCreatorTEST<TEntity>();
                var x = filterCreator.GetOperationByString("eq");

                var groupConnector = filterCreator.GetGroupConnectorFromString(jsonFilters["groupOp"].ToString());
                var listOfFilters = (JArray) jsonFilters["rules"];

                var filter = new Filter<TEntity>();
                foreach (var filterRule in listOfFilters)
                {
                    var mappingsForLinkedTEntity = Mapper.GetTypeMapFor(typeof(TEntity), typeof(TQueryDto));
                    //var entityExpressionCreatorType = new LambdaExpressionCreator<TEntity>();
                    var entityPathExp =
                        GetMappingExpressionFromDtoToEntity(filterRule["field"].ToString(), mappingsForLinkedTEntity,
                            typeof(TEntity));

                    var stack = ExpressionExtensions.GetStackOfExpressionBodyProperties(entityPathExp.Body);
                    var fullPropertyName = string.Join(".", stack.ToArray());
                    var propertyType = PropertyTypeExtractor<TEntity>.GetPropertyTypeName(fullPropertyName);
                    var op = filterRule["op"].ToString();
                    IOperation operation = filterCreator.GetOperationByString(op);
                    var dataStr = filterRule["data"].ToString();
                    dynamic data = filterCreator.GetValidDataByOperationType(operation, dataStr, propertyType);
                    filter.By(fullPropertyName, operation, data, groupConnector);
                }

                wherePredicate = filter;
            }

            var entities = GetListOfEntitesTEST(pager, wherePredicate, orderByProperties);
            return Mapper.Map<IPagedList<TEntity>, StaticPagedList<TQueryDto>>((PagedList<TEntity>)entities);
        }

        protected virtual IPagedList<TEntity> GetListOfEntitesTEST(Pager pager, Filter<TEntity> filters, OrderByProperties orderByProperties)
        {
            return Repository.Filter(pager, filters, orderByProperties);
        }

        // ReSharper disable once InconsistentNaming
        public Dictionary<string, string> OptionsForForeignKey(string dtoFK, string templateWithColumnNames, string concatenator)
        {
            dtoFK = dtoFK.ToUpperFirsLetter();
            var template = new TemplateWithColumnNames(templateWithColumnNames);

            var entityFK = GetFKNameInEntityForDtoFKName(dtoFK, typeof(TQueryDto), typeof(TEntity));
            var typeOfLinkedTable = GetTypeOfTableRelatedToFK(typeof(TEntity), entityFK);
            var queryDtoOfLinkedTable = GetQueryDtoTypeOfRelatedTabelForFK(typeof(TEntity),entityFK);

            var mappingsForLinkedTEntity = Mapper.GetTypeMapFor(typeOfLinkedTable, queryDtoOfLinkedTable);
            var linkedTableExpressionCreatorType = typeof(LambdaExpressionCreator<>).MakeGenericType(typeOfLinkedTable);

            var dtoColumnNames = template.GetDtoColumnNames().Select(s => s.ToUpperFirsLetter()).ToList();
            foreach (var dtoColumnName in dtoColumnNames)
            {
                if (!dtoColumnName.Contains("."))
                {
                    var exp = GetMappingExpressionFromDtoToEntity(dtoColumnName,mappingsForLinkedTEntity,linkedTableExpressionCreatorType);
                    template.ExpressionsOfDtoToEntityColNames.Add(dtoColumnName.ToLower(), exp);
                }
                else
                {

                }
            }

            return Repository.OptionsForForeignKey(typeOfLinkedTable, template);

        }

        public LambdaExpression GetMappingExpressionFromDtoToEntity(string dtoColumnName, TypeMap mappingsForLinkedTEntity,Type linkedTableExpressionCreatorType)
        {
            var pathToLinkedTableProp = dtoColumnName;
            var propertyMap = mappingsForLinkedTEntity.GetPropertyMapForDestionationName(dtoColumnName);
            if (propertyMap.CustomMapExpression != null)
            {
                dynamic expression = propertyMap.CustomMapExpression;
                return expression;
                //pathToLinkedTableProp = ExpressionExtensions.NonExtenionGetExpressionBodyAsString(expression);
            }

            dynamic linkedTableExpressionCreator =
                Activator.CreateInstance(linkedTableExpressionCreatorType, pathToLinkedTableProp);
            return linkedTableExpressionCreator.LambdaExpression;
        }

        // ReSharper disable once InconsistentNaming
        public Type GetQueryDtoTypeOfRelatedTabelForFK(Type entity, string fkName)
        {
            var typeOfLinkedTable = GetTypeOfTableRelatedToFK(entity, fkName);
            var serviceRelatedToLinkedTable = entity.Assembly.GetCrudMasterServiceWithTEntity(typeOfLinkedTable);
            return serviceRelatedToLinkedTable.BaseType?.GenericTypeArguments[0];
        }

        public Type GetQueryDtoTypeOfForSourceEntity(Type entity)
        {
            var serviceRelatedToLinkedTable = entity.Assembly.GetCrudMasterServiceWithTEntity(entity);
            return serviceRelatedToLinkedTable.BaseType?.GenericTypeArguments[0];
        }

        // ReSharper disable once InconsistentNaming
        public string GetFKNameInEntityForDtoFKName(string dtoFK,Type dto,Type entity )
        {
            var mappingsForEntity = Mapper.GetTypeMapFor(typeof(TEntity), destinationType: typeof(TQueryDto));
            var mappingForFk = mappingsForEntity.GetPropertyMapForDestionationName(dtoFK);
            return mappingForFk.GetForeignKeyEntityName();
        }

        // ReSharper disable once InconsistentNaming
        public Type GetTypeOfTableRelatedToFK(Type entityThatHasFK, string fkName )
        {
            return Repository.GetTypeOfLinkedTableByForeignKeyName(entityThatHasFK, fkName);
        }

        protected virtual IPagedList<TEntity> GetListOfEntites(Pager pager, string filters, OrderByProperties orderByProperties)
        {
            return Repository.Filter(pager, filters, orderByProperties);
        }

        public virtual void Create(TCommandDto dto)
        {
            ValidateDtoBeforeCreate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();

            Mapper.Map(dto, entity);

            Repository.Create(entity);
        }

        public virtual TQueryDto CreateAndReturn(TCommandDto dto)
        {
            ValidateDtoBeforeCreate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();

            Mapper.Map(dto, entity);

            var createdEntity = Repository.CreateAndReturn(entity);
            return Mapper.Map<TEntity, TQueryDto>(createdEntity);
        }

        public virtual TModel CreateAndReturnModel<TModel>(TCommandDto dto)
        {
            ValidateDtoBeforeCreate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();
            Mapper.Map(dto, entity);

            var createdEntity = Repository.CreateAndReturn(entity);
            return Mapper.Map<TEntity, TModel>(createdEntity);
        }

        public virtual void ValidateDtoBeforeCreate(TCommandDto dto)
        {

        }

        public virtual void Update(TCommandDto dto)
        {
            ValidateDtoBeforeUpdate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();

            Mapper.Map(dto, entity);

            Repository.Update(entity);
        }

        public virtual TQueryDto UpdateAndReturn(TCommandDto dto)
        {
            ValidateDtoBeforeUpdate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();
            Mapper.Map(dto, entity);

            var upadtedEntity = Repository.UpdateAndReturn(entity);
            return Mapper.Map<TEntity, TQueryDto>(upadtedEntity);

        }

        public virtual TModel UpdateAndReturnModel<TModel>(TCommandDto dto)
        {
            ValidateDtoBeforeUpdate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();
            Mapper.Map(dto, entity);

            var createdEntity = Repository.UpdateAndReturn(entity);
            return Mapper.Map<TEntity, TModel>(createdEntity);
        }

        public virtual void ValidateDtoBeforeUpdate(TCommandDto dto)
        {

        }

        public virtual void ValidateDtoBeforeUpdateOrCreate(TCommandDto dto)
        {

        }
        public virtual void Delete(int id)
        {
            Repository.Delete(id);
        }
        public virtual void ValidateDtoBeforeDelete(TCommandDto dto)
        {

        }

        public virtual int DeleteAndReturn(int id)
        {
            return Repository.DeleteAndReturn(id);
        }

        public TQueryDto Find(int id)
        {
            var entity = Repository.Find(id);
            return Mapper.Map<TEntity, TQueryDto>(entity);
        }
    }
}