using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CrudMaster.Extensions;
using CrudMaster.RecordSelector;
using CrudMaster.RecordSelector.States;
using CrudMaster.Sorter;
using CrudMaster.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using X.PagedList;

namespace CrudMaster
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IPagedList<TEntity> Filter(Pager pager, Expression<Func<TEntity, bool>> filters, IOrderByProperties orderByProperties, IEnumerable<string> includings);
        TEntity Find(int id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        TEntity GetNewDbSet();
        IRecordSelectorInitialState<TEntity> RecordSelector();
    }

   

    public abstract class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected TContext Db { get; private set; }
        protected GenericRepository(TContext context)
        {
            Db = context;
        }

        public TEntity GetNewDbSet() => Db.CreateProxy<TEntity>();
        public IRecordSelectorInitialState<TEntity> RecordSelector() => new RecordSelector<TEntity>(Db);

        public TEntity Find(int id) => Db.Set<TEntity>().Find(id);

        public virtual void Delete(int id)
        {
            var entity = Find(id);
            Db.Set<TEntity>().Remove(entity ?? throw new Exception($"Database is missing record with id:{id}"));
            Db.SaveChanges();
        }

        public virtual IPagedList<TEntity> Filter(Pager pager, Expression<Func<TEntity, bool>> filters,
            IOrderByProperties orderByProperties, IEnumerable<string> includings)
        {
            IQueryable<TEntity> listOfEntities = Db.Set<TEntity>();
            //include
            listOfEntities = includings.Aggregate(listOfEntities, (current, including) => current.Include(including));
            

            //aply filters
            var listOfFilteredEntities = filters == null ? listOfEntities : listOfEntities.Where(filters);

            //orderBy
            var listOfOrderedEntities = listOfFilteredEntities;
            if (orderByProperties != null)
            {
                listOfOrderedEntities = orderByProperties.OrderDirection == SortDirections.Ascending
                    ? listOfFilteredEntities.OrderBy(orderByProperties.OrderByProperty)
                    : listOfFilteredEntities.OrderByDescending(orderByProperties.OrderByProperty);
            }


            //page
            var pagedList = Paged(listOfOrderedEntities, pager);
            
            return pagedList;
        }


        protected IPagedList<TEntity> Paged(IQueryable<TEntity> listOfEntities, Pager pager)
        {
            if (pager.NumOfRowsPerPage < 0)
                return listOfEntities.ToPagedList();

            return listOfEntities.ToPagedList(pager.CurrentPageNumber, pager.NumOfRowsPerPage);
        }
        
        public virtual void Create(TEntity entity)
        {
            Db.Set<TEntity>().Add(entity);
            Db.SaveChanges();
        }

        protected virtual TEntity ModifyEntityBeforeCreate(TEntity entity) => entity;

        public virtual void Update(TEntity entity)
        {
            var id = GetPKValue(entity);
            var oldEntity = Db.Set<TEntity>().Find(id);
            Db.Entry(oldEntity).CurrentValues.SetValues(entity);
            Db.SaveChanges();
        }

        // ReSharper disable once InconsistentNaming
        public virtual object GetPKValue(TEntity entity)
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