using Assembler.Assembler;
using Assembler.Parsing;
using System;
using System.IO;

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
            Console.Write("File (with complete Path): ");

            while (val.Equals(null) || val.Trim().Equals(""))
            {
                val = Console.ReadLine();
            }

            Shell shell = new Shell(val);

            try
            {
                shell.ExportFiles();
            }
            catch (Exception err) { Console.WriteLine($"Unexpected Error during runtime: \n\t'{err}'");
        }
    }

    public class Shell
    {
        //private FileManager fm;
        private Parser parser;
        private Lexer lexer;
        public Compiler compiler;
        private string fullFilePath;
        public AssemblyLogger logger;
        public Shell(string filePath)
        {
            this.fullFilePath = @filePath;
            try
            {
                this.logger = new AssemblyLogger(Path.GetFileNameWithoutExtension(fullFilePath));
                this.lexer = new Lexer(FileManager.Instance.ToReadFile(fullFilePath));
                this.parser = new Parser(this.lexer);
                this.compiler = new Compiler(parser, logger);
                this.compiler.Compile();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found...");
            }
        }

        public void ExportFiles()
        {
            string workingDirFullPath = Path.GetDirectoryName(this.fullFilePath);

            string objFileName = $"{Path.GetFileNameWithoutExtension(fullFilePath)}_OBJ_FILE.txt";

            logger.StatusUpdate("Writting Obj. file");
            Console.WriteLine("Writting Obj. file");
            Console.WriteLine($"Obj file located in: '{workingDirFullPath}'");

            FileManager.Instance.ToWriteFile(
                Path.Combine(workingDirFullPath, objFileName),
                compiler.GetOutput()
            );

            logger.StatusUpdate("Writting obj. file completed.");
            Console.WriteLine("Writting obj. file completed.");

            Console.WriteLine("Writting assembly log file file Completed.");
            logger.StatusUpdate("Writting assembly log file file Completed.");

            FileManager.Instance.ToWriteFile(logger, workingDirFullPath);

            Console.WriteLine("Exiting.");
        }
    }
}
