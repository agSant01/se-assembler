using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using static Assembler.FileManager;

namespace Assembler.UnitTests
{
   public class FileManagerTest
    {
        private FileManager fileManager;

        [TestInitialize]
        public void TestInit()
        {
            fileManager = new FileManager();
        }

        [TestMethod]
        public void FileManagerTester_ReadFile_ShowTextAsOutput()
        {
           string nameOfFile = "mock.txt";
        ToReadFile(nameOfFile);
           
            //string filePath = @"C:\Users\sgrib\Desktop\example.txt";

            //List<string> fileLines = File.ReadAllLines(filePath).ToList();

            //foreach (var line in fileLines)
            //{
            //    Console.WriteLine(line);
            //}
        }


    }
}
