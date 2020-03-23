using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CrudMaster.RecordSelector.States;

namespace CrudMaster.RecordSelector.Operations
{
    public interface IWhere<TEntity>
    {
        IWhereState<TEntity> Where(List<Expression<Func<TEntity, bool>>> predicates);
    }
}