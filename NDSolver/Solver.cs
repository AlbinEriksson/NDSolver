using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDSolver
{
    class Solver
    {
        private readonly Dictionary<RuleType, InferenceRule> rules;
        private Stack<ISymbol> proofStack = new Stack<ISymbol>();
        private CountingSet<IProofStep> truths = new CountingSet<IProofStep>();

        // Remembers which rule was last used when no direct reference was available.
        // This is required to break infinite chains of double negation eliminations.
        private RuleType lastUnreferredRule;
        public RuleType? currentRule = null;

        private ISymbol currentOEPremise = null;
        private Stack<ISymbol> orEliminationPremises = new Stack<ISymbol>();

        public Solver()
        {
            rules = InferenceRule.AllNonDerived();
        }

        public Proof Prove(Sequent sequent)
        {
            if (proofStack.Contains(sequent.conclusion) && !AllowBypassProofStack(sequent.conclusion))
            {
                // We're already trying to prove this conclusion; circular argument detected
                return null;
            }

            if(currentRule == RuleType.OR_Elimination)
            {
                orEliminationPremises.Push(currentOEPremise);
            }

            Proof proof = new Proof
            {
                conclusion = sequent.conclusion
            };
            var premiseSteps = proof.AddPremises(sequent.premises);

            proofStack.Push(sequent.conclusion);
            truths.AddAll(premiseSteps);

            RuleType[] concludingRuleTypes = sequent.conclusion.GetConcludingRuleTypes();
            Proof subproof = null;
            foreach (RuleType ruleType in concludingRuleTypes)
            {
                if(rules.ContainsKey(ruleType))
                {
                    subproof = TryRule(sequent, rules[ruleType]);
                    if(subproof != null)
                    {
                        proof.AddStepsFromProof(subproof);
                        break;
                    }
                }
            }

            truths.RemoveAll(premiseSteps);
            proofStack.Pop();

            if(currentRule == RuleType.OR_Elimination)
            {
                orEliminationPremises.Pop();
            }

            if(subproof == null)
            {
                return null;
            }

            return proof;
        }

        private Proof TryRule(Sequent sequent, InferenceRule rule)
        {
            RuleType? prevRule = currentRule;
            currentRule = rule.type;

            FormulaMapping mapping = null;
            if (!MapFormulas(rule.conclusion, sequent.conclusion, null, ref mapping))
            {
                throw new ArgumentException("The given rule should have been applicable to the conclusion");
            }

            Proof proof = new Proof();
            RuleStep step = new RuleStep(1, sequent.conclusion, rule.type);
            if(MatchPremises(rule, mapping, ref proof, ref step))
            {
                proof.AddRuleStep(step);
            }
            else
            {
                proof = null;
            }

            currentRule = prevRule;

            return proof;
        }

        private bool MatchPremises(InferenceRule rule, FormulaMapping mapping, ref Proof proof, ref RuleStep step, int premiseIndex = 0)
        {
            if(premiseIndex >= rule.premises.Count)
            {
                return true;
            }

            ISymbol premise = rule.premises[premiseIndex];

            List<MappingResult> nonRootMappings = new List<MappingResult>();
            for(int i = 0; i < truths.Count; i++)
            {
                IProofStep truth = truths[i];
                foreach(MappingResult mappingResult in FindFormulaMappings(premise, truth.Conclusion, mapping))
                {
                    if(mappingResult.isRoot)
                    {
                        step.AddReferredStep(truth);

                        if (MatchPremises(rule, mappingResult.mapping, ref proof, ref step, premiseIndex + 1))
                        {
                            break;
                        }

                        step.PopReferredStep();
                    }
                    else
                    {
                        // Do non-root options later for efficiency
                        nonRootMappings.Add(mappingResult);
                    }
                }

                if (step.referredSteps.Count >= rule.premises.Count)
                {
                    return true;
                }
            }

            foreach(MappingResult mappingResult in nonRootMappings)
            {
                Proof newProof = TryProve(mappingResult.formula, rule, mappingResult.mapping, premiseIndex, ref step);
                if (newProof != null)
                {
                    proof.AddStepsFromProof(newProof);
                    return true;
                }

                if (step.referredSteps.Count >= rule.premises.Count)
                {
                    return true;
                }
            }

            if (mapping.Count >= rule.maxFormulaIndex)
            {
                RuleType lastUnreferredRuleBefore = lastUnreferredRule;

                if (rule.type == RuleType.Double_Negation_Elimination && lastUnreferredRule == RuleType.Double_Negation_Elimination)
                {
                    return false;
                }

                lastUnreferredRule = rule.type;

                ISymbol mappedPremise = premise.ApplyMapping(mapping);
                Proof newProof = TryProve(mappedPremise, rule, mapping, premiseIndex, ref step);
                if(newProof != null)
                {
                    proof.AddStepsFromProof(newProof);
                    return true;
                }

                lastUnreferredRule = lastUnreferredRuleBefore;
            }

            // Proof is impossible (invalid)
            return false;
        }

        private Proof TryProve(ISymbol requiredPremise, InferenceRule rule, FormulaMapping mapping, int premiseIndex, ref RuleStep step)
        {
            ISymbol lastOEPremise = currentOEPremise;
            if(rule.type == RuleType.OR_Elimination)
            {
                currentOEPremise = rule.premises[0].ApplyMapping(mapping);
            }

            SolverVisitor solverVisitor = new SolverVisitor(this);
            requiredPremise.Accept(solverVisitor);

            if(rule.type == RuleType.OR_Elimination)
            {
                currentOEPremise = lastOEPremise;
            }

            if (solverVisitor.Result != null)
            {
                Proof proof = solverVisitor.Result;
                step.AddReferredStep(proof.ProofSteps[^1]);
                if(MatchPremises(rule, mapping, ref proof, ref step, premiseIndex + 1))
                {
                    return proof;
                }
                step.PopReferredStep();
            }

            return null;
        }

        private List<MappingResult> FindFormulaMappings(ISymbol formulas, ISymbol truthRoot, FormulaMapping mapping)
        {
            List<MappingResult> results = new List<MappingResult>();

            SymbolEnumerator symbolEnum = new SymbolEnumerator(truthRoot);
            bool isRoot = true;
            while(symbolEnum.MoveNext())
            {
                FormulaMapping newMapping = null;
                if(MapFormulas(formulas, symbolEnum.Current, mapping, ref newMapping))
                {
                    results.Add(new MappingResult(symbolEnum.Current, newMapping, isRoot));
                }
                isRoot = false;
            }

            return results;
        }

        private bool MapFormulas(ISymbol formulas, ISymbol statement, FormulaMapping mapping, ref FormulaMapping newMapping)
        {
            newMapping = new FormulaMapping(mapping);

            SymbolEnumerator formulaEnum = new SymbolEnumerator(formulas);
            SymbolEnumerator statementEnum = new SymbolEnumerator(statement);

            while (formulaEnum.MoveNext())
            {
                statementEnum.MoveNext();

                if (formulaEnum.Current is Formula formula)
                {
                    ISymbol definition = mapping?.GetMapping(formula);

                    if (definition == null)
                    {
                        newMapping.Map(formula, statementEnum.Current);
                        statementEnum.EscapeBranch();
                    }
                    else if(!statementEnum.Current.Equals(definition))
                    {
                        return false;
                    }
                }
                else if(formulaEnum.Current.GetType() != statementEnum.Current.GetType())
                {
                    return false;
                }
            }

            return true;
        }

        private bool AllowBypassProofStack(ISymbol conclusion)
        {
            if (currentRule == RuleType.OR_Elimination && !orEliminationPremises.Contains(currentOEPremise))
            {
                return true;
            }

            return false;
        }
    }
}
