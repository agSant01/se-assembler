using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;

namespace Assembler.UnitTests.MicroprocessorTests
{
    [TestClass]
    public class InstructionSetExeTester
    {
        private MicroSimulator micro;

        private readonly string machineCodeFile = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"MicroprocessorTests\TestFiles\assembly_test_OBJ_FILE.txt");

        [TestInitialize]
        public void TestSetup()
        {
            string[] lines = FileManager.Instance.ToReadFile(machineCodeFile);

            VirtualMemory vm = new VirtualMemory(lines);

            micro = new MicroSimulator(vm);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            micro = null;
        }

        [TestMethod]
        public void InstructionSetExeTester_TestLoadInstructionToRegisters_Success()
        {
            // LOAD R2 02
            MCInstructionF2 i1 = new MCInstructionF2(3, "00000", "010", "00000010");

            // LOAD R2 03
            MCInstructionF2 i2 = new MCInstructionF2(3, "00000", "011", "00000011");

            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine($"Registers after Execution #1 -> {micro.MicroRegisters}");

            Assert.AreEqual("05", micro.MicroRegisters.GetRegisterValue(2));

            InstructionSetExe.ExecuteInstruction(i2, micro);


            Assert.AreEqual("07", micro.MicroRegisters.GetRegisterValue(3));

            Console.WriteLine($"Registers after Execution #2 -> {micro.MicroRegisters}");
        }
    }
}
