using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    abstract class UnaryFunction : Function
    {
        public readonly ISymbol operand;

        public UnaryFunction(ISymbol operand)
        {
            this.operand = operand;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            UnaryFunction other = (UnaryFunction)obj;
            return operand.Equals(other.operand);
        }

        public override int GetHashCode()
        {
            int result = 37;
            result = result * 397 + GetType().GetHashCode();
            result = result * 397 + operand.GetHashCode();
            return result;
        }
    }
}
