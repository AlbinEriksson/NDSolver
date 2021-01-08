using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class Proof : IProofStep
    {
        private List<IProofStep> proofSteps = new List<IProofStep>();
        public ISymbol conclusion;
        private int startStep, endStep;

        public ISymbol Conclusion { get => conclusion; }
        public int Start { get => startStep; }
        public int End { get => endStep; }

        public List<IProofStep> ProofSteps { get => proofSteps; }

        public Proof(int startStep)
        {
            this.startStep = startStep;
            endStep = startStep - 1;
        }

        public Proof(): this(1) { }

        public List<IProofStep> AddPremises(List<ISymbol> premises)
        {
            List<IProofStep> premiseSteps = new List<IProofStep>();

            foreach(ISymbol premise in premises)
            {
                endStep++;
                PremiseStep step = new PremiseStep(endStep, premise);
                proofSteps.Add(step);
                premiseSteps.Add(step);
            }

            return premiseSteps;
        }

        public void AddStepsFromProof(Proof other)
        {
            foreach(IProofStep step in other.proofSteps)
            {
                step.Move(endStep);
                proofSteps.Add(step);
            }
            endStep += other.endStep - other.startStep + 1;
        }

        public void AddRuleStep(RuleStep step)
        {
            endStep++;
            step.stepNumber = endStep;
            proofSteps.Add(step);
        }

        public void AddProofStep(Proof proof)
        {
            proof.Move(endStep);
            proofSteps.Add(proof);
            endStep += proof.End - proof.Start + 1;
        }

        public void Move(int offset)
        {
            startStep += offset;
            endStep += offset;

            foreach(var step in proofSteps)
            {
                step.Move(offset);
            }
        }

        public void PrintToConsole(string prefix = "")
        {
            foreach(var step in proofSteps)
            {
                if(step.Start == step.End)
                {
                    Console.Write("{0}{1}. ", prefix, step.Start);
                }
                else if(!(step is Proof))
                {
                    Console.Write("{0}{1}-{2}. ", prefix, step.Start, step.End);
                }

                if (step is Proof)
                {
                    step.PrintToConsole(prefix + "\t");
                }
                else
                {
                    step.PrintToConsole(prefix);
                }
            }
        }
    }
}
