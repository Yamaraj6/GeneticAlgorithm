using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic.Models
{
    class Generation
    {
        public const int CHILDREN_NUMBER = 2;

        public List<Individual> population { get; private set; }
        public int populationSize { get; private set; }
        private MatrixData matrixData;

        public float bestFitness { get; private set; }
        public float averageFitness { get; private set; }
        public float worstFitness { get; private set; }
        public int generationNumber { get; private set; }



        public Generation(int populationSize, MatrixData matrixData)
        {
            SetupGeneration(populationSize, 0, matrixData);
        }

        public Generation(List<Individual> population,
            int populationSize, int generationNumber, MatrixData matrixData)
        {
            this.population = population;
            SetupGeneration(populationSize, generationNumber, matrixData);
        }

        private void SetupGeneration(int populationSize, int generationNumber, MatrixData matrixData)
        {
            this.populationSize = populationSize;
            this.generationNumber = generationNumber;
            this.matrixData = matrixData;
            if (generationNumber == 0)
            {
                population = new List<Individual>();
            }
            FillPopulation(population);
            CountRates();
        }
        
        private void FillPopulation(List<Individual> population)
        {
            while (population.Count < populationSize)
            {
                population.Add(new Individual(matrixData));
            }
        }

        public Generation MakeGeneration(float mutationProb, float crossoverProb, int tour)
        {
            var _newPopulation = new List<Individual>();
            Individual[] _parents;

            for (int i = 0; i < (crossoverProb * population.Count)/CHILDREN_NUMBER; i++)
            {
                _parents = Selection(tour);

                for (int j = 0; j < CHILDREN_NUMBER; j++)
                {
                    _newPopulation.Add(Crossover(_parents));
                }
            }

            FillPopulation(_newPopulation);
            MutatePopulation(_newPopulation, mutationProb);

            return new Generation(_newPopulation, populationSize, generationNumber+1, matrixData);
        }

        private Individual[] Selection(int tour)
        {
            Random _random = new Random();
            Individual[] _selectedParents = new Individual[2];
            Individual[] _candidateParents = new Individual[tour];

            for (int i = 0; i < _selectedParents.Length; i++)
            {
                for (int j = 0; j < _candidateParents.Length; j++)
                {
                    _candidateParents[j] = population[_random.Next() % population.Count];
                }
                _selectedParents[i] = _candidateParents[0];

                for (int j = 0; j < _candidateParents.Length; j++)
                {
                    if (_candidateParents[j].IsBetterFitness(_selectedParents[i].fitness))
                    {
                        _selectedParents[i] = _candidateParents[j];
                    }
                }
            }
            return _selectedParents;
        }

        private Individual Crossover(Individual[] parents)
        {
            Random _random = new Random();
            int _crossoverPoint = _random.Next() % parents[0].genotype.Length;
            int[,] gene = new int[parents[0].genotype.Length / Individual.ROWS_COUNT, Individual.ROWS_COUNT];
            for (int x = 0; x < parents[0].genotype.Length / Individual.ROWS_COUNT; x++)
            {
                gene[x, Individual.PLACES_ROW] = x;
                if (x < _crossoverPoint)
                {
                    gene[x, Individual.FACTORIES_ROW] = parents[0].genotype[x, Individual.FACTORIES_ROW];
                }
                else
                {
                    gene[x, Individual.FACTORIES_ROW] = parents[1].genotype[x, Individual.FACTORIES_ROW];
                }
            }
            FixGene(ref gene, _crossoverPoint);
            return new Individual(gene, matrixData);
        }

        private void FixGene(ref int[,] gene, int crossoverPoint)
        {
            for (int x = crossoverPoint; x < gene.Length / Individual.ROWS_COUNT; x++)
            {
                for (int i = 0; i < x; i++)
                {
                    if (gene[x, Individual.FACTORIES_ROW] == gene[i, Individual.FACTORIES_ROW])
                    {
                        gene[x, Individual.FACTORIES_ROW] = FindMissingGeneFragment(gene);
                    }
                }
            }
            return;
        }

        private int FindMissingGeneFragment(int[,] gene)
        {
            for (int x = 0; x < gene.Length / Individual.ROWS_COUNT; x++)
            {
                for (int i = 0; gene[x, Individual.PLACES_ROW] != gene[i, Individual.FACTORIES_ROW];)
                {
                    i++;
                    if (i == gene.Length / Individual.ROWS_COUNT)
                    {
                        return gene[x, Individual.PLACES_ROW];
                    }
                }
            }
            return -1;
        }

        private void MutatePopulation(List<Individual> population, float mutationProb)
        {
            Random _random = new Random();
            for (int i = 0; i < population.Count; i++)
            {
                if (_random.NextDouble() < mutationProb)
                {
                    population[i].Mutates();
                }
            }
        }

        private void CountRates()
        {
            bestFitness = population[0].fitness;
            worstFitness = population[0].fitness;
            averageFitness = 0;

            for (int i = 1; i < population.Count; i++)
            {
                averageFitness += population[i].fitness;
                if (population[i].IsBetterFitness(bestFitness))
                {
                    bestFitness = population[i].fitness;
                }
                else if (!population[i].IsBetterFitness(worstFitness))
                {
                    worstFitness = population[i].fitness;
                }
            }
            averageFitness /= population.Count;
        }

        public override string ToString()
        {
            return generationNumber + ";" + bestFitness + ";" +
                averageFitness + ";" + worstFitness+"\n";
        }
    }
}
