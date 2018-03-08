using System;
using Genetic.Models;

namespace Genetic
{
    class RandomData
    {
        public Individual RandomSearch(int numberOfDraw, MatrixData matrixData)
        {
           Individual _bestIndividual = null;

            for (int k = 0; k < numberOfDraw; k++)
            {
                var individual = new Individual(matrixData);

                if (_bestIndividual == null || !_bestIndividual.IsBetterFitness(individual.fitness))
                {
                    _bestIndividual = individual;
                }
            };
            return _bestIndividual;
        }
    }
}
