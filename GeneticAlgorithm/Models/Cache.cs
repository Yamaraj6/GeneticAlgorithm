using System;
using System.Collections.Generic;

namespace Genetic.Models
{
    public class Cache : ICache<int,int>
    { 
        private int size;
        private int[,] distance;
        private int[,] flow;

        private Dictionary<string, float> calculatedFitnesses;
        private Random random;

        public Cache(int[,] distance, int[,] flow, Random random)
        {
            this.distance = distance;
            this.flow = flow;
            this.size = (int)Math.Sqrt(distance.Length);
            this.random = random;
            calculatedFitnesses = new Dictionary<string, float>();
        }

        public int GetProblemSize() { return size; }

        public bool TryGetFitness(IGenotype<int,int> genotype, out float fitness)
        {
            var _fitnessIsCalculated = calculatedFitnesses.TryGetValue(genotype.ToString(), out fitness);
            if(!_fitnessIsCalculated)
            {
                for (int y = 0; y < genotype.GetGenotypeSize(); y++)
                {
                    for (int x = y + 1; x < genotype.GetGenotypeSize(); x++)
                    {
                        fitness += distance[x, y] *
                            (flow[genotype.GetGene(y).GetValue(), genotype.GetGene(x).GetValue()] +
                            flow[genotype.GetGene(x).GetValue(), genotype.GetGene(y).GetValue()]);
                    }
                }
            }
            return _fitnessIsCalculated;
        }

        public void AddCalculatedFitness(string genotype, float fitness)
        {
            calculatedFitnesses.Add(genotype, fitness);
        }

        public int GetCalculatedFitnessesCount() { return calculatedFitnesses.Count; }

        public int GetRandomNext() { return random.Next(); }

        public double GetRandomNextDouble() { return random.NextDouble(); }
    }
}