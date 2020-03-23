using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CrudMaster.RecordSelector.Test
{
    public static class TestUtils
    {
        public static void TypeShouldInheritInterfaces(Type type, List<Type> interfacesToInherit)
        {
            var interfacesAsStringToInherit
                =interfacesToInherit
                    .Select(i => i.Name)
                    .ToList();

            var inheritedInterfaces
                = type
                    .GetInterfaces()
                    .Select(i => i.Name)
                    .ToList();

            interfacesAsStringToInherit.ForEach(
                (interfaceToInherit)
                    => Assert.Contains(interfaceToInherit, inheritedInterfaces));
        }

        public static void TypeShouldNotInheritInterfaces(Type type, List<Type> interfacesNotToInherit)
        {
            var interfacesAsStringNotToInherit
                = interfacesNotToInherit
                    .Select(i => i.Name)
                    .ToList();

            var inheritedInterfaces
                = type
                    .GetInterfaces()
                    .Select(i => i.Name)
                    .ToList();

            interfacesAsStringNotToInherit.ForEach(
                (interfaceNotToInherit)
                    => Assert.DoesNotContain(interfaceNotToInherit, inheritedInterfaces));
        }
    }
}
