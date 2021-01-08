using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    enum RuleType
    {
        AND_Introduction, AND_Elimination_Left, AND_Elimination_Right,
        Implication_Introduction, Implication_Elimination,
        OR_Introduction_Left, OR_Introduction_Right, OR_Elimination,
        NOT_Introduction, NOT_Elimination,
        Contradiction_Elimination,
        Double_Negation_Elimination,
        Reiteration
    }

    class InferenceRule
    {
        public static string TypeToString(RuleType type)
        {
            switch(type)
            {
                case RuleType.AND_Introduction: return "∧i";
                case RuleType.AND_Elimination_Left: return "∧e₁";
                case RuleType.AND_Elimination_Right: return "∧e₂";
                case RuleType.Implication_Introduction: return "→i";
                case RuleType.Implication_Elimination: return "→e";
                case RuleType.OR_Introduction_Left: return "∨i₁";
                case RuleType.OR_Introduction_Right: return "∨i₂";
                case RuleType.OR_Elimination: return "∨e";
                case RuleType.NOT_Introduction: return "¬i";
                case RuleType.NOT_Elimination: return "¬e";
                case RuleType.Contradiction_Elimination: return "⊥e";
                case RuleType.Double_Negation_Elimination: return "¬¬e";
                case RuleType.Reiteration: return "copy";
            }

            throw new NotImplementedException("Invalid rule type to convert to string");
        }

        public static readonly InferenceRule andIntroduction = new InferenceRule(
           RuleType.AND_Introduction,
           new List<ISymbol> { new Formula(0), new Formula(1) },
           new AND(new Formula(0), new Formula(1)),
           2);

        public static readonly InferenceRule andEliminationLeft = new InferenceRule(
           RuleType.AND_Elimination_Left,
           new List<ISymbol> { new AND(new Formula(0), new Formula(1)) },
           new Formula(0),
           2);

        public static readonly InferenceRule andEliminationRight = new InferenceRule(
           RuleType.AND_Elimination_Right,
           new List<ISymbol> { new AND(new Formula(0), new Formula(1)) },
           new Formula(1),
           2);

        public static readonly InferenceRule implicationIntroduction = new InferenceRule(
           RuleType.Implication_Introduction,
           new List<ISymbol> { new Hypothesis(new Formula(0), new Formula(1)) },
           new Implies(new Formula(0), new Formula(1)),
           2);

        public static readonly InferenceRule implicationElimination = new InferenceRule(
           RuleType.Implication_Elimination,
           new List<ISymbol> { new Implies(new Formula(0), new Formula(1)), new Formula(0) },
           new Formula(1),
           2);

        public static readonly InferenceRule orIntroductionLeft = new InferenceRule(
           RuleType.OR_Introduction_Left,
           new List<ISymbol> { new Formula(0) },
           new OR(new Formula(0), new Formula(1)),
           2);

        public static readonly InferenceRule orIntroductionRight = new InferenceRule(
           RuleType.OR_Introduction_Right,
           new List<ISymbol> { new Formula(1) },
           new OR(new Formula(0), new Formula(1)),
           2);

        public static readonly InferenceRule orElimination = new InferenceRule(
           RuleType.OR_Elimination,
           new List<ISymbol> {
               new OR(new Formula(0), new Formula(1)),
               new Hypothesis(new Formula(0), new Formula(2)),
               new Hypothesis(new Formula(1), new Formula(2))
           },
           new Formula(2),
           3);

        public static readonly InferenceRule notIntroduction = new InferenceRule(
           RuleType.NOT_Introduction,
           new List<ISymbol> { new Hypothesis(new Formula(0), new Contradiction()) },
           new NOT(new Formula(0)),
           1);

        public static readonly InferenceRule notElimination = new InferenceRule(
           RuleType.NOT_Elimination,
           new List<ISymbol> { new Formula(0), new NOT(new Formula(0)) },
           new Contradiction(),
           1);

        public static readonly InferenceRule contradictionElimination = new InferenceRule(
           RuleType.Contradiction_Elimination,
           new List<ISymbol> { new Contradiction() },
           new Formula(0),
           1);

        public static readonly InferenceRule doubleNegationElimination = new InferenceRule(
           RuleType.Double_Negation_Elimination,
           new List<ISymbol> { new NOT(new NOT(new Formula(0))) },
           new Formula(0),
           1);

        public static readonly InferenceRule reiteration = new InferenceRule(
           RuleType.Reiteration,
           new List<ISymbol> { new Formula(0) },
           new Formula(0),
           1);

        public readonly RuleType type;
        public readonly List<ISymbol> premises;
        public readonly ISymbol conclusion;
        public readonly int maxFormulaIndex;

        public InferenceRule(RuleType type, List<ISymbol> premises, ISymbol conclusion, int maxFormulaIndex)
        {
            this.type = type;
            this.premises = premises;
            this.conclusion = conclusion;
            this.maxFormulaIndex = maxFormulaIndex;
        }

        public static Dictionary<RuleType, InferenceRule> AllNonDerived()
        {
            List<InferenceRule> rules = new List<InferenceRule>
            {
                andIntroduction, andEliminationLeft, andEliminationRight,
                implicationIntroduction, implicationElimination,
                orIntroductionLeft, orIntroductionRight, orElimination,
                notIntroduction, notElimination,
                contradictionElimination,
                doubleNegationElimination,
                reiteration
            };

            Dictionary<RuleType, InferenceRule> dict = new Dictionary<RuleType, InferenceRule>();
            foreach(var rule in rules)
            {
                dict[rule.type] = rule;
            }

            return dict;
        }
    }
}
