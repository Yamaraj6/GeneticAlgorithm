namespace Genetic
{
    public interface IIndividual<GeneKey, GeneValue>
    {
        IIndividual<GeneKey, GeneValue> CreateNewIndividual(ICache<GeneKey, GeneValue> cache);
        IIndividual<GeneKey, GeneValue> CreateNewIndividual(IGenotype<GeneKey, GeneValue> genotype, ICache<GeneKey, GeneValue> cache);
        float GetFitness();
        IGenotype<GeneKey, GeneValue> GetGenotype();
        int GetGenotypeSize();
        
        IIndividual<GeneKey, GeneValue> Crossover(IIndividual<GeneKey, GeneValue> parent);
        void Mutates();
        bool IsBetter(float othersFitness);
    }
}
