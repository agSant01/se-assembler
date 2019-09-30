using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace Assembler
{
    public class FileManager
    {
        //public FileManager()
        //{
           
        //    string filePath = @"C:\Users\sgrib\Desktop\example.txt";

        //    List<string> fileLines = File.ReadAllLines(filePath).ToList();

        //    foreach (var line in fileLines)
        //    {
        //        Console.WriteLine(line);
        //    }
        //}

        public void ToReadFile(string fileName)
        {
            List<string> fileLines = File.ReadAllLines(fileName).ToList();
            foreach (var line in fileLines)
            {
                string filePath = @"C:\Users\sgrib\Desktop\example.txt";
                File.WriteAllText(filePath, line);
            }
            
        }

        public void ToWriteFile(string text)
        {
            string filePath = @"C:\Users\sgrib\Desktop\example.txt";
            File.WriteAllText(filePath, text);
        }

        public void LoggerFile(string fileName, string message)
        {
            string filePath = @"C:\Users\sgrib\Desktop\"+ fileName;
         
            File.AppendAllText(filePath, message);
        }

    }
}
