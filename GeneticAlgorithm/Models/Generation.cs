using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic.Models
{
    class Generation<GeneKey, GeneValue>
    {
        public List<IIndividual<GeneKey, GeneValue>> population { get; private set; }
        public int populationSize { get; private set; }
        public int generationNumber { get; private set; }

        private ICache<GeneKey, GeneValue> cache;

        public IIndividual<GeneKey, GeneValue> bestIndividual { get; private set; }
        public IIndividual<GeneKey, GeneValue> worstIndividual { get; private set; }
        public float averageFitness { get; private set; }

        public Generation(int populationSize, int generationNumber,
            ICache<GeneKey, GeneValue> cache)
        {
            this.populationSize = populationSize;
            this.generationNumber = generationNumber;
            this.cache = cache;
            population = new List<IIndividual<GeneKey, GeneValue>>();
        }

        public Generation(IIndividual<GeneKey, GeneValue> individual, int populationSize,
        ICache<GeneKey, GeneValue> cache)
        {
            this.populationSize = populationSize;
            this.generationNumber = 0;
            this.cache = cache;
            population = new List<IIndividual<GeneKey, GeneValue>>();
            population.Add(individual);
            FillPopulation();
        }

        public void FillPopulation()
        {
            while (population.Count < populationSize)
            {
                population.Add(population[0].CreateNewIndividual(cache));
            }
            CountRates();
        }

        public IIndividual<GeneKey, GeneValue>[] Selection(GeneticParameters geneticParameters)
        {
            switch (geneticParameters.selectionType)
            {
                case SelectionType.TournamentSelection:
                    return TournamentSelection((int)(geneticParameters.crossoverProb * populationSize),
                    geneticParameters.tournamentSize);
                case SelectionType.RouletteSelection:
                    return RouletteSelection((int)(geneticParameters.crossoverProb * populationSize));
                default:
                    return OptimizedRouletteSelection((int)(geneticParameters.crossoverProb * populationSize));
            }
        }

        private IIndividual<GeneKey, GeneValue>[] TournamentSelection(int numberOfParents, int tour)
        {
            var _selectedParents = new IIndividual<GeneKey, GeneValue>[numberOfParents];
            var _candidateParents = new IIndividual<GeneKey, GeneValue>[tour];

            for (int i = 0; i < _selectedParents.Length; i++)
            {
                for (int j = 0; j < _candidateParents.Length; j++)
                {
                    _candidateParents[j] = population[cache.GetRandomNext() % populationSize];
                }
                _selectedParents[i] = _candidateParents[0];

                for (int j = 1; j < _candidateParents.Length; j++)
                {
                    if (_candidateParents[j].IsBetter(_selectedParents[i].GetFitness()))
                    {
                        _selectedParents[i] = _candidateParents[j];
                    }
                }
            }
            return _selectedParents;
        }

        private IIndividual<GeneKey, GeneValue>[] RouletteSelection(int numberOfParents)
        {
            var _selectedParents = new IIndividual<GeneKey, GeneValue>[numberOfParents];
            float _fitnessesSum = 0;
            foreach (var individual in population)
            {
                _fitnessesSum += (worstIndividual.GetFitness()/individual.GetFitness());
            }

            for (int i = 0; i < _selectedParents.Length; i++)
            {
                var _randIndividual = cache.GetRandomNext() % _fitnessesSum;
                foreach(var individual in population)
                {
                    _randIndividual -= (worstIndividual.GetFitness() / individual.GetFitness());
                    if (_randIndividual <= 0)
                    {
                        _selectedParents[i] = individual;
                        break;
                    }
                }
            }
            return _selectedParents;
        }

        private IIndividual<GeneKey, GeneValue>[] OptimizedRouletteSelection(int numberOfParents)
        {
            var _selectedParents = new IIndividual<GeneKey, GeneValue>[numberOfParents];
            float _fitnessesSum = 0;
            var min = Math.Abs(worstIndividual.GetFitness() - bestIndividual.GetFitness()) / populationSize;
            foreach (var individual in population)
            {
                _fitnessesSum += (Math.Abs(worstIndividual.GetFitness() - individual.GetFitness()) + min);
            }

            for (int i = 0; i < _selectedParents.Length; i++)
            {
                var _randIndividual = cache.GetRandomNext() % _fitnessesSum;
                for (int j = 0; j < populationSize; j++)
                {
                    _randIndividual -= (Math.Abs(worstIndividual.GetFitness() - population[j].GetFitness()) + min);
                    if (_randIndividual <= 0)
                    {
                        _selectedParents[i] = population[j];
                        break;
                    }
                }
            }
            return _selectedParents;
        }

        public void Crossover(IIndividual<GeneKey, GeneValue>[] parents)
        {
            for (int i = 1; i < parents.Length; i = i + 2)
            {
                population.Add(parents[i - 1].Crossover(parents[i]));
                population.Add(parents[i].Crossover(parents[i - 1]));
            }
        }

        public void MutatePopulation(float mutationProb)
        {
            for (int i = 0; i < populationSize; i++)
            {
                if (cache.GetRandomNextDouble() < mutationProb)
                {
                    population[i].Mutates();
                }
            }
            CountRates();
        }

        private void CountRates()
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

        public override string ToString()
        {
            return generationNumber + ";" + bestIndividual.GetFitness() + ";" +
                averageFitness + ";" + worstIndividual.GetFitness();
        }
    }
}