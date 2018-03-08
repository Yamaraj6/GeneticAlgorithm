using System;

namespace Genetic
{
    public static class ExtensionMethods
    {
        public static string SquareMatrixToString(this int[,] matrix)
        {
            return matrix.MatrixToString((int)Math.Sqrt(matrix.Length), (int)Math.Sqrt(matrix.Length));
        }

        public static string MatrixToString(this int[,] matrix, int numberOfColumns, int numberOfRows)
        {
            var txtMatrix = "";
            for (int y = 0; y < numberOfColumns; y++)
            {
                for (int x = 0; x < numberOfRows; x++)
                {
                    txtMatrix+=matrix[x, y] + " ";
                }
                txtMatrix += "\n";
            }
            return txtMatrix;
        }
    }
}
