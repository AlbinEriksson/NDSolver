using System;
using System.Collections.Generic;

namespace NDSolver
{
    class Program
    {
        static Sequent[] problems = new Sequent[]
        {
            //     Sequent 0:
            // p -> q, q -> r  |-  p -> r
            // Status: SUCCESS (Dec 22, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new Atom(1)),
                    new Implies(new Atom(1), new Atom(2))
                },
                new Implies(new Atom(0), new Atom(2))
            ),
            
            //     Sequent 1:
            // p ^ q, r  |-  q ^ r
            // Status: SUCCESS (Dec 22, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new AND(new Atom(0), new Atom(1)),
                    new Atom(2)
                },
                new AND(new Atom(1), new Atom(2))
            ),
            
            //     Sequent 2:
            // p, --(q ^ r)  |-  --p ^ r
            // Status: SUCCESS (Dec 29, 2020)
            // The problem below was solved by prohibiting the action of trying to prove a brand new double negation. This ensures that
            // the proof stack can only add more not-towers if it is based on an existing truth.
            // Past Status: NOT POSSIBLE (Dec 29, 2020)
            // Despite implementing the second option below, this problem got stuck when it would try to use double negation elimination
            // forever, causing a new tower of not-operations to be added to the proof stack.
            // Past Status: NOT POSSIBLE (Dec 22, 2020)
            // It will successfully infer --p from p.
            // However, the other requirement, r, needs and-elimination on α ^ r.
            // We do not have such a formula. We only have --(α ^ r).
            // To solve this, we could approach it in two ways:
            //  1. (Seems more circumstantial) Look ahead on the current truths, and try to infer new ones using no proofs whatsoever.
            //     In this method, we can use --e on --(q ^ r) to infer q ^ r.
            //     We would have to store this in some kind of "unused steps" list. When they become used, they are added to the proof.
            //     Complication: We could do this forever, so there needs to be some sort of arbitrary limit. This is only useful as a heuristic.
            //     Complication: Because of the limit, we don't know if we've inferred the correct formulae that we need to perform the proof.
            //  2. (Not circumstantial, perhaps harder to implement) When matching required premises, search for sub-trees that may match
            //     the required premise. So, when we find that the formula tree's current node is different from the truth tree node,
            //     we only advance in the truth tree until we find a matching element. This means that if we find a matching SUB-tree,
            //     as opposed to the entire tree itself, then we cannot refer to it as a proof step just yet. That sub-tree must also be
            //     proven first.
            new Sequent(
                new ISymbol[]
                {
                    new Atom(0),
                    new NOT(new NOT(new AND(new Atom(1), new Atom(2))))
                },
                new AND(new NOT(new NOT(new Atom(0))), new Atom(2))
            ),
            
            //     Sequent 3:
            // (p ^ q) ^ r, s ^ t  |-  q ^ s
            // Status: SUCCESS (Dec 29, 2020)
            // The implementation below has successfully been adapted such that other previously functional problems are still solved correctly,
            // and now this one is also one of them.
            // Past Status: NOT POSSIBLE (Dec 27, 2020)
            // This one relies on the same change as the one above. It tries to find q, and it may seem that and-elimination is the correct rule,
            // but it ultimately cannot use it to get q. It gets either p ^ q or r, and it doesn't know that it can use and-elimination twice.
            // At this point, I'm inclined to believe that approach number 2 in the previous sequent is the ideal one, albeit with a few tweaks:
            // Firstly, when matching required premises to a truth, don't just match the truth's root. Try to match all the nodes in the entire
            // truth parse tree. This means you'll return a list of possible mappings, and whether they were the root or not. If a mapping was
            // based on the root, then it requires no proof, similar to how it is handled now. If it wasn't based on the root, then we may try
            // to prove that sub-tree.
            new Sequent(
                new ISymbol[]
                {
                    new AND(new AND(new Atom(0), new Atom(1)), new Atom(2)),
                    new AND(new Atom(3), new Atom(4))
                },
                new AND(new Atom(1), new Atom(3))
            ),
            
            //     Sequent 4:
            // -p ^ q, -p ^ q -> r v -p  |-  r v -p
            // Status: SUCCESS (Dec 29, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new AND(new NOT(new Atom(0)), new Atom(1)),
                    new Implies(new AND(new NOT(new Atom(0)), new Atom(1)), new AND(new Atom(2), new NOT(new Atom(0))))
                },
                new AND(new Atom(2), new NOT(new Atom(0)))
            ),
            
            //     Sequent 5:
            // p -> (q -> r), p -> q, p  |-  r
            // Status: SUCCESS (Dec 29, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new Implies(new Atom(1), new Atom(2))),
                    new Implies(new Atom(0), new Atom(1)),
                    new Atom(0)
                },
                new Atom(2)
            ),
            
            //     Sequent 6:
            // p -> (q -> r), p, -q  |-  -q
            // Status: SUCCESS (Dec 29, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new Implies(new Atom(1), new Atom(2))),
                    new Atom(0),
                    new NOT(new Atom(2))
                },
                new NOT(new Atom(1))
            ),
            
            //     Sequent 7:
            // -p -> q, -q  |-  p
            // Status: SUCCESS (Dec 30, 2020)
            // Below has been implemented!
            // Past Status: NOT POSSIBLE (Dec 29, 2020)
            // Due to the addition of prohibiting double negation on new proofs, this one will never finish, since it needs
            // to make --p. There would have to be something a bit more sophisticated in order to detect the possibility to infer
            // double negation. Perhaps we can allow double negation on new proofs iff it is justified by something other than
            // double negation elimination. If, for any reason, a sequent has a premise like -----p -> q, then we don't have to
            // allow this kind of double negation proof. It is only for the small cases where only one not is present, like in
            // this case, -p -> q.
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new NOT(new Atom(0)), new Atom(1)),
                    new NOT(new Atom(1))
                },
                new Atom(0)
            ),
            
            //     Sequent 8:
            // p -> -q, q  |-  -p
            // Status: SUCCESS (Dec 30, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new NOT(new Atom(1))),
                    new Atom(1)
                },
                new NOT(new Atom(0))
            ),
            
            //     Sequent 9:
            // p -> q  |-  -q -> -p
            // Status: SUCCESS (Dec 30, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new Atom(1))
                },
                new Implies(new NOT(new Atom(1)), new NOT(new Atom(0)))
            ),
            
            //     Sequent 10:
            // -q -> -p  |-  p -> --q
            // Status: SUCCESS (Dec 30, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new NOT(new Atom(1)), new NOT(new Atom(0)))
                },
                new Implies(new Atom(0), new NOT(new NOT(new Atom(1))))
            ),
            
            //     Sequent 11:
            // p  |-  p
            // Status: SUCCESS (Dec 30, 2020)
            // A little strange solution, but it works:
            // 1. p         (Premise)
            //    2. -p     (Hypothesis)
            //    3. _|_    (-e 1, 2)
            // 4. --p       (-i 2-3)
            // 5. p         (--e 4)
            new Sequent(
                new ISymbol[]
                {
                    new Atom(0)
                },
                new Atom(0)
            ),
            
            //     Sequent 12:
            // |-  p -> p
            // Status: SUCCESS (Dec 30, 2020)
            // Similar to the one above
            new Sequent(
                new ISymbol[0],
                new Implies(new Atom(0), new Atom(0))
            ),
            
            //     Sequent 13:
            // |-  (q -> r) -> ((-q -> -p) -> (p -> r))
            // Status: SUCCESS (Dec 30, 2020)
            // I'm surprised at how quickly it runs, and how it actually doesn't get stuck or runs forever.
            new Sequent(
                new ISymbol[0],
                new Implies(
                    new Implies(new Atom(1), new Atom(2)),
                    new Implies(
                        new Implies(new NOT(new Atom(1)), new NOT(new Atom(0))),
                        new Implies(new Atom(0), new Atom(2))
                    )
                )
            ),
            
            //     Sequent 14:
            // (p ^ q) -> r  |-  p -> (q -> r)
            // Status: SUCCESS (Dec 30, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new AND(new Atom(0), new Atom(1)), new Atom(2))
                },
                new Implies(new Atom(0), new Implies(new Atom(1), new Atom(2)))
            ),
            
            //     Sequent 15:
            // p -> (q -> r)  |-  (p ^ q) -> r
            // Status: SUCCESS (Dec 30, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new Implies(new Atom(1), new Atom(2)))
                },
                new Implies(new AND(new Atom(0), new Atom(1)), new Atom(2))
            ),
            
            //     Sequent 16:
            // p -> q  |-  p ^ r -> q ^ r
            // Status: SUCCESS (Dec 30, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new Atom(1))
                },
                new Implies(
                    new AND(new Atom(0), new Atom(2)),
                    new AND(new Atom(1), new Atom(2))
                )
            ),
            
            //     Sequent 17:
            // p v q  |-  q v p
            // Status: SUCCESS (Dec 31, 2020)
            // Below was not implemented. Depths of proofs/boxes would increase for not-introduction, which
            // may be used to later infer double negation elimination. As we have already established, double
            // negation eliminations may cause infinite expansions of proof conclusions. To avoid this,
            // multiple measures were taken:
            //  1. We may allow a proof to bypass the stack check iff the current rule is or-elimination and
            //     the previous rule was not double negation elimination.
            //  2. Since or elimination could be used upon itself, we tweak the double negation elimination
            //     fix by remembering the last unreferred rule in general, as opposed to only DNE. Also, a
            //     "last referred depth" is added, so that or-elimination may fulfil the proof of its two
            //     boxes.
            // It should be noted that or-elimination is VERY inefficient at the moment, and will appear
            // perpetually when there is an or-connective among the premises. In theory, this could be solved
            // with some form of alpha-beta pruning, but I'm unsure how this can be done.
            // Past Status: NOT POSSIBLE (Dec 30, 2020)
            // We need to use or elimination to take out two cases; one for p and one for q. In both cases, we
            // use or introduction to conclude q v p. The problem is that we are already proving q v p outside
            // these cases, so the prover detects the cases as circular arguments.
            // The difference between the first proof of q v p and the second proof is that they occur at
            // different "depths" of proofs/boxes. What we could do is to not only store a symbol in the proof
            // stack, but also its depth. That depth would start at 0, increment whenever a new hypothesis is
            // being proven, and decrement when that proof is over. Thus, it also ends with 0, but will change
            // over the course of the entire proof procedure.
            new Sequent(
                new ISymbol[]
                {
                    new OR(new Atom(0), new Atom(1))
                },
                new OR(new Atom(1), new Atom(0))
            ),
            
            //     Sequent 18:
            // q -> r  |-  p v q -> p v r
            // Status: SUCCESS (Dec 31, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(1), new Atom(2))
                },
                new Implies(
                    new OR(new Atom(0), new Atom(1)),
                    new OR(new Atom(0), new Atom(2))
                )
            ),
            
            //     Sequent 19:
            // (p v q) v r  |-  p v (q v r)
            // Status: SUCCESS (Jan 2, 2020)
            // This was solved by realizing that each and every or-eliminations conclusion, when not looping
            // infinitely, has a unique base premise. So, I simply added a stack of or-elimination premises,
            // and prevent proof stack bypass when the current or-elimination premise is not already in the
            // stack of OE premises.
            // Past Status: NOT POSSIBLE (Dec 31, 2020)
            // It starts out by trying to do or-introduction on p. It sees the p v q and tries to infer it.
            // This leads to the solver seeing the same p v q sub-expression when using or-elimination to
            // infer p v q, leading to a perpetual loop of proving the same p v q over and over.
            new Sequent(
                new ISymbol[]
                {
                    new OR(
                        new OR(new Atom(0), new Atom(1)),
                        new Atom(2)
                    )
                },
                new OR(
                    new Atom(0),
                    new OR(new Atom(1), new Atom(2))
                )
            ),
            
            //     Sequent 20:
            // p ^ (q v r)  |-  (p ^ q) v (p ^ r)
            // Status: SUCCESS (Jan 2, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new AND(
                        new Atom(0),
                        new OR(new Atom(1), new Atom(2))
                    )
                },
                new OR(
                    new AND(new Atom(0), new Atom(1)),
                    new AND(new Atom(0), new Atom(2))
                )
            ),
            
            //     Sequent 21:
            // |- p -> (q -> p)
            // Status: SUCCESS (Jan 2, 2020)
            // We really need to fix the reiteration rule...
            new Sequent(
                new ISymbol[0],
                new Implies(
                    new Atom(0),
                    new Implies(new Atom(1), new Atom(0))
                )
            ),
            
            //     Sequent 22:
            // -p v q  |-  p -> q
            // Status: SUCCESS (Jan 2, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new OR(
                        new NOT(new Atom(0)),
                        new Atom(1)
                    )
                },
                new Implies(new Atom(0), new Atom(1))
            ),
            
            //     Sequent 23:
            // p -> q, p -> -q  |-  -p
            // Status: SUCCESS (Jan 2, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new Atom(1)),
                    new Implies(new Atom(0), new NOT(new Atom(1)))
                },
                new NOT(new Atom(0))
            ),
            
            //     Sequent 24:
            // p -> -p  |-  -p
            // Status: COULD BE POSSIBLE (Jan 2, 2020)
            // The below implementation WOULD work. However, it will slow down many of the other proofs above
            // significantly. This implementation would require the use of the aforementioned alpha-beta pruning
            // to eliminate redundant subproving.
            // Past Status: NOT POSSIBLE (Jan 2, 2020)
            // To prove this one, you must prove the conclusion -p through not-introduciton, which implies
            // the use of not-elimination on p and -p. p is the premise of the new box, and -p must be proven.
            // However, even though it would be trivial, it is detected as a circular argument, as -p is already
            // on the stack.
            // One idea I have is to modify the proof stack. Instead of simply storing the conclusion, also store
            // the formula mapping alongside it. This would apply the same bypass as for or-elimination, but
            // for all rules.
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new NOT(new Atom(0)))
                },
                new NOT(new Atom(0))
            ),
            
            //     Sequent 25:
            // p ^ -q -> r, -r, p  |-  q
            // Status: SUCCESS (Jan 2, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(
                        new AND(new Atom(0), new NOT(new Atom(1))),
                        new Atom(2)
                    ),
                    new NOT(new Atom(2)),
                    new Atom(0)
                },
                new Atom(1)
            ),
            
            //     Sequent 26:
            // p -> q, -q  |-  -p
            // Status: SUCCESS (Jan 2, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new Atom(1)),
                    new NOT(new Atom(1))
                },
                new NOT(new Atom(0))
            ),
            
            //     Sequent 27:
            // p  |-  --p
            // Status: SUCCESS (Jan 2, 2020)
            new Sequent(
                new ISymbol[]
                {
                    new Atom(0)
                },
                new NOT(new NOT(new Atom(0)))
            ),
            
            //     Sequent 28:
            // -p -> _|_  |-  p
            // Status: SUCCESS (Jan 2, 2020)
            // Implemented below.
            // Past Status: NOT POSSIBLE (Jan 2, 2020)
            // Contradictions cannot be inferred through implication elimination. Though it makes sense that
            // it should, doesn't it? For and-connectives, you cannot have a contradiction (that would never
            // be true). But implications can conclude contradictions indeed.
            new Sequent(
                new ISymbol[]
                {
                    new Implies(
                        new NOT(new Atom(0)),
                        new Contradiction()
                    )
                },
                new Atom(0)
            ),
            
            //     Sequent 29:
            // |-  p v -p
            // Status: COULD BE POSSIBLE (Jan 2, 2020)
            // It would need to prove a box with contradiction, by having -(p v -p) and p v -p as members for
            // the not-elimination. So, it needs to prove p v -p, which is already on the stack. This would be
            // solved if we add the slow proof stack.
            new Sequent(
                new ISymbol[0],
                new OR(
                    new Atom(0),
                    new NOT(new Atom(0))
                )
            ),
            
            //     Sequent 30:
            // p -> q  |-  -p v q
            // Status: NOT POSSIBLE (Jan 2, 2020)
            // For this one to work, we would need the problem above (otherwise known as LEM).
            new Sequent(
                new ISymbol[]
                {
                    new Implies(new Atom(0), new Atom(1))
                },
                new OR(new NOT(new Atom(0)), new Atom(1))
            ),
        };

        static void Main(string[] args)
        {
            Solver solver = new Solver();

            if(args.Length == 1)
            {
                DoProof(solver, int.Parse(args[0]));
            }
            else if(args.Length == 0)
            {
                int solveCount = 0;
                for(int i = 0; i < problems.Length; i++)
                {
                    if(DoProof(solver, i))
                    {
                        solveCount++;
                    }

                    Console.WriteLine("\n\n");
                }

                Console.WriteLine("Proved " + solveCount + " out of " + problems.Length + " sequents");
            }
        }

        static bool DoProof(Solver solver, int problemNumber)
        {
            Sequent sequent = problems[problemNumber];
            Proof proof = solver.Prove(sequent);

            if (proof == null)
            {
                Console.WriteLine("The following sequent could not be proven:");
                Console.WriteLine(sequent.ToString());
                return false;
            }
            else
            {
                proof.PrintToConsole();
                return true;
            }
        }
    }
}
