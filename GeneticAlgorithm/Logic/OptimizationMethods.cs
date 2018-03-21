using System;
using Genetic.Models;

namespace Genetic
{
    public class OptimizationMethods
    {
        public Individual RandomSearch(int numberOfDraw, Cache cache)
        {
           Individual _bestIndividual = null;

            for (int k = 0; k < numberOfDraw; k++)
            {
                var individual = new Individual(cache);

                if (_bestIndividual == null || !_bestIndividual.IsBetter(individual.GetFitness()))
                {
                    _bestIndividual = individual;
                }
            };
            return _bestIndividual;
        }

        public Individual GreedyAlgorithm(Individual individual, Cache cache)
        {
            var _bestIndividual = individual;

            for (int i = 0; i < individual.GetGenotypeSize(); i++)
            {
                for (int j = i + 1; i < individual.GetGenotypeSize(); j++)
                {
                    var _genotype = individual.GetGenotype();
                    _genotype.SwapGenes(i, j);
                    var _individual = new Individual(_genotype, cache);

                    if (!_bestIndividual.IsBetter(_individual.GetFitness()))
                    {
                        _bestIndividual = individual;
                    }
                }
            }
                return _bestIndividual;
        }
    }
}
