using Genetic.Models.FactoryPlacementProblem;

namespace GeneticAlgorithmTest
{
    public class OptimizationMethods
    {
        public static Individual RandomSearch(int numberOfDraw, Cache cache)
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

        public static Individual GreedyAlgorithm(Individual individual, Cache cache)
        {
            var _bestIndividual = individual;

            for (int i = 0; i < individual.GetGenotypeSize(); i++)
            {
                for (int j = i + 1; j < individual.GetGenotypeSize(); j++)
                {
                    Genotype _genotype = new Genotype(individual.GetGenotype(), cache);

                    _genotype.SwapGenes(i, j);
                    var _individual = new Individual(_genotype, cache);

                    if (!_bestIndividual.IsBetter(_individual.GetFitness()))
                    {
                        _bestIndividual = _individual;
                    }
                }
            }
            return _bestIndividual;
        }

        public static Individual GreedySearch(int numberOfDraw, Cache cache)
        {
            var _bestIndividual = new Individual(cache);
            _bestIndividual = GreedyAlgorithm(_bestIndividual, cache);
            for(int i =1; i < numberOfDraw;i++)
            {
                var _individual = new Individual(cache);
                _individual = GreedyAlgorithm(_individual, cache);
                if (!_bestIndividual.IsBetter(_individual.GetFitness()))
                {
                    _bestIndividual = _individual;
                }
            }
            return _bestIndividual;
        }
    }
}
