using System;
using System.Collections.Generic;
using CrudMaster.RecordSelector.Operations;
using CrudMaster.RecordSelector.States;
using Xunit;

namespace CrudMaster.RecordSelector.Test.States
{
    // ReSharper disable once InconsistentNaming
    public class IRecodSelectorInitialStateTests
    {
        [Fact]
        public void ShouldInheritInterfaces()
        {
            TestUtils.TypeShouldInheritInterfaces(typeof(IRecordSelectorInitialState<>), new List<Type>
            {
                typeof(IInclude<>),
                typeof(IWhere<>),
                typeof(IApplyOrders<>),
                typeof(IPaginate<>),
                typeof(IGetAll<>)
            });
        }

        [Fact]
        public void ShouldNotInheritInterfaces()
        {
            TestUtils.TypeShouldNotInheritInterfaces(typeof(IRecordSelectorInitialState<>), new List<Type>
            {
            });
        }
    }
}
