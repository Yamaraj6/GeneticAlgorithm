using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic
{
    public interface ICache<GeneKey, GeneValue>
    {
        int GetProblemSize();

        bool TryGetFitness(IGenotype<GeneKey, GeneValue> genotype, out float fitness);
        void AddCalculatedFitness(string genotype, float fitness);
        int GetCalculatedFitnessesCount();

        int GetRandomNext();
        double GetRandomNextDouble();
    }
}
