using Assembler.Assembler;
using Assembler.Parsing;
using Assembler.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assembler.Core
{
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

