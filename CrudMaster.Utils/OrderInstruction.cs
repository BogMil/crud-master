using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CrudMaster.Utils
{
    public class OrderInstruction
    {
        public LambdaExpression LambdaExpression{ get; set; }
        public string Direction { get; set; }

        public OrderInstruction(LambdaExpression lambdaExpression, string direction=OrderDirections.Ascending)
        {
            LambdaExpression = lambdaExpression;
            Direction = direction;
        }
    }
}
