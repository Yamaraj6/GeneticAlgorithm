using Genetic.Logic;
using System;
using Genetic.Models;
using Genetic.FileOperations;

namespace Genetic
{
    class Program
    {
        static void Main(string[] args)
        {
            var geneticParameters = new GeneticParameters
            {
                generationsCount = 1000,
                populationSize = 100,
                crossoverProb = 0.99f,
                mutationProb = 0.01f,
                selectionType = SelectionType.RouletteSelection,
                tournamentSize = 5
            };

            var fileName = "had20.dat";
            int seed = Environment.TickCount;
            Console.WriteLine("SEED? n\\y");
            char c = Console.ReadKey().KeyChar;
            if (c == 'y')
            {
                Console.WriteLine("\nSEED: ");
                seed = Convert.ToInt32(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("\nSEED: "+seed);
            }

            var _matrices = DataReader.GetMatrices(fileName);
            var cache = new Cache(_matrices[0], _matrices[1], new Random(seed));

            var geneticAlgorithm = new GeneticAlgorithm<int, int>(geneticParameters, cache);

            var firstIndividual = new Individual(geneticAlgorithm.cache);
            geneticAlgorithm.CreateSpecies(firstIndividual);

            DataWriter.SaveDataToCsv(geneticAlgorithm.ToString(), "plik" + (new Random().Next() % 100));
        }
    }
}