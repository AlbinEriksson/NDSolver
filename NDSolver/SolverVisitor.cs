using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class SolverVisitor : ISymbolVisitor
    {
        private readonly Solver solver;
        public Proof Result { get; private set; }

        public SolverVisitor(Solver solver)
        {
            this.solver = solver;
        }

        public void Visit(AND and)
        {
            Sequent sequent = new Sequent(
                new ISymbol[0],
                and
            );

            Result = solver.Prove(sequent);
        }

        public void Visit(Atom atom)
        {
            Sequent sequent = new Sequent(
                new ISymbol[0],
                atom
            );

            Result = solver.Prove(sequent);
        }

        public void Visit(Contradiction contradiction)
        {
            Sequent sequent = new Sequent(
                new ISymbol[0],
                contradiction
            );

            Result = solver.Prove(sequent);
        }

        public void Visit(Formula formula)
        {
            throw new NotImplementedException();
        }

        public void Visit(Hypothesis hypothesis)
        {
            Sequent sequent = new Sequent(
                new ISymbol[] { hypothesis.premise },
                hypothesis.conclusion
            );

            Proof subproof = solver.Prove(sequent);
            if(subproof != null)
            {
                Result = new Proof();
                Result.AddProofStep(subproof);
            }
        }

        public void Visit(Implies implies)
        {
            Sequent sequent = new Sequent(
                new ISymbol[0],
                implies
            );

            Result = solver.Prove(sequent);
        }

        public void Visit(NOT not)
        {
            Sequent sequent = new Sequent(
                new ISymbol[0],
                not
            );

            Result = solver.Prove(sequent);
        }

        public void Visit(OR or)
        {
            Sequent sequent = new Sequent(
                new ISymbol[0],
                or
            );

            Result = solver.Prove(sequent);
        }
    }
}
