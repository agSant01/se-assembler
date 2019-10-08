using Assembler.Assembler;
using Assembler.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static Assembler.FileManager;
namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"
            ___  ___                                       
            |  \/  (_)                                      
            | .  . |_  ___ _ __ ___                         
            | |\/| | |/ __| '__/ _ \                        
            | |  | | | (__| | | (_) |                       
            \_|  |_/_|\___|_|  \___/                        
                                                
                                                
              ___                         _     _           
             / _ \                       | |   | |          
            / /_\ \___ ___  ___ _ __ ___ | |__ | | ___ _ __ 
            |  _  / __/ __|/ _ \ '_ ` _ \| '_ \| |/ _ \ '__|
            | | | \__ \__ \  __/ | | | | | |_) | |  __/ |   
            \_| |_/___/___/\___|_| |_| |_|_.__/|_|\___|_|  

            by: sin cafe

            Detect: Memory undefined variables, syntax error and memory overwrites
            Generate: object file and log file
            
            ");
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
            logger.StatusUpdate("Writting output file");
            String outputPath = $@"{Path.GetDirectoryName(this.path)}\{Path.GetFileNameWithoutExtension(path)}_HEX_output.txt";
            Console.WriteLine("output located in : " + outputPath);
            bool outResult = FileManager.Instance.ToWriteFile(outputPath, outLines);
            logger.StatusUpdate("Writting output file completed");
            logger.StatusUpdate("Writting log file");
            String logPath = $@"{Path.GetDirectoryName(this.path)}\{Path.GetFileNameWithoutExtension(path)}_report.log";
            Console.WriteLine("output located in : " + logPath);
            bool logResult = FileManager.Instance.ToWriteFile(logPath, logLines);
            logger.StatusUpdate("Writting output file Completed");


            return logResult & outResult;
        }


        //public static void Main()
        //{
            

        //}

    }
}
