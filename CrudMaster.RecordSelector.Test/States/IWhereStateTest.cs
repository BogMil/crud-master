using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrudMaster.RecordSelector.Operations;
using CrudMaster.RecordSelector.States;
using Xunit;

namespace CrudMaster.RecordSelector.Test.States
{
    // ReSharper disable once InconsistentNaming
    public class IWhereStateTest
    {
        [Fact]
        public void ShouldInheritInterfaces()
        {
            TestUtils.TypeShouldInheritInterfaces(typeof(IWhereState<>), new List<Type>
            {
                typeof(IApplyOrders<>),
                typeof(IPaginate<>),
                typeof(IGetAll<>)
            });
        }

        [Fact]
        public void ShouldNotInheritInterfaces()
        {
            TestUtils.TypeShouldNotInheritInterfaces( typeof(IWhereState<>),new List<Type>
            {
                typeof(IInclude<>),
            });
        }
    }
}
