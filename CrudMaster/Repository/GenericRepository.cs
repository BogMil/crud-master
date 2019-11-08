using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using AutoMapper;
using CrudMaster.Extensions;
using CrudMaster.Filter;
using CrudMaster.PropertyMapper;
using CrudMaster.Service;
using CrudMaster.Sorter;
using ExpressionBuilder.Generics;
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

        protected IMapper Mapper;
        protected GenericRepository(TContext context, IMapper mapper)
        {
            Db = context;
            Mapper = mapper;
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

        public virtual IPagedList<TEntity> Filter(Pager pager, Filter<TEntity> filters, OrderByProperties orderByProperties)
        {
            var orderBy = new TOrderByPredicateCreator().GetPropertyObject(orderByProperties);

            IQueryable<TEntity> listOfEntities = Db.Set<TEntity>();

            var listOfFilteredEntities = filters == null ? listOfEntities : listOfEntities.Where(filters);
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