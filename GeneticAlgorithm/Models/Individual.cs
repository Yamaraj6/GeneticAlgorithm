using System;

namespace Genetic.Models
{
    public class Individual
    {
        public const int PLACES_ROW = 0;
        public const int FACTORIES_ROW = 1;
        public const int ROWS_COUNT = 2;
        public const int MULTIPLIER_AMOUNT_OF_DRAWS = 2;


        public int[,] genotype { get; private set; }  // factoriesOnLocations
        public float fitness { get; private set; }
        private MatrixData matrixData;

        public Individual(MatrixData matrixData)
        {
            this.matrixData = matrixData;
            RandomIndividual();
            CalculateFitness();
        }

        public Individual(int[,] genotype, MatrixData matrixData)
        {
            this.matrixData = matrixData;
            this.genotype = genotype;
            CalculateFitness();
        }
        
        private void RandomIndividual()
        {
            CreateDefaultIndividual();

            for (int i = 0; i < matrixData.size * MULTIPLIER_AMOUNT_OF_DRAWS; i++)
            {
                Mutates();
            }
        }

        public void Mutates()
        {
            Random _random = new Random();

            int[] _places = new int[2];
            _places[0] = _random.Next() % matrixData.size;

            do
            {
                _places[1] = _random.Next() % matrixData.size;
            }
            while (_places[0] == _places[1]);

            var a = genotype[_places[0], FACTORIES_ROW];
            genotype[_places[0], FACTORIES_ROW] = genotype[_places[1], FACTORIES_ROW];
            genotype[_places[1], FACTORIES_ROW] = a;
        }

        private void CreateDefaultIndividual()
        {
            genotype = new int[matrixData.size, ROWS_COUNT];

            for (int y = 0; y < ROWS_COUNT; y++)
            {
                for (int x = 0; x < matrixData.size; x++)
                {
                    genotype[x, y] = x;
                }
            }
        }

        private void CalculateFitness()
        {
            fitness = 0;

            for (int y = 0; y < matrixData.size; y++)
            {
                for (int x = y + 1; x < matrixData.size; x++)
                {
                    fitness += matrixData.distance[x, y] *
                        (matrixData.flow[genotype[y, 1], genotype[x, 1]] +
                        matrixData.flow[genotype[x, 1], genotype[y, 1]]);
                }
            }
        }

        public bool IsBetterFitness(float othersFitness)
        {
            return fitness < othersFitness;
        }

        override public string ToString()
        {
            return "Fitness: " + fitness + "\n" + 
                genotype.MatrixToString(2, matrixData.size);
        }
    }
}