using Assembler.Parsing;
<<<<<<< HEAD
using Assembler.Assembler;
using Assembler.Parsing.InstructionFormats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using Assembler.Parsing.InstructionItems;
using System.Collections.Generic;
=======
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
>>>>>>> master

namespace Assembler.UnitTests
{
    [TestClass]
    public class ParserTests
    {
        Lexer lexer;

        private readonly string testFileSuccess =  Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"AssemblyFiles\assembly_test.txt");
        private readonly string testFileErrorToken = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"AssemblyFiles\assembly_test_syntax_error.txt");

        
        [TestCleanup]
        public void TestCleanup()
        {
            lexer = null;  
        }

        [TestMethod]
        public void ParserTests_GenerateInstructionTree_Success()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileSuccess);

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines);

            Parser parser = new Parser(lexer);

            while (parser.MoveNext())
            {
                Console.WriteLine(parser.CurrentInstruction);
<<<<<<< HEAD
                
            }


=======
            }
>>>>>>> master
        }

        [TestMethod]
        public void LexicalAnalyzer_TokenizeAssemblyFile_WithErrorTokens()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileErrorToken);

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines);

            Parser parser = new Parser(lexer);

            while (parser.MoveNext())
            {
                Console.WriteLine(parser.CurrentInstruction);
            }
        }
    }
}
