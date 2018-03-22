
using Genetic.Models.FactoryPlacementProblem;

namespace GeneticAlgorithmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test(123456789);
         //   test.Start();
            test.StartOtherOptimizeMethods();
        }
    }
}