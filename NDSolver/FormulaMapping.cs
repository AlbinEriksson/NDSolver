using System;
using System.Collections.Generic;
using System.Text;

namespace NDSolver
{
    class FormulaMapping
    {
        private Dictionary<int, ISymbol> mapping;

        public ISymbol this[Formula formula]
        {
            get
            {
                return GetMapping(formula);
            }

            set
            {
                Map(formula, value);
            }
        }

        public int Count => mapping.Count;

        public FormulaMapping()
        {
            mapping = new Dictionary<int, ISymbol>();
        }
        
        public FormulaMapping(FormulaMapping other)
        {
            if (other == null)
            {
                mapping = new Dictionary<int, ISymbol>();
            }
            else
            {
                mapping = new Dictionary<int, ISymbol>(other.mapping);
            }
        }

        public void Map(Formula formula, ISymbol symbol)
        {
            mapping[formula.formulaIndex] = symbol;
        }

        public void MapAll(FormulaMapping formulaMapping)
        {
            foreach(var pair in formulaMapping.mapping)
            {
                mapping[pair.Key] = pair.Value;
            }
        }

        public ISymbol GetMapping(Formula formula)
        {
            return mapping.GetValueOrDefault(formula.formulaIndex);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            FormulaMapping other = (FormulaMapping)obj;
            
            if(Count != other.Count)
            {
                return false;
            }

            foreach(var pair in mapping)
            {
                if(!other.mapping[pair.Key].Equals(pair.Value))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            string result = "";
            foreach(var pair in mapping)
            {
                result += new Formula(pair.Key).ToString() + ": " + pair.Value.ToString() + ", ";
            }
            return result[0..^2];
        }
    }
}
