﻿using System;
using System.IO;
using System.Collections.Generic;

using Assembler.Parsing;

namespace Assembler
{



    namespace Assembler.Parsing
    {

        public class Shell
        {
            //private FileManager fm;
            private Parser parser;
            private Lexer lexer;

            private string path;
            public Shell(string filename)
            {
                path =  @filename;
                try
                {
                    this.lexer = new Lexer(FileManager.Instance.ToReadFile(path));
                    this.parser = new Parser(this.lexer);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Couldn't open file....");
                }

            }

            public void outputFile()
            {
                if (this.lexer.Equals(null))
                    throw new FileNotFoundException();


                Console.WriteLine("Provided File's Contents:\n");
                while (this.lexer.MoveNext())
                {
                    Console.WriteLine(this.lexer.CurrrentToken);
                }

                Console.WriteLine("\n");
            }


            public static void Main()
            {
                string val = "";
                Console.Write("File in Current Dir:");

                while (val.Equals(null) || val.Trim().Equals(""))
                {
                    val = Console.ReadLine();
                }


                Shell shell = new Shell(val);

                try { shell.outputFile(); }
                catch (FileNotFoundException) { Console.WriteLine("Couldn't open file...."); }


            }

        }

    }

}