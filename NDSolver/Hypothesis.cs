using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class Hypothesis : ISymbol
    {
        public readonly ISymbol premise, conclusion;

        public Hypothesis(ISymbol premise, ISymbol conclusion)
        {
            this.premise = premise;
            this.conclusion = conclusion;
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

            Hypothesis other = (Hypothesis)obj;
            return premise.Equals(other.premise) && conclusion.Equals(other.conclusion);
        }

        public override int GetHashCode()
        {
            int result = 37;
            result = result * 397 + GetType().GetHashCode();
            result = result * 397 + premise.GetHashCode();
            result = result * 397 + conclusion.GetHashCode();
            return result;
        }

        public ISymbol ApplyMapping(FormulaMapping mapping)
        {
            return new Hypothesis(premise.ApplyMapping(mapping), conclusion.ApplyMapping(mapping));
        }

        public override string ToString()
        {
            return string.Format("({0} ... {1})", premise, conclusion);
        }
    }
}
