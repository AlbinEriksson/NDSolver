using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    internal class SymbolProgress
    {
        public ISymbol symbol;
        public int step = 0;

        internal SymbolProgress(ISymbol symbol)
        {
            this.symbol = symbol;
        }
    }

    class SymbolEnumerator : IEnumerator<ISymbol>, ISymbolVisitor
    {
        private readonly ISymbol root;
        private Stack<SymbolProgress> symbolStack;

        public ISymbol Current => symbolStack.Peek().symbol;

        object IEnumerator.Current => Current;

        public SymbolEnumerator(ISymbol root)
        {
            this.root = root;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if(symbolStack == null)
            {
                symbolStack = new Stack<SymbolProgress>();
                symbolStack.Push(new SymbolProgress(root));
                return true;
            }

            VisitTop();
            return symbolStack.Count > 0;
        }

        public void EscapeBranch()
        {
            symbolStack.Pop();
        }

        private void VisitTop()
        {
            if(symbolStack.Count == 0)
            {
                return;
            }

            SymbolProgress progress = symbolStack.Peek();
            progress.symbol.Accept(this);
            progress.step++;
        }

        public void Reset()
        {
            symbolStack = null;
        }

        private void VisitBinaryFunction(BinaryFunction function)
        {
            int step = symbolStack.Peek().step;
            switch (step)
            {
                case 0:
                    symbolStack.Push(new SymbolProgress(function.left));
                    break;
                case 1:
                    symbolStack.Push(new SymbolProgress(function.right));
                    break;
                case 2:
                    symbolStack.Pop();
                    VisitTop();
                    break;
            }
        }

        private void VisitUnaryFunction(UnaryFunction function)
        {
            int step = symbolStack.Peek().step;
            switch (step)
            {
                case 0:
                    symbolStack.Push(new SymbolProgress(function.operand));
                    break;
                case 1:
                    symbolStack.Pop();
                    VisitTop();
                    break;
            }
        }

        private void VisitLeaf(ISymbol symbol)
        {
            int step = symbolStack.Peek().step;
            switch (step)
            {
                case 0:
                    symbolStack.Pop();
                    VisitTop();
                    break;
            }
        }

        public void Visit(AND and)
        {
            VisitBinaryFunction(and);
        }

        public void Visit(Atom atom)
        {
            VisitLeaf(atom);
        }

        public void Visit(Contradiction contradiction)
        {
            VisitLeaf(contradiction);
        }

        public void Visit(Formula formula)
        {
            VisitLeaf(formula);
        }

        public void Visit(Hypothesis hypothesis)
        {
            VisitLeaf(hypothesis);
        }

        public void Visit(Implies implies)
        {
            VisitBinaryFunction(implies);
        }

        public void Visit(NOT not)
        {
            VisitUnaryFunction(not);
        }

        public void Visit(OR or)
        {
            VisitBinaryFunction(or);
        }
    }
}
