using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace Assembler
{
    public class FileManager
    {
        public FileManager()
        {
            string filePath = @"C:\Users\sgrib\Desktop\example.txt";

            List<string> fileLines = File.ReadAllLines(filePath).ToList();

            foreach (var line in fileLines)
            {
                Console.WriteLine(line);
            }
        }

    }
}
