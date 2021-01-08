using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class MappingResult
    {
        public readonly ISymbol formula;
        public readonly FormulaMapping mapping;
        public readonly bool isRoot;

        public MappingResult(ISymbol formula, FormulaMapping mapping, bool isRoot)
        {
            this.formula = formula;
            this.mapping = mapping;
            this.isRoot = isRoot;
        }
    }
}
