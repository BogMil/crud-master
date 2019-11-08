using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using CrudMaster.Extensions;
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
using ExpressionExtensions = CrudMaster.Extensions.ExpressionExtensions;

namespace CrudMaster.Service
{
    public abstract class GenericService<TQueryDto, TCommandDto, TRepository, TEntity>
        : IGenericService<TQueryDto, TCommandDto>
        where TQueryDto : class
        where TCommandDto : class
        where TRepository : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected TRepository Repository { get; }
        protected IMapper Mapper { get; }

        protected IMappingService MappingService { get; }

        protected GenericService(TRepository repository, IMapper mapper)
        {
            Repository = repository;
            MappingService = new MappingService();
            Mapper = Mapping.Mapper;
        }

        public IEnumerable<TQueryDto> GetListOfDto(Pager pager, string filters, OrderByProperties orderByProperties)
        {
            var entities = GetListOfEntites(pager, filters, orderByProperties);
            var listOfDto = MappingService.Mapper.Map<List<TEntity>, List<TQueryDto>>(entities.ToList());
            return listOfDto;
        }

        public virtual StaticPagedList<TQueryDto> GetJqGridData(Pager pager, string filters, OrderByProperties orderByProperties)
        {

            var entities = GetListOfEntites(pager, filters, orderByProperties);
            return MappingService.Mapper.Map<IPagedList<TEntity>, StaticPagedList<TQueryDto>>((PagedList<TEntity>)entities);
        }
        public virtual StaticPagedList<TQueryDto> GetJqGridDataTest(Pager pager, string filters, OrderByProperties orderByProperties)
        {
            var filterCreator = new FilterCreatorTEST<TEntity, TQueryDto>(filters);

            var wherePredicate = new Filter<TEntity>();
            if (filters != null)
                wherePredicate = filterCreator.Create();

            var entities = GetListOfEntitesTEST(pager, wherePredicate, orderByProperties);
            return MappingService.Map<IPagedList<TEntity>, StaticPagedList<TQueryDto>>((PagedList<TEntity>)entities);
        }

        protected virtual IPagedList<TEntity> GetListOfEntitesTEST(Pager pager, Filter<TEntity> filters, OrderByProperties orderByProperties)
        {
            return Repository.Filter(pager, filters, orderByProperties);
        }

        public Dictionary<string, string> OptionsForForeignKey(string dtoFkName, string templateWithColumnNames, string concatenator)
        {
            dtoFkName = dtoFkName.ToUpperFirsLetter();
            var template = new TemplateWithColumnNames(templateWithColumnNames);

            var entityFkName = MappingService.GetFkNameInSourceForDestinationFkName(dtoFkName, typeof(TQueryDto), typeof(TEntity));
            var typeOfLinkedTable = Repository.GetTypeOfLinkedTableByForeignKeyName(typeof(TEntity), entityFkName);
            var queryDtoOfLinkedTable = GetQueryDtoTypeOfRelatedTabelForFk(typeof(TEntity), entityFkName);

            var dtoColumnNames = template.GetDtoColumnNames().Select(s => s.ToUpperFirsLetter()).ToList();
            foreach (var dtoColumnName in dtoColumnNames)
            {
                if (!dtoColumnName.Contains("."))
                {
                    var exp = MappingService.GetMappingExpressionFromDestinationPropToSourceProp(dtoColumnName, queryDtoOfLinkedTable, typeOfLinkedTable);
                    template.ExpressionsOfDtoToEntityColNames.Add(dtoColumnName.ToLower(), exp);
                }
                else
                {

                }
            }

            return Repository.OptionsForForeignKey(typeOfLinkedTable, template);
        }

        public Type GetQueryDtoTypeOfRelatedTabelForFk(Type entity, string fkName)
        {
            var typeOfLinkedTable = Repository.GetTypeOfLinkedTableByForeignKeyName(entity, fkName);
            var serviceRelatedToLinkedTable = entity.Assembly.GetCrudMasterServiceWithTEntity(typeOfLinkedTable);
            return serviceRelatedToLinkedTable.BaseType?.GenericTypeArguments[0];
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

            MappingService.Mapper.Map(dto, entity);

            Repository.Create(entity);
        }

        public virtual TQueryDto CreateAndReturn(TCommandDto dto)
        {
            ValidateDtoBeforeCreate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();

            MappingService.Mapper.Map(dto, entity);

            var createdEntity = Repository.CreateAndReturn(entity);
            return MappingService.Mapper.Map<TEntity, TQueryDto>(createdEntity);
        }

        public virtual TModel CreateAndReturnModel<TModel>(TCommandDto dto)
        {
            ValidateDtoBeforeCreate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();
            Mapper.Map(dto, entity);

            var createdEntity = Repository.CreateAndReturn(entity);
            return MappingService.Mapper.Map<TEntity, TModel>(createdEntity);
        }

        public virtual void ValidateDtoBeforeCreate(TCommandDto dto)
        {

        }

        public virtual void Update(TCommandDto dto)
        {
            ValidateDtoBeforeUpdate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();

            MappingService.Mapper.Map(dto, entity);

            Repository.Update(entity);
        }

        public virtual TQueryDto UpdateAndReturn(TCommandDto dto)
        {
            ValidateDtoBeforeUpdate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();
            MappingService.Mapper.Map(dto, entity);

            var upadtedEntity = Repository.UpdateAndReturn(entity);
            return MappingService.Mapper.Map<TEntity, TQueryDto>(upadtedEntity);

        }

        public virtual TModel UpdateAndReturnModel<TModel>(TCommandDto dto)
        {
            ValidateDtoBeforeUpdate(dto);
            ValidateDtoBeforeUpdateOrCreate(dto);

            var entity = Repository.NewDbSet();
            MappingService.Mapper.Map(dto, entity);

            var createdEntity = Repository.UpdateAndReturn(entity);
            return MappingService.Mapper.Map<TEntity, TModel>(createdEntity);
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
            return MappingService.Mapper.Map<TEntity, TQueryDto>(entity);
        }
    }
}