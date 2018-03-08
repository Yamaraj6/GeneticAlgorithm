using System;

namespace Genetic.Models
{
    public class MatrixData
    {
        public int size { get; private set; }
        public int[,] distance { get; private set; }
        public int[,] flow { get; private set; }

        public MatrixData(int[,] distance, int[,] flow)
        {
            this.distance = distance;
            this.flow = flow;
            this.size = (int)Math.Sqrt(distance.Length);
        }
    }
}