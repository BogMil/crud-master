using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CrudMaster.Extensions;
using CrudMaster.Filter;
using CrudMaster.Repository;
using CrudMaster.Sorter;
using X.PagedList;
using ExpressionBuilder.Generics;

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
        protected IMappingService MappingService { get; }
        protected GenericService(TRepository repository)
        {
            Repository = repository;
            MappingService = new MappingService();
        }

        public virtual StaticPagedList<TQueryDto> GetJqGridDataTest(Pager pager, string filters, OrderByProperties orderByProperties)
        {
            var wherePredicate = FilterFactory.Create<TEntity, TQueryDto>(filters);
            var orderBy = new GenericOrderByPredicateCreator<TEntity, TQueryDto>().GetPropertyObject(orderByProperties);
            var entities = Repository.Filter(pager, wherePredicate, orderBy);

            return MappingService.MapToStaticPageList<TEntity,TQueryDto>(entities);
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
                    var exp = MappingService.GetPropertyMappingExpression(dtoColumnName, queryDtoOfLinkedTable, typeOfLinkedTable);
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
            MappingService.Mapper.Map(dto, entity);

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