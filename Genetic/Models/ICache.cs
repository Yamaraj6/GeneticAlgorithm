namespace Genetic.Models
{
    public interface ICache<GeneKey, GeneValue>
    {
        int GetProblemSize();

        bool TryGetFitness(IGenotype<GeneKey, GeneValue> genotype, out float fitness);
        void AddCalculatedFitness(string genotype, float fitness);
        int GetCalculatedFitnessesCount();

        int GetRandomNext();
        double GetRandomNextDouble();
    }
}
