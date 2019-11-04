using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using CrudMaster.Repository;
using CrudMaster.Sorter;
using ExpressionBuilder.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using X.PagedList;
using Expression = System.Linq.Expressions.Expression;

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
            Mapper = mapper;
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

        public Dictionary<string, string> OptionsForForeignKey(string fkDtoName, string templateWithColumnNames, string concatenator)
        {
            fkDtoName = fkDtoName.ToUpperFirsLetter();

            var mappingForTEntity = Mapper.GetTypeMapFor(typeof(TEntity), destinationType: typeof(TQueryDto));
            var mappingForFk = mappingForTEntity.GetPropertyMapForDestionationName(fkDtoName);
            var fkEntityName = mappingForFk.GetForeignKeyEntityName();
            var typeOfLinkedTable = Repository.GetTypeOfLinkedTableByForeignKeyName(typeof(TEntity), fkEntityName);


            var serviceRelatedToLinkedTable= typeof(TEntity).Assembly.GetCrudMasterServiceWithTEntity(typeOfLinkedTable);
            var queryDtoOfLinkedTable = serviceRelatedToLinkedTable.BaseType.GenericTypeArguments[0];
            var mappingsForLinkedTEntity = Mapper.GetTypeMapFor(typeOfLinkedTable, queryDtoOfLinkedTable);

            //var z= serviceRelatedToLinkedTable.GetType().GetMethod("GetQueryDtoType").Invoke(serviceRelatedToLinkedTable, null);
            var template = new TemplateWithColumnNames(templateWithColumnNames);

            var dtoColumnNames = template.GetDtoColumnNames().Select(s => s.ToUpperFirsLetter()).ToList();
            var entityColumnNamesExpressions = new List<LambdaExpression>();
            var dtoColumnNameToExpression = new Dictionary<string, LambdaExpression>();
            foreach (var dtoColumnName in dtoColumnNames)
            {
                var pathToLinkedTableProp = "";
                var x = typeof(LambdaExpressionCreator<>).MakeGenericType(typeOfLinkedTable);
                var propertyMap = mappingsForLinkedTEntity.GetPropertyMapForDestionationName(dtoColumnName);
                if (propertyMap.CustomMapExpression == null)
                {
                    pathToLinkedTableProp = dtoColumnName;
                }
                else
                {
                    dynamic expression = propertyMap.CustomMapExpression;
                    pathToLinkedTableProp = ExpressionExtensions.NonExtenionGetExpressionBodyAsString(expression);
                }

                dynamic inst = Activator.CreateInstance(x, pathToLinkedTableProp);
                var prop = inst.GetType().GetProperty("LambdaExpression");
                dynamic exp = inst.LambdaExpression;
                entityColumnNamesExpressions.Add(exp);
                template.ExpressionsOfDtoToEntityColNames.Add(dtoColumnName.ToLower(),exp);

            }
            //Repository.Test(typeOfLinkedTable, entityColumnNamesExpressions);

            //return Repository.OptionsForForeignKey(fkDtoName, templateWithColumnNames, concatenator);
            return Repository.OptionsForForeignKeyTest(typeOfLinkedTable, template);

        }

        public Type GetQueryDtoType()
        {
            return typeof(TQueryDto);
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