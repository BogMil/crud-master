﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CrudMaster.Extensions;
using CrudMaster.Sorter;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CrudMaster.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IPagedList<TEntity> Filter(Pager pager, Expression<Func<TEntity, bool>> filters, IOrderByProperties orderByProperties, List<string> includings);
        TEntity Find(int id);
        void Create(TEntity entity);
        TEntity CreateAndReturn(TEntity entity);
        void Update(TEntity entity);
        TEntity UpdateAndReturn(TEntity entity);
        void Delete(int id);
        int DeleteAndReturn(int id);
        Expression<Func<TEntity, bool>> CustomWherePredicate { get; set; }
        TEntity NewDbSet();
        Dictionary<string, string> OptionsForForeignKey(Type linkedTableType, TemplateWithColumnNames template);
        Type GetTypeOfLinkedTableByForeignKeyName(Type typeOfEntity, string fkEntityName);

    }

    public abstract class GenericRepository<TEntity, TContext> :

        IGenericRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        public Expression<Func<TEntity, bool>> CustomWherePredicate { get; set; } = null;
        protected TContext Db { get; private set; }

        protected GenericRepository(TContext context)
        {
            Db = context;
        }

        public TEntity NewDbSet() => Db.CreateProxy<TEntity>();

        public Type GetTypeOfLinkedTableByForeignKeyName(Type typeOfEntity, string fkEntityName)
        {
            var allFks = Db.Model.FindEntityType(typeOfEntity).GetForeignKeys();

            var foundFk = allFks.Single(s => (s.Properties.Select(d => d.Name)).Contains(fkEntityName));
            if (foundFk == null)
                throw new Exception($"{fkEntityName} is not a foreign key of {typeOfEntity.ToString()}");

            var typeOfLinkedTable = foundFk.PrincipalEntityType;
            var clrType = typeOfLinkedTable.ClrType;
            return typeOfEntity.Assembly
                .GetType(clrType.FullName ??
                         throw new InvalidOperationException("ClrType does not have fullName of linked table Type"));
        }

        public Dictionary<string, string> OptionsForForeignKey(Type linkedTableType, TemplateWithColumnNames template)
        { 
            var tableNamesToInclude = new List<string>();
            foreach (var exp in template.ExpressionsOfDtoToEntityColNames.Values)
            {
                var t = exp.Body.NodeType;
                if (t == ExpressionType.Call)
                {
                    var objectNodeType = (exp.Body as MethodCallExpression).Object.NodeType;
                }
                var expString = new ExpressionString(exp.ToString());
                var x=(exp.Body)as Expression;
                if (exp.Body.NodeType == ExpressionType.Call)
                {
                    var handle= (exp.Body as MethodCallExpression).Method.MethodHandle;
                    var xc = MethodBase.GetMethodFromHandle(handle);
                    var a=xc.GetMethodBody();
                }
                foreach (var tableName in expString.TableNamesToInclude)
                {
                    if(!tableNamesToInclude.Contains(tableName))
                        tableNamesToInclude.Add(tableName);
                }
            }
            var pkOfLinkedTable = Db.Model.FindEntityType(linkedTableType).FindPrimaryKey().Properties.Select(y => y.Name).Single();
            //var linkedTableType1 = Db.GetType().Assembly.GetType(linkedTableType.FullName);
            var dbSetOflinkedTable =
                (IQueryable<object>)Db.GetType().GetMethod("Set").MakeGenericMethod(linkedTableType).Invoke(Db, null);

            foreach (var tableName in tableNamesToInclude)
            {
                dbSetOflinkedTable = dbSetOflinkedTable.Include(tableName);
            }

            var res=dbSetOflinkedTable
                //.ToList()
                .Select(x =>
                new KeyValuePair<string, string>(
                    x.GetType().GetProperty(pkOfLinkedTable).GetValue(x).ToString(),
                    template.Replace(x)
                )
            ).ToList();

            return res?.ToDictionary(x => x.Key, x => x.Value);
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

        public virtual IPagedList<TEntity> Filter(Pager pager, Expression<Func<TEntity, bool>> filters, IOrderByProperties orderByProperties,List<string> includings)
        {
            IQueryable<TEntity> listOfEntities = Db.Set<TEntity>();

            listOfEntities = listOfEntities.Include("City.Region");

            var listOfFilteredEntities = filters == null ? listOfEntities : listOfEntities.Where(filters);
            listOfFilteredEntities = ApplyCustomCondition(listOfFilteredEntities);

            if (CustomWherePredicate != null)
                listOfFilteredEntities = listOfFilteredEntities.Where(CustomWherePredicate);

            var listOfOrderedEntities = listOfFilteredEntities;
            if (orderByProperties != null)
            {
                listOfOrderedEntities = orderByProperties.OrderDirection == SortDirections.Ascending
                    ? listOfFilteredEntities.OrderBy(orderByProperties.OrderByProperty)
                    : listOfFilteredEntities.OrderByDescending(orderByProperties.OrderByProperty);
            }

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