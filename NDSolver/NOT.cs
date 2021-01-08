using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class NOT : UnaryFunction
    {
        public NOT(ISymbol operand) : base(operand) { }

        public override RuleType[] GetConcludingRuleTypes()
        {
            return new RuleType[]
            {
                RuleType.NOT_Introduction,
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
            return new NOT(operand.ApplyMapping(mapping));
        }

        public override string ToString()
        {
            return string.Format("(¬{0})", operand);
        }
    }
}
