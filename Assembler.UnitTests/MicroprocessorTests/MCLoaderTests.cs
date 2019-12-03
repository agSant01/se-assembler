using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

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
            string expected = "MCInstructionF3[InstructionAddressDecimal: (decimal)'0', opcode:'21', AddressParamHex:'06']";

            VirtualMemory vm = new VirtualMemory(new string[] { "A8 06" });

            MCLoader l = new MCLoader(vm, new MicroSimulator(vm));

            IMCInstruction i = l.NextInstruction();

            Console.WriteLine(i);

            Assert.AreEqual(expected, i.ToString());
        }

        [TestMethod]
        public void MCLoaderTests_LoadObjectFile_Success()
        {
            string[] lines = FileManager.Instance.ToReadFile(machineCodeFile);

            Assert.IsNotNull(lines);

            string[] expected =
            {
                "MCInstructionF3[InstructionAddressDecimal: (decimal)'0', opcode:'21', AddressParamHex:'06']",
                "MCInstructionF2[InstructionAddressDecimal: (decimal)'6', opcode:'0', Ra:'1', AddressParamHex:'02']",
                "MCInstructionF2[InstructionAddressDecimal: (decimal)'8', opcode:'0', Ra:'2', AddressParamHex:'03']",
                "MCInstructionF1[InstructionAddressDecimal: (decimal)'10', opcode:'25', Ra:'1', Rb:'2', Rc:'0']",
                "MCInstructionF3[InstructionAddressDecimal: (decimal)'12', opcode:'21', AddressParamHex:'12']",
                "MCInstructionF2[InstructionAddressDecimal: (decimal)'18', opcode:'3', Ra:'1', AddressParamHex:'04']",
                "MCInstructionF2[InstructionAddressDecimal: (decimal)'20', opcode:'1', Ra:'3', AddressParamHex:'08']",
                "MCInstructionF3[InstructionAddressDecimal: (decimal)'22', opcode:'21', AddressParamHex:'16']",
                "MCInstructionF3[InstructionAddressDecimal: (decimal)'22', opcode:'21', AddressParamHex:'16']",
                "MCInstructionF3[InstructionAddressDecimal: (decimal)'22', opcode:'21', AddressParamHex:'16']"
            };

            VirtualMemory vm = new VirtualMemory(lines);

            Console.WriteLine(vm);

            MicroSimulator micro = new MicroSimulator(vm);

            MCLoader l = new MCLoader(vm, micro);

            int i = 0;
            while (i < 10)
            {
                IMCInstruction instruction = l.NextInstruction();
                
                Console.WriteLine(instruction);

                Assert.AreEqual(expected[i], instruction.ToString());

                i++;
            }
        }
    }
}