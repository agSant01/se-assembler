using System;
using System.IO;
using System.Collections.Generic;
<<<<<<< HEAD
using Assembler.Parsing;

namespace Assembler
{
    namespace Assembler.Parsing
    {
        //<summary>
        //Class dedicated to presenting the user with temporary input of a file
        //and parser for said file to generate object code for assembler language.
        //</summary>
        public class Shell
        {
            //Parser and lexer for sifting through the file
            private Parser parser;
            private Lexer lexer;
            private Compiler compiler;
            private string path;

            ///<summary>Constructor that receives a string of the file to be parsed.
            ///</summary>
            /// <param name="filename"> Name of the file from current directory to be parsed.</param>
            public Shell(string filename)
            {
                path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, @filename);
                try
                {
                    this.lexer = new Lexer(FileManager.Instance.ToReadFile(path));
                    this.parser = new Parser(this.lexer);
                    this.compiler = new Compiler(this.parser);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Couldn't open file....");
                }

            }

            ///<summary>
            ///Prints the contents of the provided file into stdout.
            ///</summary>

            public void outputFile()
            {
                if (this.lexer.Equals(null))
                    throw new FileNotFoundException();


                Console.WriteLine("Provided File's Contents:\n");
                while (this.lexer.MoveNext())
                {
                    Console.WriteLine(this.lexer.CurrrentToken);
                }

                Console.WriteLine("End of File...\n");
            }

            private void writeObjectCodeFile()
            {
                this.compiler.Compile();
                string[] lines = compiler.GetOutput();

                Console.WriteLine("\nOuput file contents:");
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine("\n");

                System.IO.File.WriteAllLines(this.path + ".out", lines);
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

                try
                {
                    shell.outputFile();
                    shell.writeObjectCodeFile();
                }
                catch (FileNotFoundException) { Console.WriteLine("Couldn't open file...."); }


            }
        }

=======

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
            path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, @filename);
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

            while(val.Equals(null) || val.Trim().Equals(""))
            {
                val = Console.ReadLine();
            }
                

            Shell shell = new Shell(val);

            try { shell.outputFile(); }
            catch(FileNotFoundException){Console.WriteLine("Couldn't open file....");}
            

        }
>>>>>>> master
    }

}