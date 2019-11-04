using Assembler.Assembler;
using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Parsing;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assembler.UnitTests.MicroprocessorTests
{
    [TestClass]
    public class MicroSimulatorTests
    {
        private MicroSimulator micro;

        private readonly string test1 = Path.Combine(
           Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
           @"MicroprocessorTests\TestFiles\test1.txt");
        private readonly string test1Comparison = Path.Combine(
           Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
           @"MicroprocessorTests\MicroTestsComparisons\test1Comparison.txt");

        private readonly string test2 = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"MicroprocessorTests\TestFiles\test2.txt");
        private readonly string test2Comparison = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"MicroprocessorTests\MicroTestsComparisons\test2Comparison.txt");

        private readonly string test3 = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"MicroprocessorTests\TestFiles\test3.txt");
        
        [TestInitialize]
        public void TestSetup()
        {
            VirtualMemory vm = new VirtualMemory(new string[] { });

            micro = new MicroSimulator(vm);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            micro = null;
        }

        [TestMethod]
        public void MicroSimulatorTests_ExecuteOneInstruction_Success()
        {
            Assert.AreEqual(0, micro.ProgramCounter);

            micro.ProgramCounter = 1000;

            Assert.AreEqual(1000, micro.ProgramCounter);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException), "The number is to big for the 11-bit PC")]
        public void MicroSimulatorTests_ToBigProgramCounterAddress_Success()
        {
            Assert.AreEqual(0, micro.ProgramCounter);

            micro.ProgramCounter = 2900;
        }

        [TestMethod]
        public void MicroSimulatorTests_ProfTest1_Success()
        {
            string[] asmLines = FileManager.Instance.ToReadFile(test1);
            string[] expected = FileManager.Instance.ToReadFile(test1Comparison);

            Lexer lexer = new Lexer(asmLines);

            Parser parser = new Parser(lexer);

            Compiler assembler = new Compiler(parser);

            assembler.Compile();

            string[] objData = assembler.GetOutput();

            VirtualMemory vm = new VirtualMemory(objData);

            MicroSimulator micro = new MicroSimulator(vm);

            int i = 0;
            int counter = 0;
            while (i < 6)
            {
                micro.NextInstruction();
                Console.WriteLine(micro.CurrentInstruction);
                Assert.AreEqual(expected[counter++], micro.CurrentInstruction.ToString());

                if (micro.CurrentInstruction.Equals(micro.PreviousInstruction))
                {
                    i++;
                }
            }
            Console.WriteLine(micro.MicroRegisters);
            Assert.AreEqual("Registers[0,8,5,49,10,0,0,0]", micro.MicroRegisters.ToString());

            Console.WriteLine(vm.ToString());
        }

        [TestMethod]
        public void MicroSimulatorTests_ProfTest2_Success()
        {
            string[] asmLines = FileManager.Instance.ToReadFile(test2);
            string[] expected = FileManager.Instance.ToReadFile(test2Comparison);

            Lexer lexer = new Lexer(asmLines);

            Parser parser = new Parser(lexer);

            Compiler assembler = new Compiler(parser);

            assembler.Compile();

            string[] objData = assembler.GetOutput();

            VirtualMemory vm = new VirtualMemory(objData);

            MicroSimulator micro = new MicroSimulator(vm);

            int i = 0, counter = 0;
            while (i < 6)
            {
                micro.NextInstruction();
                Console.WriteLine(micro.CurrentInstruction);
                Assert.AreEqual(expected[counter++], micro.CurrentInstruction.ToString());
                if (micro.CurrentInstruction.Equals(micro.PreviousInstruction))
                {
                    i++;
                }
            }
            Console.WriteLine(micro.MicroRegisters);
            Assert.AreEqual("Registers[0,5,15,5,0,0,0,0]", micro.MicroRegisters.ToString());

            Console.WriteLine(vm.ToString());
        }

        [TestMethod]
        public void MicroSimulatorTests_ProfTest3_Success()
        {
            string[] asmLines = FileManager.Instance.ToReadFile(test3);

            Lexer lexer = new Lexer(asmLines);

            Parser parser = new Parser(lexer);

            Compiler assembler = new Compiler(parser);

            assembler.Compile();

            string[] objData = assembler.GetOutput();

            VirtualMemory vm = new VirtualMemory(objData);

            MicroSimulator micro = new MicroSimulator(vm);

            int i = 0;
            while (i < 1000)
            {
                micro.NextInstruction();
                i++;
            }
            Console.WriteLine(micro.MicroRegisters);
            Console.WriteLine(vm.ToString());

            Assert.AreEqual("Registers[0,0,0,0,0,12,15,125]", micro.MicroRegisters.ToString());
        }
    }
}