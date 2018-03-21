using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic
{
    public interface IGenotype<GeneKey, GeneValue>
    {
        IGene<GeneKey, GeneValue> GetGene(int positon);
        void SwapGenes(int positonOne, int positonTwo);
        int GetGenotypeSize();
        void Mutates();
    }
}
