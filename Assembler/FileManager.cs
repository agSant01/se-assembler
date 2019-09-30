/* 
     * The purpose of this FileManager class is to be able to handle
     * file actions. Here we have three methods. One for reading from a file, 
     * one for writing to a file and a last one that acts as the logger file
     * to keep track of activity.
     * We are using the File Class (System.IO) */
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

        /* 
         * ToReadFile accepts only one parameter and that is the name of the file to read.
         * This file name must include its extension.
         *  It takes the file and makes a list of its lines, then goes through every line 
         * of the list and rewrites the folder */

        public static void ToReadFile(string fileName)
        {
            List<string> fileLines = File.ReadAllLines(fileName).ToList();
            foreach (var line in fileLines)
            {       
                //string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;
                File.WriteAllText(fileName, line);
            }
       
        }

        /* 
         * ToWriteFile accepts only one parameter and that is a text you want to 
         * write to a file.  First it looks for the current directory you are in 
         * and creates a folder called example to store the info there. */

        public void ToWriteFile(string text)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\example.txt";
            File.WriteAllText(filePath, text);
        }

        /* 
         * LoggerFile accepts two parameters. One is the name of the file and
         * the other is the message to be logged. The name of the file must
         * include its extension. First, it takes the path in which you are
         * currently in and saves the file there to append the message/logs to it. */
        public void LoggerFile(string fileName, string message)
        {
          
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;
            File.AppendAllText(filePath, message);
        }

    }
}
