using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class PremiseStep : IProofStep
    {
        public ISymbol Conclusion { get => premise; }

        public int Start { get => stepNumber; }
        public int End { get => stepNumber; }

        private int stepNumber;
        private readonly ISymbol premise;

        public PremiseStep(int stepNumber, ISymbol premise)
        {
            this.stepNumber = stepNumber;
            this.premise = premise;
        }

        public void Move(int offset)
        {
            stepNumber += offset;
        }

        public void PrintToConsole(string prefix = "")
        {
            Console.WriteLine("{0} (Premise)", premise);
        }
    }
}
