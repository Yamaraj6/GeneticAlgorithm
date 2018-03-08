using System;
using System.Collections.Generic;
using System.Text;
using Genetic.FileOperations;
using Genetic.Models;

namespace Genetic.Logic
{
    public class GeneticAlgorithm
    {
        public int populationSize { get; private set; }         // popSize
        public float mutationProb { get; private set; }         // Pm - mutation probability
        public float crossoverProb { get; private set; }        // Px - crossover probability
        public int tour { get; private set; }                   //  number of candidate parents
        public int generationsNumber { get; private set; }    // gen

        private List<Generation> generations;
        private MatrixData matrixData;


        public GeneticAlgorithm(int populationSize, float mutationProb, float crossoverProb,
            int tour, int numberOfGenerations, string fileName)
        {
            this.populationSize = populationSize;
            this.mutationProb = mutationProb;
            this.crossoverProb = crossoverProb;
            this.tour = tour;
            this.generationsNumber = numberOfGenerations;

            var _matrices = MatrixReader.GetMatrices(fileName);
            this.matrixData = new MatrixData(_matrices[0], _matrices[1]);
        }

        public void Start()
        {
            generations = new List<Generation>();
            generations.Add(new Generation(populationSize, matrixData));
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("generationNumber;bestFitness;averageFitness;worstFitness\n");
            stringBuilder.Append(generations[0].ToString());
            for (int i=1; i<generationsNumber;i++)
            {
                generations.Add(generations[i - 1].MakeGeneration(mutationProb, crossoverProb, tour));
                stringBuilder.Append(generations[i].ToString());
            }
            DataWriter.SaveDataToCsv(stringBuilder.ToString(),"plik"+(new Random().Next()%100));
        }
        
    }
}