using Assembler.Assembler;
using Assembler.Parsing;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

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
        private readonly string test1Comparison = Path.Combine(
           Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
           @"CompilerTests\AssemblerTestsComparisons\test1Comparison.txt");

        private readonly string test2 = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\test2.txt");
        private readonly string test2Comparison = Path.Combine(
          Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
          @"CompilerTests\AssemblerTestsComparisons\test2Comparison.txt");

        private readonly string test3 = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\test3.txt");
        private readonly string test3Comparison = Path.Combine(
          Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
          @"CompilerTests\AssemblerTestsComparisons\test3Comparison.txt");

        private readonly string testParsePush = Path.Combine(
           Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
           @"CompilerTests\PaserTestsComparisons\testFailParser.txt");


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
            for (int i = 0; i < compiler.Size(); i++)
            {
                Console.WriteLine($"instruction: {arr[i]}");
            }

            compiler.AsmLogger.Reset();
            while (compiler.AsmLogger.MoveNext())
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

            Console.WriteLine($"\nEnd of line output: {compiler.GetOutput()[compiler.GetOutput().Length - 1]}");

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
            string[] asmLines = FileManager.Instance.ToReadFile(test1);
            string[] expectedAsmLines = FileManager.Instance.ToReadFile(test1Comparison);

            Lexer lexer = new Lexer(asmLines);

            Parser parser = new Parser(lexer);

            Compiler compiler = new Compiler(parser);

            compiler.Compile();

            int counter = 0;
            foreach (string l in compiler.GetOutput())
            {
                Console.WriteLine(l);
                Assert.AreEqual(expectedAsmLines[counter++], l);
            }
        }

        [TestMethod]
        public void CompilerTest_ProfTest2_Success()
        {
            string[] asmLines = FileManager.Instance.ToReadFile(test2);
            string[] expectedAsmLines = FileManager.Instance.ToReadFile(test2Comparison);

            Lexer lexer = new Lexer(asmLines);

            Parser parser = new Parser(lexer);

            Compiler compiler = new Compiler(parser);

            compiler.Compile();

            int counter = 0;
            foreach (string l in compiler.GetOutput())
            {
                Console.WriteLine(l);
                Assert.AreEqual(expectedAsmLines[counter++], l);
            }
        }

        [TestMethod]
        public void CompilerTest_ProfTest3_Success()
        {
            string[] asmLines = FileManager.Instance.ToReadFile(test3);
            string[] expectedAsmLines = FileManager.Instance.ToReadFile(test3Comparison);

            Lexer lexer = new Lexer(asmLines);

            Parser parser = new Parser(lexer);

            Compiler compiler = new Compiler(parser);

            compiler.Compile();

            int counter = 0;
            foreach (string l in compiler.GetOutput())
            {
                Console.WriteLine(l);
                Assert.AreEqual(expectedAsmLines[counter++], l);
            }
        }


        [TestMethod]
        public void CompilerTest_ParseFailNullToken_Success()
        {
            string[] asmLines = FileManager.Instance.ToReadFile(testParsePush);

            Lexer lexer = new Lexer(asmLines);

            Parser parser = new Parser(lexer);

            Compiler compiler = new Compiler(parser);

            compiler.Compile();

            foreach (string l in compiler.GetOutput())
            {
                Console.WriteLine(l);
            }
        }
    }
}
