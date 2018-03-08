using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Genetic.FileOperations
{
    class DataWriter
    {
        public static void SaveDataToCsv(string data, string fileName)
        {
            File.WriteAllText(Environment.GetFolderPath
                (Environment.SpecialFolder.DesktopDirectory) + 
                "\\"+ fileName + ".csv", data);
        }
    }
}