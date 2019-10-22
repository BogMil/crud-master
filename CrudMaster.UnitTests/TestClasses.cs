using System;
using System.Collections.Generic;
using System.Text;

namespace CrudMaster.UnitTests
{
    public class TestClass
    {
        public int someInt { get; set; }
        public NestedTestClass nestedClass { get; set; }
    }


    public class NestedTestClass
    {
        public NestedTestClassLvl2 nestedTestClassLvl2 { get; set; }
    }

    public class NestedTestClassLvl2
    {
        public int someIntInLvl2 { get; set; }
    }
}
