using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    interface IProofStep
    {
        public ISymbol Conclusion { get; }
        public int Start { get; }
        public int End { get; }

        public void Move(int offset);
        public void PrintToConsole(string prefix = "");
    }
}
