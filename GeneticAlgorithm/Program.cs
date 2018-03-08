using Genetic.Logic;

namespace Genetic
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine(a.ToString());
            var popSize = 100;
            var Px = 0.95f;
            var Pm = 0.01f;
            var tour = 5;
            var gene = 1000;
            var fileName = "had16.dat";

            GeneticAlgorithm genetic = new GeneticAlgorithm(
                popSize, Pm, Px, tour, gene, fileName);
            genetic.Start();
        }
    }
}