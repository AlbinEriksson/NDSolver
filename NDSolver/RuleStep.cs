using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class RuleStep : IProofStep
    {
        public ISymbol Conclusion { get => conclusion; }
        public int Start { get => stepNumber; }
        public int End { get => stepNumber; }

        public RuleType ruleType;
        public List<IProofStep> referredSteps = new List<IProofStep>();
        public int stepNumber;
        public ISymbol conclusion;

        public RuleStep(int stepNumber, ISymbol conclusion, RuleType ruleType)
        {
            this.stepNumber = stepNumber;
            this.conclusion = conclusion;
            this.ruleType = ruleType;
        }

        public void AddReferredStep(IProofStep step)
        {
            referredSteps.Add(step);
        }

        public void PopReferredStep()
        {
            referredSteps.RemoveAt(referredSteps.Count - 1);
        }

        public void Move(int offset)
        {
            stepNumber += offset;
        }

        public void PrintToConsole(string prefix = "")
        {
            Console.Write("{0} ({1} ", conclusion, InferenceRule.TypeToString(ruleType));

            for (int i = 0; i < referredSteps.Count; i++)
            {
                if (i > 0)
                {
                    Console.Write(", ");
                }

                var step = referredSteps[i];

                if (step.Start == step.End)
                {
                    Console.Write("{0}", step.Start);
                }
                else
                {
                    Console.Write("{0}-{1}", step.Start, step.End);
                }
            }

            Console.WriteLine(")");
        }
    }
}
