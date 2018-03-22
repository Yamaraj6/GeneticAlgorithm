using System;
using System.Collections.Generic;

namespace Genetic.Models.FactoryPlacementProblem
{
    public class Individual : IIndividual<int, int>
    {
        private IGenotype<int, int> genotype; // factoriesOnLocations
        private float fitness;
        private ICache<int, int> cache;

        public Individual(ICache<int, int> cache)
        {
            this.cache = cache;
            genotype = new Genotype(cache);
            fitness = CalculateFitness();
        }

        public Individual(IGenotype<int, int> genotype, ICache<int,int> cache)
        {
            this.cache = cache;
            this.genotype = genotype;
            fitness = CalculateFitness();
        }

        public IIndividual<int, int> CreateNewIndividual(ICache<int, int> cache)
        {
            return new Individual(cache);
        }

        public IIndividual<int, int> CreateNewIndividual(IGenotype<int, int> genotype, ICache<int, int> cache)
        {
            return new Individual(genotype, cache);
        }

        public IGenotype<int,int> GetGenotype() {return genotype; }
        public int GetGenotypeSize() { return genotype.GetGenotypeSize(); }
        public float GetFitness() { return fitness; } 

        public IIndividual<int, int> Crossover(IIndividual<int, int> parent)
        {
            int _crossoverPoint = cache.GetRandomNext() % parent.GetGenotypeSize();
            var _childGenotype = new List<IGene<int, int>>();
            for (int i = 0; i < GetGenotypeSize(); i++)
            {
                if (i < _crossoverPoint)
                {
                    _childGenotype.Add(new Gene<int, int>
                        (genotype.GetGene(i).GetKey(),
                        genotype.GetGene(i).GetValue()));
                }
                else
                {
                    _childGenotype.Add(new Gene<int, int>
                        (parent.GetGenotype().GetGene(i).GetKey(),
                        parent.GetGenotype().GetGene(i).GetValue()));
                }
            }
            return new Individual(new Genotype(_childGenotype, 0, cache), cache);
        }

        public void Mutates()
        {
            genotype.Mutates();
            fitness = CalculateFitness();
        }

        private float CalculateFitness()
        {
            float _fitness = 0;

            if (cache.TryGetFitness(genotype, out _fitness))
            {
                return _fitness;
            }
            
            cache.AddCalculatedFitness(genotype.ToString(), _fitness);
            return _fitness;
        }

        public bool IsBetter(float othersFitness)
        {
            return fitness < othersFitness;
        }

        override public string ToString()
        {
            return "Fitness: " + fitness + "\n" + 
                genotype.ToString();
        }
    }
}