using System;
using System.Linq;
using CrudMaster.RecordSelector;
using CrudMaster.RecordSelector.States;
using Microsoft.EntityFrameworkCore;

namespace CrudMaster
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
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
        public virtual void Create(TEntity entity)
        {
            Db.Set<TEntity>().Add(entity);
            Db.SaveChanges();
        }
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