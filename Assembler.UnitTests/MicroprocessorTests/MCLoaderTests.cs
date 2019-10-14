using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assembler.UnitTests.MicroprocessorTests
{
    [TestClass]
    public class MCLoaderTests
    {
        private readonly string machineCodeFile = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"MicroprocessorTests\TestFiles\assembly_test_OBJ_FILE.txt");

        [TestMethod]
        public void MCLoaderTests_ExecuteOneInstruction_Success()
        {
            string expected = "MCInstructionF1[MemoryAddress: (decimal)'0', opcode:'21', Address:'06']";

            VirtualMemory vm = new VirtualMemory(new string[] { "A8 06" });

            MCLoader l = new MCLoader(vm, new MicroSimulatorTester(vm));

            IMCInstruction i = l.NextInstruction();

            Console.WriteLine(i);

            Assert.AreEqual(expected, i.ToString());
        }

        [TestMethod]
        public void MCLoaderTests_LoadObjectFile_Success()
        {
            string[] lines = FileManager.Instance.ToReadFile(machineCodeFile);

            Assert.IsNotNull(lines);

            VirtualMemory vm = new VirtualMemory(lines);

            Console.WriteLine(vm);

            MicroSimulatorTester micro = new MicroSimulatorTester(vm);

            MCLoader l = new MCLoader(vm, micro);

            int i = 0;
            while (i < 20)
            {
                IMCInstruction instruction = l.NextInstruction();

                //micro.ProgramCounter += 2;
                
                Console.WriteLine(instruction);
                i++;
            }

            Console.WriteLine(i);
        }
    }

    class MicroSimulatorTester : MicroSimulator
    {
        public MicroSimulatorTester(VirtualMemory virtualMemory)
            : base(virtualMemory) {
        }
    }
}