using FileOperations.Matrices;
using FileOperations.Writer;
using Genetic;
using Genetic.Models.FactoryPlacementProblem;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithmTest
{
    class Test
    {
        private const int NUMBER_OF_STARTS = 10;

        private GeneticParameters geneticParameters;
        private string[] fileNames = { "had12", "had14", "had16", "had18", "had20", "had30" };
        private Cache[] caches;
        private List<float[]> averageFitnesses;
        private List<float[]> standardDeviation;
        private int seed;

        public Test()
        {
            seed = Environment.TickCount;
            ReadMatrices();
        }

        public Test(int seed)
        {
            this.seed = seed;
            ReadMatrices();
        }

        private void ReadMatrices()
        {
            caches = new Cache[fileNames.Length];
            for (int i = 0; i < fileNames.Length; i++)
            {
                var _matrices = MatricesReader.GetMatrices(fileNames[i]+".dat");
                caches[i] = new Cache(_matrices[0], _matrices[1], new Random(seed));
            }
        }

        public void Start()
        {
            CreateGeneticParameters();
            var geneticAlgorithm = new GeneticAlgorithm<int, int>[NUMBER_OF_STARTS];
            for (int i = 0; i < fileNames.Length; i++)
            {  
                for (int j = 0; j < NUMBER_OF_STARTS; j++)
                {
                    geneticAlgorithm[j] = new GeneticAlgorithm<int, int>(geneticParameters, caches[i]);
                    var firstIndividual = new Individual(caches[i]);
                    geneticAlgorithm[j].CreateSpecies(firstIndividual);
                   // DataWriter.SaveDataToCsv(geneticAlgorithm[j].ToString(), fileNames[i] + "_" + j);
                }
             //   DataWriter.SaveDataToCsv(geneticAlgorithm[0].ToString(), fileNames[i] +"lel");
                CountAverageFitnessesForFewStarts(geneticAlgorithm);
                SaveFitnessesToFile(i);
            }
        }

        public void StartOtherOptimizeMethods()
        {
        //    var sb = new StringBuilder();
            var sb2 = new StringBuilder();
            for (int i = 0; i < fileNames.Length; i++)
            {
          //      sb.Append(fileNames[i] + ";" + OptimizationMethods.RandomSearch(100000, caches[i]).GetFitness());
                sb2.Append(fileNames[i] + ";" + OptimizationMethods.GreedySearch(10000, caches[i]).GetFitness() + "\n");
            }
          //  DataWriter.SaveDataToCsv(sb.ToString(), "RandomSearch");
            DataWriter.SaveDataToCsv(sb2.ToString(), "GreedySearch");
        }

        private void CountAverageFitnessesForFewStarts(GeneticAlgorithm<int, int>[] geneticAlgorithm)
        {
            averageFitnesses = new List<float[]>();
            standardDeviation = new List<float[]>();

            for (int k = 0; k < geneticParameters.generationsCount; k++)
            {
                float[] _avrageFitnesses = { 0, 0, 0 };
                for (int j = 0; j < NUMBER_OF_STARTS; j++)
                {
                    var _temp = geneticAlgorithm[j].GetGenerationFitnesses(k);
                    _avrageFitnesses[0] += _temp[0];
                    _avrageFitnesses[1] += _temp[1];
                    _avrageFitnesses[2] += _temp[2];
                }
                _avrageFitnesses[0] /= NUMBER_OF_STARTS;
                _avrageFitnesses[1] /= NUMBER_OF_STARTS;
                _avrageFitnesses[2] /= NUMBER_OF_STARTS;

                float[] _standardDeviation= { 0, 0, 0 };
                for (int j = 0; j < NUMBER_OF_STARTS; j++)
                {
                    var _temp = geneticAlgorithm[j].GetGenerationFitnesses(k);
                    _standardDeviation[0] += MathF.Pow((_temp[0] - _avrageFitnesses[0]),2);
                    _standardDeviation[1] += MathF.Pow((_temp[1] - _avrageFitnesses[1]),2);
                    _standardDeviation[2] += MathF.Pow((_temp[2] - _avrageFitnesses[2]),2);
                }
                _standardDeviation[0] = MathF.Sqrt(_standardDeviation[0] / NUMBER_OF_STARTS);
                _standardDeviation[1] = MathF.Sqrt(_standardDeviation[1] / NUMBER_OF_STARTS);
                _standardDeviation[2] = MathF.Sqrt(_standardDeviation[2] / NUMBER_OF_STARTS);

                averageFitnesses.Add(_avrageFitnesses);
                standardDeviation.Add(_standardDeviation);
            }
        }

        private void SaveFitnessesToFile(int fileNumber)
        {
            var sb = new StringBuilder();
            sb.Insert(0, "generationNumber;bestFitness;averageFitness;worstFitness;sdBestFitness;sdAverageFitness;sdWorstFitness\n");
            for(int i =0;  i<averageFitnesses.Count;i++)
            {
                sb.Append((i+1)+";"+(averageFitnesses[i])[0] + ";" + averageFitnesses[i][1] + ";" + averageFitnesses[i][2] + ";"
                    +standardDeviation[i][0] + ";" + standardDeviation[i][1] + ";" + standardDeviation[i][2] + "\n");
            }
            DataWriter.SaveDataToCsv(sb.ToString(), fileNames[fileNumber]);
        }

        private void CreateGeneticParameters()
        {
            geneticParameters = new GeneticParameters
            {
                generationsCount = 100,
                populationSize = 100,
                crossoverProb = 0.8f,
                mutationProb = 0.5f,
                selectionType = SelectionType.TournamentSelection,
                tournamentSize = 5
            };
        }

    }
}
