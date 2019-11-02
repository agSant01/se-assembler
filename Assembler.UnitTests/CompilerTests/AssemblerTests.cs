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

        private readonly string test1 = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\test1.txt");

        private readonly string test2 = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\test2.txt");

        private readonly string test3 = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\test3.txt");

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
        public void CompilerTest_DecimalOutput()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileSuccess);

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines);

            Parser parser = new Parser(lexer);

            Compiler compiler = new Compiler(parser);

            compiler.Compile();
            int[] arr = compiler.GetDecimalInstructions();
            for(int i = 0; i < compiler.Size(); i++)
            {
                Console.WriteLine($"instruction: {arr[i]}");
            }

            compiler.AsmLogger.Reset();
            while(compiler.AsmLogger.MoveNext())
            {
                Console.WriteLine(compiler.AsmLogger.Current);
            }
        }

        [TestMethod]
        public void CompilerTest_HexOutput()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileSuccess);

            string[] expectedLines =
            {
                "A8 06",
                "05 07",
                "00 00",
                "01 02",
                "02 03",
                "C9 40",
                "A8 12",
                "1A 04",
                "A8 16",
                "19 04",
                "0B 08",
                "A8 16"
            };

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines);

            Parser parser = new Parser(lexer);

            Compiler compiler = new Compiler(parser);

            compiler.Compile();
            int i = 0;
            Console.WriteLine(compiler.GetOutput().Length);
            foreach (string s in compiler.GetOutput())
            {
                Console.WriteLine(s);

                if (i < expectedLines.Length)
                {
                    Assert.AreEqual(expectedLines[i], s);
                }
                else
                {
                    if (i == (expectedLines.Length + 10))
                    {
                        break;
                    }
                }

                i++;
            }

            Console.WriteLine($"\nEnd of line output: {compiler.GetOutput()[compiler.GetOutput().Length-1]}");

            Assert.AreEqual("00 00", compiler.GetOutput()[compiler.GetOutput().Length - 1]);

            compiler.AsmLogger.Reset();
            while (compiler.AsmLogger.MoveNext())
            {
                Console.WriteLine(compiler.AsmLogger.Current);
            }
        }

        [TestMethod]
        public void CompilerTest_HexOutput_Error()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileErrorToken);

            string[] expectedLines =
            {
                "A8 06",
                "05 07",
                "00 00",
                "01 02",
                "02 03",
                "C9 40",
                "1A 04",
                "A8 12",
                "0B 08",
                "A8 12"
            };

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines);

            Parser parser = new Parser(lexer);

            Compiler compiler = new Compiler(parser);

            compiler.Compile();
            int i = 0;
            Console.WriteLine(compiler.GetOutput().Length);
            foreach (string s in compiler.GetOutput())
            {
                Console.WriteLine(s);

                if (i < expectedLines.Length)
                {
                    Assert.AreEqual(expectedLines[i], s);
                } else
                {
                    if (i == (expectedLines.Length + 10))
                    {
                        break;
                    }
                }

                i++;
            }

            Console.WriteLine($"\nEnd of line output: {compiler.GetOutput()[compiler.GetOutput().Length - 1]}");
            Assert.AreEqual("00 00", compiler.GetOutput()[compiler.GetOutput().Length - 1]);

            compiler.AsmLogger.Reset();
            while (compiler.AsmLogger.MoveNext())
            {
                Console.WriteLine(compiler.AsmLogger.Current);
            }
        }

        [TestMethod]
        public void CompilerTest_ProfTest1_Success()
        {

        }
    }
}
