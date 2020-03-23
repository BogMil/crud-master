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
    public class IIncludeStateTests
    {
        [Fact]
        public void ShouldInheritInterfaces()
        {
            TestUtils.TypeShouldInheritInterfaces(typeof(IIncludeState<>),new List<Type>() {
                typeof(IWhere<>),
                typeof(IPaginate<>),
                typeof(IApplyOrders<>),
                typeof(IGetAll<>)
            });
        }

        [Fact]
        public void ShouldNotInheritInterfaces()
        {
            TestUtils.TypeShouldNotInheritInterfaces(typeof(IIncludeState<>),new List<Type>()
            {
            });
        }
    }
}
