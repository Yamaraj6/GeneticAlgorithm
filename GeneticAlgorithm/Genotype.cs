using Genetic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic
{
    public class Genotype : IGenotype<int,int>
    {
        public ICache<int, int> cache { get; private set; }
        public List<IGene<int, int>> genotype { get; private set; }

        public Genotype(ICache<int, int> cache)
        {
            this.cache = cache;
            CreateRandomGenotype();
        }

        public Genotype(List<IGene<int, int>> genotype, int crossoverPoint, ICache<int, int> cache)
        {
            this.cache = cache;
            this.genotype = genotype;
            FixGenotype(crossoverPoint);
        }

        private void CreateRandomGenotype()
        {
            genotype = new List<IGene<int, int>>();
            for (int i = 0; i < cache.GetProblemSize(); i++)
            {
                genotype.Add(new Gene<int, int>(i, i));
            }
            for (int i = 0; i < genotype.Count; i++)
            {
                SwapGenes(i, cache.GetRandomNext() % genotype.Count);
            }
        }

        public int GetGenotypeSize()
        {
            return genotype.Count;
        }

        public IGene<int, int> GetGene(int positon)
        {
            return genotype[positon];
        }

        public void Mutates()
        {
            SwapGenes(cache.GetRandomNext() % genotype.Count,
                cache.GetRandomNext() % genotype.Count);
        }

        public void SwapGenes(int positonOne, int positonTwo)
        {
            var _tempGeneValue = genotype[positonOne].GetValue();
            genotype[positonOne].SetValue(genotype[positonTwo].GetValue());
            genotype[positonTwo].SetValue(_tempGeneValue);
        }

        private void FixGenotype(int crossoverPoint)
        {
            for (int i = crossoverPoint; i < genotype.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (genotype[i].GetValue() == genotype[j].GetValue())
                    {
                        genotype[i].SetValue(FindMissingGeneValue());
                        break;
                    }
                }
            };
        }

        private int FindMissingGeneValue()
        {
            for (int i = 0; i < genotype.Count; i++)
            {
                for (int j = 0; i != genotype[j].GetValue();)
                {
                    j++;
                    if (j == genotype.Count)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public override string ToString()
        {
            var _genotype = "";
            foreach (var gene in genotype)
            {
                _genotype += gene.GetValue() + ";";
            }
            return _genotype;
        }
    }
}