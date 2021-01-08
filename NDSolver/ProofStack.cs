using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class ProofStackItem
    {
        public readonly ISymbol conclusion;
        public readonly int depth;

        public ProofStackItem(ISymbol conclusion, int depth)
        {
            this.conclusion = conclusion;
            this.depth = depth;
        }
    }

    class ProofStack
    {
        private Stack<ProofStackItem> stack = new Stack<ProofStackItem>();

        public void Push(ISymbol conclusion, int depth)
        {
            stack.Push(new ProofStackItem(conclusion, depth));
        }

        public void Pop()
        {
            stack.Pop();
        }

        public bool Contains(ISymbol conclusion, int depth)
        {
            return stack.Contains(new ProofStackItem(conclusion, depth));
        }
    }
}
