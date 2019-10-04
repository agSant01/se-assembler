using Assembler.Assembler;
using Assembler.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Assembler.FileManager;
namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {

            string val = "";
            Console.Write("File (with complete Path):");

            while (val.Equals(null) || val.Trim().Equals(""))
            {
                val = Console.ReadLine();
            }


            Shell shell = new Shell(val);

            try
            {
                shell.outputFile();
                shell.compiler.Compile();
                string[] lines = shell.compiler.GetOutput();
                shell.objectCodeOfFile(lines);
            }
            catch (FileNotFoundException) { Console.WriteLine("Couldn't open file...."); }

        }
    }

    public class Shell
    {
        //private FileManager fm;
        private Parser parser;
        private Lexer lexer;
        public Compiler compiler;
        private string path;
        public Shell(string filename)
        {
            path = @filename;
            try
            {
                this.lexer = new Lexer(FileManager.Instance.ToReadFile(path));
                this.parser = new Parser(this.lexer);
                this.compiler = new Compiler(parser);
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

        public bool objectCodeOfFile(string[] lines)
        {
            String outputPath = $@"{Path.GetDirectoryName(this.path)}\{Path.GetFileNameWithoutExtension(path)}_HEX_output.txt";
            Console.WriteLine("output located in : " + outputPath);
            bool result = FileManager.Instance.ToWriteFile(outputPath, lines);

            return result;
        }


        //public static void Main()
        //{
            

        //}

    }
}
