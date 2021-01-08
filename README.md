# NDSolver
A sequent prover which uses natural deduction. Only supports propositional logic which includes the symbols ∧, ∨, →, ¬ and ⊥.

Currently, there is no expression parser, so you must pass in a symbol object into the `prover.Prove(...)` method. See `Program.cs` as an example.

If you open the .sln file, you can compile an executable file which can be used to test the algorithm on 31 different sequents.
Note that some of these are not provable by the algorithm at this point.
The syntax for the executable is:
```
NDSolver [sequentNumber]
```

Providing no sequent number means it will test all of the sequents. Don't worry, it doesn't take too long to execute and won't melt your processor.
