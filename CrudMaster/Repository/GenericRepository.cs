﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CrudMaster.Filter;
using CrudMaster.PropertyMapper;
using CrudMaster.Sorter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using X.PagedList;

namespace CrudMaster.Repository
{
    public abstract class GenericRepository<TEntity, TContext, TOrderByPredicateCreator, TFilterPredicateCreator> :

        IGenericRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
        where TOrderByPredicateCreator : IOrderByPredicateCreator<TEntity>, new()
        where TFilterPredicateCreator : IWherePredicateCreator<TEntity>, new()
    {
        public Expression<Func<TEntity, bool>> CustomWherePredicate { get; set; } = null;
        protected TContext Db { get; private set; }


        protected GenericRepository(TContext context)
        {
            Db = context;
        }

        public TEntity NewDbSet() => Db.CreateProxy<TEntity>();
        public Dictionary<string, string> OptionsForForeignKey(string fkDto, string[] colNames,string concatenator)
        {
            var assemblyOfDb = Db.GetType().Assembly;
            var typesThatImplementsPropertyMapperInterfaceOfTEntity =
                assemblyOfDb.GetTypesThatImplements(typeof(IPropertyMapper<>), new List<Type>() { typeof(TEntity) });

            IPropertyMapper<TEntity> proppertyMapper;
            if (typesThatImplementsPropertyMapperInterfaceOfTEntity.Count == 1)
                proppertyMapper = (IPropertyMapper<TEntity>)Activator.CreateInstance(typesThatImplementsPropertyMapperInterfaceOfTEntity.First());
            else
                throw new Exception("PropertyMapper not implemented for entity" + typeof(TEntity));


            var fkExpression = proppertyMapper.GetCorespondingPropertyNavigationInEntityForDtoField(fkDto);
            var fkAsString = fkExpression.GetExpressionBodyAsString();

            
            

            var allFks = Db.Model.FindEntityType(typeof(TEntity)).GetForeignKeys();

            var fkEntity = allFks.Single(s => s.Properties.Select(d => d.Name).Contains(fkAsString));
            var nameOfLinkedTable = fkEntity.PrincipalEntityType.Name;


            var linkedTableType = assemblyOfDb.GetType(nameOfLinkedTable);
            var dbSetOflinkedTable = (IQueryable<object>)Db.GetType().GetMethod("Set").MakeGenericMethod(linkedTableType).Invoke(Db, null);

            var pkOfLinkedTable = Db.Model.FindEntityType(linkedTableType).FindPrimaryKey().Properties.Select(y => y.Name).Single();

            dynamic linkedTableProppertyMapper;
            var z = typeof(IPropertyMapper<>).MakeGenericType(linkedTableType);

            var typesThatImplementsPropertyMapperInterfaceOfRelatedTEntity =
                assemblyOfDb.GetTypesThatImplements(typeof(IPropertyMapper<>), new List<Type>() { linkedTableType });

            if (typesThatImplementsPropertyMapperInterfaceOfRelatedTEntity.Count == 1)
                linkedTableProppertyMapper = Activator.CreateInstance(typesThatImplementsPropertyMapperInterfaceOfRelatedTEntity.First());
            else
                throw new Exception("PropertyMapper not implemented for entity" + typeof(TEntity));

            
            var i = 0;
            var mappedColnamesAsStrings = new string[colNames.Length];

            //var x = typesThatImplementsPropertyMapperInterfaceOfRelatedTEntity.First().MakeGenericType(linkedTableType);
            
            foreach (var colName in colNames)
            {
                var colNameExpression = linkedTableProppertyMapper.GetCorespondingPropertyNavigationInEntityForDtoField(colName);
                var colNameAsString = ExpressionExtensions.NonExtenionGetExpressionBodyAsString(colNameExpression);
                mappedColnamesAsStrings[i] = colNameAsString;
                i++;
            }

            var res = dbSetOflinkedTable.Select(x =>
                new KeyValuePair<string, string>(
                    x.GetType().GetProperty(pkOfLinkedTable).GetValue(x).ToString(),
                    //x.GetType().GetProperty(colNameAsString).GetValue(x).ToString()
                    ConcatColValues(x, mappedColnamesAsStrings, concatenator)
                )
            ).ToList();
            return res?.ToDictionary(x=>x.Key,x=>x.Value);
        }

        private static string ConcatColValues(object entity, IReadOnlyCollection<string> colNames, string concatenator)
        {
            var colValues=new string[colNames.Count];
            var i = 0;
            foreach (var colName in colNames)
            {
                colValues[i] = entity.GetType().GetProperty(colName)?.GetValue(entity).ToString();
                i++;
            }

            return colValues.Join(concatenator);
        }

        public TEntity Find(int id) => Db.Set<TEntity>().Find(id);

        public virtual int DeleteAndReturn(int id)
        {
            Delete(id);
            return id;
        }

        public virtual void Delete(int id)
        {
            var entity = Find(id);
            ShouldDeleteEntity(entity);
            Db.Set<TEntity>().Remove(entity ?? throw new Exception($"Database is missing record with id:{id}"));
            Db.SaveChanges();
        }

        public virtual IPagedList<TEntity> Filter(Pager pager, string filters, OrderByProperties orderByProperties)
        {
            var orderBy = new TOrderByPredicateCreator().GetPropertyObject(orderByProperties);
            var filterPredicate = new TFilterPredicateCreator().GetWherePredicate(filters);

            IQueryable<TEntity> listOfEntities = Db.Set<TEntity>();

            var listOfFilteredEntities = filterPredicate == null ? listOfEntities : listOfEntities.Where(filterPredicate);
            listOfFilteredEntities = ApplyCustomCondition(listOfFilteredEntities);

            if (CustomWherePredicate != null)
                listOfFilteredEntities = listOfFilteredEntities.Where(CustomWherePredicate);

            var listOfOrderedEntities = orderByProperties.OrderDirection == SortDirections.Ascending
                ? listOfFilteredEntities.OrderBy(orderBy.OrderByProperty)
                : listOfFilteredEntities.OrderByDescending(orderBy.OrderByProperty);

            var pagedList = Paged(listOfOrderedEntities, pager);
            return pagedList;
        }



        protected virtual IQueryable<TEntity> ApplyCustomCondition(IQueryable<TEntity> listOfFilteredEntities) => listOfFilteredEntities;

        protected IPagedList<TEntity> Paged(IEnumerable<TEntity> listOfEntities, Pager pager)
        {
            if (pager.NumOfRowsPerPage < 0)
                return listOfEntities.ToPagedList();

            return listOfEntities.ToPagedList(pager.CurrentPageNumber, pager.NumOfRowsPerPage);
        }

        public virtual void Create(TEntity entity)
        {
            entity = ModifyEntityBeforeCreate(entity);
            Db.Set<TEntity>().Add(entity);
            Db.SaveChanges();
        }

        public virtual TEntity CreateAndReturn(TEntity entity)
        {
            Create(entity);
            return entity;
        }

        protected virtual TEntity ModifyEntityBeforeCreate(TEntity entity) => entity;

        public virtual void Update(TEntity entity)
        {
            var id = GetPrimaryKeyValue(entity);
            var oldEntity = Db.Set<TEntity>().Find(id);
            entity = ModifyUpdateSourceEntityBeforeUpdate(entity, oldEntity);
            Db.Entry(oldEntity).CurrentValues.SetValues(entity);
            Db.SaveChanges();
        }

        public virtual TEntity UpdateAndReturn(TEntity entity)
        {
            var id = GetPrimaryKeyValue(entity);
            var oldEntity = Db.Set<TEntity>().Find(id);
            entity = ModifyUpdateSourceEntityBeforeUpdate(entity, oldEntity);
            Db.Entry(oldEntity).CurrentValues.SetValues(entity);
            Db.SaveChanges();

            return oldEntity;

        }

        protected virtual TEntity ModifyUpdateSourceEntityBeforeUpdate(TEntity updateSourceEntity, TEntity oldEntity) => updateSourceEntity;

        protected virtual void ShouldDeleteEntity(TEntity entity) { }

        public virtual object GetPrimaryKeyValue(TEntity entity)
        {
            var pkName = Db.Entry(entity)
                .Metadata
                .FindPrimaryKey()
                .Properties
                .Select(x => x.Name)
                .ToList()
                .First()
                .ToString();


            return entity.GetType().GetProperty(pkName)?.GetValue(entity);
        }
    }
}