﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CrudMaster.RecordSelector.Operations;
using CrudMaster.RecordSelector.States;
using CrudMaster.Utils;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CrudMaster.RecordSelector
{
    public interface IRecordSelector<TEntity> :
        IRecordSelectorInitialState<TEntity>,
        IIncludeState<TEntity>,
        IWhereState<TEntity>,
        IApplyOrdersState<TEntity>,
        IExecuteState<TEntity>
        where TEntity : class
    { }
    /// <summary>
    /// RecordSelector should provide fluent interface to encapsulate
    /// flow of awailable operations over DbSet
    /// </summary>
    /// <typeparam name="TEntity">
    /// EF Core Entity type
    /// </typeparam>
    public class RecordSelector<TEntity> :
        IRecordSelector<TEntity> where TEntity : class
    {
        private IQueryable<TEntity> _selectionResult;
        public RecordSelector(DbContext db)
        {
            _selectionResult = db.Set<TEntity>();
        }

        public IPagedList<TEntity> GetAll()
            => _selectionResult.ToPagedList();

        public IIncludeState<TEntity> Include(IEnumerable<string> includings)
        {
            _selectionResult = includings
                .Aggregate(_selectionResult, (current, including) => current.Include(including));
            return this;
        }

        public IWhereState<TEntity> Where(List<Expression<Func<TEntity, bool>>> predicates)
        {
            predicates.ForEach(
                predicate => _selectionResult = _selectionResult.Where(predicate)
            );
            return this;
        }

        public IPagedList<TEntity> Paginate(Pager pager)
            => _selectionResult.ToPagedList(pager.CurrentPageNumber, pager.NumOfRowsPerPage);

        /// <summary>
        /// Apply ordering of every instruction
        /// Odrdering will be applied in order of instructions
        /// </summary>
        /// <param name="orderByInstructions"></param>
        /// <returns></returns>
        public IApplyOrdersState<TEntity> ApplyOrders(List<OrderInstruction> orderByInstructions)
        {
            var instructionsWithAppliers = OrderInstructionWithAppliersFactory.Create<TEntity>(orderByInstructions).ToList();
            _selectionResult = 
                instructionsWithAppliers.Aggregate(_selectionResult, (current, instruction) =>
                    instructionsWithAppliers.IsFirst(instruction)
                        ? instruction.ApplyOrdering(current)
                        : instruction.ApplyOrdering((IOrderedQueryable<TEntity>)current));

            return this;
        }
    }
}
