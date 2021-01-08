using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class Implies : BinaryFunction
    {
        public Implies(ISymbol left, ISymbol right) : base(left, right) { }

        public override RuleType[] GetConcludingRuleTypes()
        {
            return new RuleType[]
            {
                RuleType.Implication_Introduction,
                RuleType.AND_Elimination_Left,
                RuleType.AND_Elimination_Right,
                RuleType.Implication_Elimination,
                RuleType.OR_Elimination,
                RuleType.Contradiction_Elimination,
                RuleType.Double_Negation_Elimination,
                RuleType.Reiteration,
            };
        }

        public override void Accept(ISymbolVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override ISymbol ApplyMapping(FormulaMapping mapping)
        {
            return new Implies(left.ApplyMapping(mapping), right.ApplyMapping(mapping));
        }

        public override string ToString()
        {
            return string.Format("({0} → {1})", left, right);
        }
    }
}
