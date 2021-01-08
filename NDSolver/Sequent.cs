using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class Sequent
    {
        public readonly List<ISymbol> premises;
        public readonly ISymbol conclusion;

        public Sequent(List<ISymbol> premises, ISymbol conclusion)
        {
            this.premises = premises;
            this.conclusion = conclusion;
        }

        public Sequent(ISymbol[] premises, ISymbol conclusion)
        {
            this.premises = new List<ISymbol>(premises);
            this.conclusion = conclusion;
        }

        public override string ToString()
        {
            string premisesStr = "";
            for(int i = 0; i < premises.Count; i++)
            {
                if(i > 0)
                {
                    premisesStr += ", ";
                }

                premisesStr += premises[i].ToString();
            }

            return premisesStr + " ⊢ " + conclusion.ToString();
        }
    }
}
