using System;
using System.Collections.Generic;
using System.IO;

namespace FileOperations.Matrices
{
    /* File Format:
    12

     0  1  2  2  3  4  4  5  3  5  6  7
     1  0  1  1  2  3  3  4  2  4  5  6
     2  1  0  2  1  2  2  3  1  3  4  5
     2  1  2  0  1  2  2  3  3  3  4  5
     3  2  1  1  0  1  1  2  2  2  3  4
     4  3  2  2  1  0  2  3  3  1  2  3
     4  3  2  2  1  2  0  1  3  1  2  3
     5  4  3  3  2  3  1  0  4  2  1  2
     3  2  1  3  2  3  3  4  0  4  5  6
     5  4  3  3  2  1  1  2  4  0  1  2
     6  5  4  4  3  2  2  1  5  1  0  1
     7  6  5  5  4  3  3  2  6  2  1  0

     0  3  4  6  8  5  6  6  5  1  4  6
     3  0  6  3  7  9  9  2  2  7  4  7
     4  6  0  2  6  4  4  4  2  6  3  6
     6  3  2  0  5  5  3  3  9  4  3  6
     8  7  6  5  0  4  3  4  5  7  6  7
     5  9  4  5  4  0  8  5  5  5  7  5
     6  9  4  3  3  8  0  6  8  4  6  7
     6  2  4  3  4  5  6  0  1  5  5  3
     5  2  2  9  5  5  8  1  0  4  5  2
     1  7  6  4  7  5  4  5  4  0  7  7
     4  4  3  3  6  7  6  5  5  7  0  9
     6  7  6  6  7  5  7  3  2  7  9  0
     */
    public class MatricesReader
    {
        public static List<int[,]> GetMatrices(string fileName)
        {
            var _matrices = new List<int[,]>();
            string _matrixText = LoadFile(fileName);
            var _matrixCounter = 0;

            int _matrixSize = GetMatrixSize(_matrixText, ref _matrixCounter);

            _matrices.Add(GetMatrix(_matrixText, _matrixSize, ref _matrixCounter));
            _matrices.Add(GetMatrix(_matrixText, _matrixSize, ref _matrixCounter));

            return _matrices;
        }

        private static int GetMatrixSize(string matrixText, ref int matrixCounter)
        {
            if (matrixText.Length > matrixCounter)
            {
                var _matrixSize = "";

                while (matrixText[matrixCounter] != '\n')
                {
                    if (Char.IsNumber(matrixText[matrixCounter]))
                    {
                        _matrixSize += matrixText[matrixCounter];
                    }
                    matrixCounter++;
                }
                return Convert.ToInt32(_matrixSize);
            }
            return -1;
        }

        private static int[,] GetMatrix(string matrixText, int matrixSize, ref int matrixCounter)
        {
            if (matrixText.Length > matrixCounter)
            {
                var matrix = new int[matrixSize, matrixSize];

                string tempNumber = "";
                var x = 0;
                var y = 0;

                while (matrixText.Length > matrixCounter && matrixSize > y)
                {
                    while (matrixText.Length > matrixCounter && matrixSize > x)
                    {
                        if (Char.IsNumber(matrixText[matrixCounter]))
                        {
                            tempNumber += matrixText[matrixCounter];
                        }
                        else if (tempNumber != "")
                        {
                            matrix[x, y] = Convert.ToInt32(tempNumber);
                            tempNumber = "";
                            x++;
                        }
                        matrixCounter++;
                    }

                    if (x >= matrixSize)
                    {
                        y++;
                    }
                    matrixCounter++;
                    x = 0;
                }

                return matrix;
            }
            return null;
        }

        private static string LoadFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    String fileText = sr.ReadToEnd();
                    return fileText;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}