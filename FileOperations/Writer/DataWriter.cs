using System;
using System.IO;

namespace FileOperations.Writer
{
    public class DataWriter
    {
        public static void SaveDataToCsv(string data, string fileName)
        {
            File.WriteAllText(Environment.GetFolderPath
                (Environment.SpecialFolder.DesktopDirectory)+"\\GenticAlgorithmResults" + 
                "\\"+ fileName + ".csv", data);
        }
    }
}