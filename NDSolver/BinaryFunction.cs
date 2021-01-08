using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    abstract class BinaryFunction : Function
    {
        public readonly ISymbol left, right;

        public BinaryFunction(ISymbol left, ISymbol right)
        {
            this.left = left;
            this.right = right;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            BinaryFunction other = (BinaryFunction)obj;
            return left.Equals(other.left) && right.Equals(other.right);
        }

        public override int GetHashCode()
        {
            int result = 37;
            result = result * 397 + GetType().GetHashCode();
            result = result * 397 + left.GetHashCode();
            result = result * 397 + right.GetHashCode();
            return result;
        }
    }
}
