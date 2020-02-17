using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CrudMaster.Sorter;
using X.PagedList;
using ExpressionBuilder.Generics;


namespace CrudMaster.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        //IEnumerable<TEntity> All(Pager pager);

        //IPagedList<TEntity> Filter(Pager pager, string filters, OrderByProperties orderByProperties);
        IPagedList<TEntity> Filter(Pager pager, Filter<TEntity> filters, IOrderByProperties orderByProperties);

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

        //dynamic Test(dynamic entityType,LambdaExpression exps);

    }
}
