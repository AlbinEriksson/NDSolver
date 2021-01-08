using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    interface ISymbolVisitor
    {
        void Visit(AND and);
        void Visit(Atom atom);
        void Visit(Contradiction contradiction);
        void Visit(Formula formula);
        void Visit(Hypothesis hypothesis);
        void Visit(Implies implies);
        void Visit(NOT not);
        void Visit(OR or);
    }
}
