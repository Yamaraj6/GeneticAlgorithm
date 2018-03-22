namespace Genetic.Models
{
    public interface IGenotype<GeneKey, GeneValue>
    {
        IGene<GeneKey, GeneValue> GetGene(int positon);
        void SwapGenes(int positonOne, int positonTwo);
        int GetGenotypeSize();
        void Mutates();
    }
}
