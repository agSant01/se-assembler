using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assembler.Parsing;
using Assembler.Assembler;
using Assembler.Parsing.InstructionFormats;
using System.IO;
using System;
using Assembler.Parsing.InstructionItems;
using System.Collections.Generic;
using Assembler.Utils;

namespace Assembler.UnitTests.CompilerTests
{
    [TestClass]
    public class AssemblerTests
    {

        Lexer lexer;

        private readonly string testFileSuccess = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\assembly_test.txt");
        private readonly string testFileErrorToken = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\assembly_test_syntax_error.txt");

        [TestMethod]
        public void CompilerTest_GetLabelsAndConstants()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileSuccess);

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines);

            Parser parser = new Parser(lexer);

            Compiler compiler = new Compiler(parser);

            compiler.Compile();

            foreach (KeyValuePair<string, int> p in compiler.GetConstants())
            {
                Console.WriteLine($"constants {p.Key}: {p.Value}");
            }

            foreach (KeyValuePair<string, int> p in compiler.GetLabels())
            {
                Console.WriteLine($"labels {p.Key}: {p.Value}");
            }
            foreach (KeyValuePair<string, int> p in compiler.GetVariables())
            {
                Console.WriteLine($"Variables {p.Key}: {p.Value}");
            }
        }

        [TestMethod]
        public void CompilerTest_DesimalOutput()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileSuccess);

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines);

            Parser parser = new Parser(lexer);

            Assert.Fail("Add logger to compiler for test");
            Compiler compiler = new Compiler(parser);

            compiler.Compile();
            int[] arr = compiler.GetDecimalInstructions();
            for(int i = 0; i < compiler.Size(); i++)
            {
                Console.WriteLine($"instruction: {arr[i]}");
            }
        }

        [TestMethod]
        public void CompilerTest_HexOutput()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileSuccess);

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines);

            Parser parser = new Parser(lexer);

            Compiler compiler = new Compiler(parser);

            compiler.Compile();
            foreach (string s in compiler.GetOutput())
                Console.WriteLine(s);
        }

    }
}
