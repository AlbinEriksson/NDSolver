using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class Contradiction : ISymbol
    {
        public RuleType[] GetConcludingRuleTypes()
        {
            return new RuleType[]
            {
                RuleType.NOT_Elimination,
                RuleType.Implication_Elimination,
                RuleType.OR_Elimination,
                RuleType.Reiteration,
            };
        }

        public void Accept(ISymbolVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            return obj != null && GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            int result = 37;
            result = result * 397 + GetType().GetHashCode();
            return result;
        }

        public ISymbol ApplyMapping(FormulaMapping mapping)
        {
            return new Contradiction();
        }

        public override string ToString()
        {
            return "⊥";
        }
    }
}
