using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class Formula : ISymbol
    {
        public readonly int formulaIndex;

        public Formula(int formulaIndex)
        {
            this.formulaIndex = formulaIndex;
        }

        public RuleType[] GetConcludingRuleTypes()
        {
            return new RuleType[0];
        }

        public void Accept(ISymbolVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Formula other = (Formula)obj;
            return formulaIndex == other.formulaIndex;
        }

        public override int GetHashCode()
        {
            int result = 37;
            result = result * 397 + GetType().GetHashCode();
            result = result * 397 + formulaIndex.GetHashCode();
            return result;
        }

        public ISymbol ApplyMapping(FormulaMapping mapping)
        {
            return mapping.GetMapping(this);
        }

        public override string ToString()
        {
            switch(formulaIndex)
            {
                case 0: return "α";
                case 1: return "β";
                case 2: return "χ";
            }

            throw new NotImplementedException("Use of formula index greater than 2.");
        }
    }
}
