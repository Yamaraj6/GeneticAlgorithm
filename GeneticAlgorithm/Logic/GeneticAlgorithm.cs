using System.Collections.Generic;
using System.Text;
using Genetic.Models;

namespace Genetic.Logic
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

        public override string ToString()
        {
            stringBuilder.Insert(0, "generationNumber;bestFitness;averageFitness;worstFitness;cache\n");
            return stringBuilder.ToString();
        }
    }
}