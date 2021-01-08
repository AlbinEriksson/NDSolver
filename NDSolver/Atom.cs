using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class Atom : ISymbol
    {
        public readonly int variableIndex;

        public Atom(int variableIndex)
        {
            this.variableIndex = variableIndex;
        }

        public RuleType[] GetConcludingRuleTypes()
        {
            return new RuleType[]
            {
                RuleType.AND_Elimination_Left,
                RuleType.AND_Elimination_Right,
                RuleType.Implication_Elimination,
                RuleType.OR_Elimination,
                RuleType.Contradiction_Elimination,
                RuleType.Double_Negation_Elimination,
                RuleType.Reiteration,
            };
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

            Atom other = (Atom)obj;
            return variableIndex == other.variableIndex;
        }

        public override int GetHashCode()
        {
            int result = 37;
            result = result * 397 + GetType().GetHashCode();
            result = result * 397 + variableIndex.GetHashCode();
            return result;
        }

        public ISymbol ApplyMapping(FormulaMapping mapping)
        {
            return new Atom(variableIndex);
        }

        public override string ToString()
        {
            if(variableIndex <= 10)
            {
                return new string((char)('p' + variableIndex), 1);
            }

            if(variableIndex <= 21)
            {
                return new string((char)('P' + variableIndex), 1);
            }

            return string.Format("v_{0}", variableIndex);
        }
    }
}
