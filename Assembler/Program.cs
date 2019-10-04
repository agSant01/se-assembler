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
                string[] outLines = shell.compiler.GetOutput();
                string[] logLines = shell.logger.GetLines();
                shell.objectCodeOfFile(outLines, logLines);
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
        public AssemblyLogger logger;
        public Shell(string filename)
        {
            path = @filename;
            try
            {
                this.logger = new AssemblyLogger();
                this.lexer = new Lexer(FileManager.Instance.ToReadFile(path));
                this.parser = new Parser(this.lexer);
                this.compiler = new Compiler(parser,logger);
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

        public bool objectCodeOfFile(string[] outLines, string[] logLines)
        {
            String outputPath = $@"{Path.GetDirectoryName(this.path)}\{Path.GetFileNameWithoutExtension(path)}_HEX_output.txt";
            Console.WriteLine("output located in : " + outputPath);
            bool outResult = FileManager.Instance.ToWriteFile(outputPath, outLines);

            String logPath = $@"{Path.GetDirectoryName(this.path)}\{Path.GetFileNameWithoutExtension(path)}_report.log";
            Console.WriteLine("output located in : " + logPath);
            bool logResult = FileManager.Instance.ToWriteFile(logPath, logLines);

            return logResult & outResult;
        }


        //public static void Main()
        //{
            

        //}

    }
}
