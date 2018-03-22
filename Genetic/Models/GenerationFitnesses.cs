using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic.Models
{
    public enum FitnessType { BestFitness, AverageFitness, WorstFitness }

    class GenerationFitnesses<GeneKey, GeneValue>
    {
        private IIndividual<GeneKey, GeneValue> worstIndividual;
        private IIndividual<GeneKey, GeneValue> bestIndividual;

        public float worstFitness { get { return worstIndividual.GetFitness(); } }
        public float bestFitness { get { return bestIndividual.GetFitness(); } }
        public float averageFitness { get; private set; }

        public void CountRates(List<IIndividual<GeneKey, GeneValue>> population)
        {
            float _bestFitness = population[0].GetFitness();
            float _worstFitness = population[0].GetFitness();
            averageFitness = population[0].GetFitness();
            bestIndividual = population[0];
            worstIndividual = population[0];

            for (int i = 1; i < population.Count; i++)
            {
                averageFitness += population[i].GetFitness();
                if (population[i].IsBetter(_bestFitness))
                {
                    _bestFitness = population[i].GetFitness();
                    bestIndividual = population[i];
                }
                else if (!population[i].IsBetter(_worstFitness))
                {
                    _worstFitness = population[i].GetFitness();
                    worstIndividual = population[i];
                }
            }
            averageFitness /= population.Count;
        }

    }
}
