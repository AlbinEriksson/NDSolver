using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    interface ISymbol
    {
        public RuleType[] GetConcludingRuleTypes();
        public void Accept(ISymbolVisitor visitor);
        public ISymbol ApplyMapping(FormulaMapping mapping);
    }
}
