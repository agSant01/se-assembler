using Assembler.Parsing;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Assembler.UnitTests.CompilerTests
{
    [TestClass]
    public class ParserTests
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
            @"CompilerTests\PaserTestsComparisons\test1.txt");

        private readonly string test2 = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\test2.txt");
        private readonly string test2Comparison = Path.Combine(
           Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
           @"CompilerTests\PaserTestsComparisons\test2.txt");

        private readonly string test3 = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\test3.txt");
        private readonly string test3Comparison = Path.Combine(
           Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
           @"CompilerTests\PaserTestsComparisons\test3.txt");

        private readonly string testParsePush = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\PaserTestsComparisons\testFailParser.txt");

        private readonly string test3Call = Path.Combine(
           Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
           @"CompilerTests\HappyHourTests\test3_Asmbly.txt");

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

            string[] expected =
            {
                "ORIGIN[op: 'org', address: '0', valid: 'True']",
                "IF3[op: 'JMPADDR', Const/Address: 'start', valid: 'True']",
                "VariableAssign[op: db, name: valor1, values: [5], valid: 'True']",
                "VariableAssign[op: db, name: valor2, values: [7], valid: 'True']",
                "VariableAssign[op: db, name: mayor, values: [0], valid: 'True']",
                "ConstantAssign[op:const, name: ten, value: 0A, valid: 'True']",
                "Label[name: start, valid: 'True']",
                "IF2[op: 'LOAD', Ra: 'R1', Const/Address: 'valor1', valid: 'True']",
                "IF2[op: 'LOAD', Ra: 'R2', Const/Address: 'valor2', valid: 'True']",
                "IF1[op: 'GRT', Ra: 'R1', Rb: 'R2', Rc: '', valid: 'True']",
                "IF3[op: 'JMPADDR', Const/Address: 'R1esMayor', valid: 'True']",
                "IF2[op: 'STORE', Ra: 'R2', Const/Address: 'mayor', valid: 'True']",
                "IF3[op: 'JMPADDR', Const/Address: 'fin', valid: 'True']",
                "Label[name: R1esMayor, valid: 'True']",
                "IF2[op: 'STORE', Ra: 'R1', Const/Address: 'mayor', valid: 'True']",
                "IF2[op: 'LOADIM', Ra: 'R3', Const/Address: '#8', valid: 'True']",
                "Label[name: fin, valid: 'True']",
                "IF3[op: 'JMPADDR', Const/Address: 'fin', valid: 'True']"
            };

            lexer = new Lexer(lines);

            Parser parser = new Parser(lexer);

            int index = 0;
            while (parser.MoveNext())
            {
                Console.WriteLine(parser.CurrentInstruction);
                Assert.AreEqual(expected[index], parser.CurrentInstruction.ToString());
                index++;
            }
        }

        [TestMethod]
        public void ParserTests_TokenizeAssemblyFile_WithErrorTokens()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileErrorToken);

            Assert.IsNotNull(lines, "File Not Found.");

            string[] expected =
            {
                "ORIGIN[op: 'org', address: '0', valid: 'True']",
                "IF3[op: 'JMPADDR', Const/Address: 'start', valid: 'True']",
                "VariableAssign[op: db, name: valor1, values: [5], valid: 'True']",
                "VariableAssign[op: db, name: valor2, values: [7], valid: 'True']",
                "VariableAssign[op: db, name: mayor, values: [0], valid: 'True']",
                "InvalidInstruction[op: cst, values: [Token(type: IDENTIFIER, val: 'ten'),Token(type: IDENTIFIER, val: '0A')], valid: 'False']",
                "Label[name: start, valid: 'True']",
                "IF2[op: 'LOAD', Ra: 'R1', Const/Address: 'valor1', valid: 'True']",
                "IF2[op: 'LOAD', Ra: 'R2', Const/Address: 'valor2', valid: 'True']",
                "IF1[op: 'GRT', Ra: 'R1', Rb: 'R2', Rc: '', valid: 'True']",
                "InvalidInstruction[op: JMPADR, values: [Token(type: IDENTIFIER, val: 'R1esMayor')], valid: 'False']",
                "IF2[op: 'STORE', Ra: 'R2', Const/Address: 'mayor', valid: 'True']",
                "IF3[op: 'JMPADDR', Const/Address: 'fin', valid: 'True']",
                "Label[name: R1esMayor, valid: 'True']",
                "InvalidInstruction[op: STOE, values: [Token(type: IDENTIFIER, val: 'mayor'),Token(type: REGISTER, val: 'R1')], valid: 'False']",
                "IF2[op: 'LOADIM', Ra: 'R3', Const/Address: '#8', valid: 'True']",
                "Label[name: fin, valid: 'True']",
                "IF3[op: 'JMPADDR', Const/Address: 'fin', valid: 'True']"
            };

            lexer = new Lexer(lines);

            Parser parser = new Parser(lexer);

            int index = 0;
            while (parser.MoveNext())
            {
                Console.WriteLine(parser.CurrentInstruction);
                Assert.AreEqual(expected[index], parser.CurrentInstruction.ToString());
                index++;
            }
        }

        [TestMethod]
        public void ParserTests_ProfTest1_Success()
        {
            string[] lines = FileManager.Instance.ToReadFile(test1);

            Assert.IsNotNull(lines, "File Not Found.");

            string[] expected = FileManager.Instance.ToReadFile(test1Comparison);

            lexer = new Lexer(lines);
            Parser parser = new Parser(lexer);

            int index = 0;
            while (parser.MoveNext())
            {
                Console.WriteLine(parser.CurrentInstruction);
                Assert.AreEqual(expected[index], parser.CurrentInstruction.ToString());
                index++;
            }
        }

        [TestMethod]
        public void ParserTests_ProfTest2_Success()
        {
            string[] lines = FileManager.Instance.ToReadFile(test2);

            Assert.IsNotNull(lines, "File Not Found.");

            string[] expected = FileManager.Instance.ToReadFile(test2Comparison);

            lexer = new Lexer(lines);
            Parser parser = new Parser(lexer);

            int index = 0;
            while (parser.MoveNext())
            {
                Console.WriteLine(parser.CurrentInstruction);
                Assert.AreEqual(expected[index], parser.CurrentInstruction.ToString());
                index++;
            }
        }

        [TestMethod]
        public void ParserTests_ProfTest3_Success()
        {
            string[] lines = FileManager.Instance.ToReadFile(test3);

            Assert.IsNotNull(lines, "File Not Found.");

            string[] expected = FileManager.Instance.ToReadFile(test3Comparison);

            lexer = new Lexer(lines);
            Parser parser = new Parser(lexer);

            int index = 0;
            while (parser.MoveNext())
            {
                Console.WriteLine(parser.CurrentInstruction);
                Assert.AreEqual(expected[index], parser.CurrentInstruction.ToString());
                index++;
            }
        }

        [TestMethod]
        public void ParserTests_ParseFailNullToken_Success()
        {
            string[] lines = FileManager.Instance.ToReadFile(testParsePush);

            Assert.IsNotNull(lines, "File Not Found.");
            
            lexer = new Lexer(lines);
            Parser parser = new Parser(lexer);

            int index = 0;
            while (parser.MoveNext())
            {
                Console.WriteLine(parser.CurrentInstruction);
                index++;
            }
        }

        [TestMethod]
        public void ParserTests_CALL_Success()
        {
            string[] lines = FileManager.Instance.ToReadFile(test3Call);

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines);

            
            Parser parser = new Parser(lexer);

            int index = 0;
            while (parser.MoveNext())
            {
                Console.WriteLine(parser.CurrentInstruction);
                index++;
            }
        }
    }
}
