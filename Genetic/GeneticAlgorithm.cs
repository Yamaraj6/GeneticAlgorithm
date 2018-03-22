using Genetic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic
{
    public class GeneticAlgorithm<GeneKey, GeneValue>
    {
        private GeneticParameters geneticParameters;
        private List<Generation<GeneKey, GeneValue>> generations;
        public ICache<GeneKey, GeneValue> cache { get; private set; }
        private StringBuilder stringBuilder;

        public GeneticAlgorithm(GeneticParameters geneticParameters, ICache<GeneKey, GeneValue> cache)
        {
            this.geneticParameters = geneticParameters;
            this.cache = cache;
            stringBuilder = new StringBuilder();
        }

        public void CreateSpecies(IIndividual<GeneKey, GeneValue> speciesType)
        {
            if (geneticParameters != null)
            {
                generations = new List<Generation<GeneKey, GeneValue>>();
                generations.Add(new Generation<GeneKey, GeneValue>(speciesType,
                    geneticParameters.populationSize, cache));

                for (int i = 1; i < geneticParameters.generationsCount; i++)
                {
                    generations.Add(CreateGeneration(generations[i - 1]));
                    stringBuilder.Append(generations[i - 1].ToString() + ";"
                        + cache.GetCalculatedFitnessesCount() + "\n");
                }
                stringBuilder.Append(generations[geneticParameters.generationsCount - 1].ToString() + ";"
                    + cache.GetCalculatedFitnessesCount() + "\n");
            }
        }

        public void CreateSpecies(IIndividual<GeneKey, GeneValue> speciesType, GeneticParameters geneticParameters)
        {
            this.geneticParameters = geneticParameters;
            CreateSpecies(speciesType);
        }

        private Generation<GeneKey, GeneValue> CreateGeneration(Generation<GeneKey, GeneValue> parentsGeneration)
        {
            var _parents = parentsGeneration.Selection(geneticParameters);

            var _newGeneration = new Generation<GeneKey, GeneValue>(geneticParameters.populationSize,
                parentsGeneration.generationNumber + 1, cache);
            _newGeneration.Crossover(_parents);
            _newGeneration.FillPopulation();
            _newGeneration.MutatePopulation(geneticParameters.mutationProb);
            return _newGeneration;
        }

        public float[] GetGenerationFitnesses(int generationNumber)
        {
            if (generationNumber < 1 && generationNumber > generations.Count)
            {
                generationNumber = 1;
            }
            float[] _fitnesses = new float[Enum.GetNames(typeof(FitnessType)).Length];
            int i = 0;
            foreach (var _fitneessType in Enum.GetValues(typeof(FitnessType)))
            {
                _fitnesses[i] = generations[generationNumber].GetFitnesses((FitnessType)_fitneessType);
                i++;
            }
            return _fitnesses;
        }

        public override string ToString()
        {
            stringBuilder.Insert(0, "generationNumber;bestFitness;averageFitness;worstFitness;cache\n");
            return stringBuilder.ToString();
        }
    }
}