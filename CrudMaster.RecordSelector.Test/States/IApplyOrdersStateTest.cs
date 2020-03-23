using System;
using System.Collections.Generic;
using System.Text;
using CrudMaster.RecordSelector.Operations;
using CrudMaster.RecordSelector.States;
using Xunit;

namespace CrudMaster.RecordSelector.Test.States
{
    // ReSharper disable once InconsistentNaming
    public class IApplyOrdersStateTest
    {
        [Fact]
        public static void ShouldInheritInterfaces()
        {
            TestUtils.TypeShouldInheritInterfaces(typeof(IApplyOrdersState<>),new List<Type>()
            {
                typeof(IPaginate<>),
                typeof(IGetAll<>),
            });
        }

        [Fact]
        public static void ShouldNotInheritInterfaces()
        {
            TestUtils.TypeShouldNotInheritInterfaces(typeof(IApplyOrdersState<>), new List<Type>()
            {
                typeof(IInclude<>),
                typeof(IWhere<>),
            });
        }
    }
}
