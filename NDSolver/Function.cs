using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    abstract class Function : ISymbol
    {
        public abstract RuleType[] GetConcludingRuleTypes();
        public abstract void Accept(ISymbolVisitor visitor);
        public abstract ISymbol ApplyMapping(FormulaMapping mapping);
    }
}
