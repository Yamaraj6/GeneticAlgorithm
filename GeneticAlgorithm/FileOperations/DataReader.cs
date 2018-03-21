using System;
using System.Collections.Generic;
using System.IO;

namespace Genetic.FileOperations
{
    class DataReader
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

        private static int[,] GetMatrix(string matrixText, int matrixSize,ref int matrixCounter)
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