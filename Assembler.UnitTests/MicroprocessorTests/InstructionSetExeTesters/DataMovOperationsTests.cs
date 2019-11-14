using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Assembler.UnitTests.MicroprocessorTests.InstructionSetExeTesters
{
    [TestClass]
    public class DataMovOperationsTests
    {
        private MicroSimulator micro;

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
        public void DataMovOperationsTests_LoadVMValueToRegister_Success()
        {
            micro.WriteToMemory(2, "05");
            micro.WriteToMemory(3, "07");

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


        [TestMethod]
        public void DataMovOperationsTests_LoadConstantValueToRegister_Success()
        {
            string value1 = "00000100";
            string value2 = "00010000";

            // LOADIM R2 02
            MCInstructionF2 i1 = new MCInstructionF2(3, "00001", "010", value1);

            // LOAD R2 03
            MCInstructionF2 i2 = new MCInstructionF2(3, "00001", "011", value2);

            InstructionSetExe.ExecuteInstruction(i1, micro);
            Console.WriteLine($"Registers after Execution #1 -> {micro.MicroRegisters}");

            InstructionSetExe.ExecuteInstruction(i2, micro);
            Console.WriteLine($"Registers after Execution #2 -> {micro.MicroRegisters}");

            Assert.AreEqual(UnitConverter.BinaryToHex(value1), micro.MicroRegisters.GetRegisterValue(2));
            Assert.AreEqual(UnitConverter.BinaryToHex(value2), micro.MicroRegisters.GetRegisterValue(3));
        }

        [TestMethod]
        public void DataMovOperationsTests_PUSH_Success()
        {
            //init
            micro.StackPointer = 100;
            micro.MicroRegisters.SetRegisterValue(2, "FF");
            MCInstructionF2 i1 = new MCInstructionF2(3, "00100", "010", null);
            ushort SPNewValue = 99;

            //execute
            InstructionSetExe.ExecuteInstruction(i1, micro);

            Assert.AreEqual(SPNewValue, micro.StackPointer);
            Assert.AreEqual("FF", micro.ReadFromMemory(micro.StackPointer));
        }

        [TestMethod]
        public void DataMovOperationsTests_POP_Success()
        {
            //init
            micro.StackPointer = 100;
            micro.WriteToMemory(100, "FF");
            MCInstructionF2 i1 = new MCInstructionF2(3, "00010", "010", null);
            ushort SPNewValue = 101;

            //execute
            InstructionSetExe.ExecuteInstruction(i1, micro);

            Assert.AreEqual(SPNewValue, micro.StackPointer);
            Assert.AreEqual("FF", micro.MicroRegisters.GetRegisterValue(2));
        }

        [TestMethod]
        public void DataMovOperationsTests_StoreValueInRegisterToMemory_Success()
        {
            // Set register state to Registers[0,0,4,16,0,0,0,0]
            micro.MicroRegisters.SetRegisterValue(2, "04");
            micro.MicroRegisters.SetRegisterValue(3, "10");

            Console.WriteLine(micro.MicroRegisters);
            Assert.AreEqual("Registers[0,0,4,16,0,0,0,0]", micro.MicroRegisters.ToString());

            string memoryAddress1 = UnitConverter.IntToBinary(50);
            string memoryAddress2 = UnitConverter.IntToBinary(51);

            Console.WriteLine($"Memory1: {UnitConverter.BinaryToInt(memoryAddress1)}");
            Console.WriteLine($"Memory1: {UnitConverter.BinaryToInt(memoryAddress2)}");

            // {F2} STORE mem, Ra    [mem] <- R[Ra]
            MCInstructionF2 i1 = new MCInstructionF2(3, "00011", "010", memoryAddress1);
            MCInstructionF2 i2 = new MCInstructionF2(3, "00011", "011", memoryAddress2);

            // execute store
            InstructionSetExe.ExecuteInstruction(i1, micro);
            InstructionSetExe.ExecuteInstruction(i2, micro);

            Console.WriteLine($"VirtualMemory #{UnitConverter.BinaryToInt(memoryAddress1)}: " +
                $"{UnitConverter.HexToInt(micro.ReadFromMemory(UnitConverter.BinaryToInt(memoryAddress1)))}");
            Console.WriteLine($"VirtualMemory #{UnitConverter.BinaryToInt(memoryAddress2)}: " +
                $"{UnitConverter.HexToInt(micro.ReadFromMemory(UnitConverter.BinaryToInt(memoryAddress2)))}");

            Assert.AreEqual(4, UnitConverter.HexToInt(micro.ReadFromMemory(
                UnitConverter.BinaryToInt(memoryAddress1)
                )));
            Assert.AreEqual(16, UnitConverter.HexToInt(micro.ReadFromMemory(
                UnitConverter.BinaryToInt(memoryAddress2)
                )));
        }

        [TestMethod]
        public void DataMovOperationsTests_LOADRIND_Success()
        {
            // LOADRIND Ra,Rb  {F1} R[Ra] <- mem[R[Rb]] 
            byte Ra = 1;
            byte Rb = 2;

            string dataHexRa = "9";
            string dataHexRb = "40";

            string valueInMemory = UnitConverter.IntToHex(52);

            // setting Registers and Memory values
            micro.WriteToMemory(
                UnitConverter.HexToInt(dataHexRb), valueInMemory);

            Assert.AreEqual(valueInMemory, micro.ReadFromMemory(UnitConverter.HexToInt(dataHexRb)));

            micro.MicroRegisters.SetRegisterValue(Ra, dataHexRa);
            micro.MicroRegisters.SetRegisterValue(Rb, dataHexRb);

            Console.WriteLine(micro.MicroRegisters.ToString());
            Assert.AreEqual("Registers[0,9,64,0,0,0,0,0]", micro.MicroRegisters.ToString());

            // starting tests
            MCInstructionF1 i1 = new MCInstructionF1(3, "00101",
                UnitConverter.IntToBinary(Ra),
                UnitConverter.IntToBinary(Rb)
                );

            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine($"Register after execution: {micro.MicroRegisters}");

            Assert.AreEqual(valueInMemory, micro.MicroRegisters.GetRegisterValue(Ra));
        }

        [TestMethod]
        public void DataMovOperationsTests_STORERIND_Success()
        {
            // STORERIND Ra,Rb  {F1} R[Rb] <- mem[R[Ra]] 

            string valueInMem = UnitConverter.IntToHex(40);
            int dataInRegisterAHex = 110;

            byte registerA = 3;
            byte registerB = 7;


            // set data in register and VM
            micro.WriteToMemory(dataInRegisterAHex, valueInMem);
            micro.MicroRegisters.SetRegisterValue(registerA, UnitConverter.IntToHex(dataInRegisterAHex));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(valueInMem, micro.ReadFromMemory(dataInRegisterAHex));
            Assert.AreEqual(UnitConverter.IntToHex(dataInRegisterAHex), micro.MicroRegisters.GetRegisterValue(registerA));

            // set instruction
            MCInstructionF1 instructionF1 = new MCInstructionF1(4, "00110",
                UnitConverter.IntToBinary(registerA),
                UnitConverter.IntToBinary(registerB)
                );

            InstructionSetExe.ExecuteInstruction(instructionF1, micro);

            Console.WriteLine($"After execution: {micro.MicroRegisters}");
            Console.WriteLine($"Value in address {dataInRegisterAHex}: {UnitConverter.HexToInt(valueInMem)}");
            Assert.AreEqual(
                valueInMem,
                micro.MicroRegisters.GetRegisterValue(registerB)
                );
        }
    }
}
