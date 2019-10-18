using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.UnitTests.MicroprocessorTests.InstructionSetExeTesters
{
    [TestClass]
    public class FlowControlOperationsTests
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
        public void FlowControlOperationsTests_JMPRIND_Success()
        {
            // JMPRIND Ra {F1} [pc] <- [R[ra]] 
            byte ra = 1;
            byte rb = 3;

            string addressInRa = UnitConverter.ByteToHex(100);
            string addressInRb = UnitConverter.ByteToHex(8);

            // set data in Register
            micro.MicroRegisters.SetRegisterValue(ra, addressInRa);
            micro.MicroRegisters.SetRegisterValue(rb, addressInRb);

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual("Registers[0,100,0,8,0,0,0,0]", micro.MicroRegisters.ToString());

            // start instruction
            MCInstructionF1 i1 = new MCInstructionF1(4, "10100", UnitConverter.ByteToBinary(ra));
            MCInstructionF1 i2 = new MCInstructionF1(4, "10100", UnitConverter.ByteToBinary(rb));

            Console.WriteLine(i1);
            Console.WriteLine(i2);

            // assert that program counter starts at 0
            Assert.AreEqual(0, micro.ProgramCounter);

            InstructionSetExe.ExecuteInstruction(i1, micro);
            Assert.AreEqual(100, micro.ProgramCounter);

            InstructionSetExe.ExecuteInstruction(i2, micro);
            Assert.AreEqual(8, micro.ProgramCounter);
        }

        [TestMethod]
        public void FlowControlOperationsTests_JMPADDR_Success()
        {
            // JMPADDR addr {F3} [pc] <- address
            string addressHex1 = UnitConverter.ByteToHex(6);
            string addressHex2 = UnitConverter.ByteToHex(44);

            // start instruction
            MCInstructionF3 i1 = new MCInstructionF3(4, "10101", UnitConverter.HexToBinary(addressHex1));
            MCInstructionF3 i2 = new MCInstructionF3(4, "10101", UnitConverter.HexToBinary(addressHex2));

            Console.WriteLine(i1);
            Console.WriteLine(i2);

            // assert that program counter starts at 0
            Assert.AreEqual(0, micro.ProgramCounter);

            InstructionSetExe.ExecuteInstruction(i1, micro);
            Assert.AreEqual(6, micro.ProgramCounter);

            InstructionSetExe.ExecuteInstruction(i2, micro);
            Assert.AreEqual(44, micro.ProgramCounter);
        }

        [TestMethod]
        public void FlowControlOperationsTests_JCONDRIN_Success()
        {
            // JCONDRIN Ra {F3} If cond then [pc] <- [R[ra]]
            byte ra = 1;
            byte rb = 3;

            byte v1 = 100;
            byte v2 = 8;

            string addressInRa = UnitConverter.ByteToHex(v1);
            string addressInRb = UnitConverter.ByteToHex(v2);

            // set data in Register
            micro.MicroRegisters.SetRegisterValue(ra, addressInRa);
            micro.MicroRegisters.SetRegisterValue(rb, addressInRb);

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual($"Registers[0,{v1},0,{v2},0,0,0,0]", micro.MicroRegisters.ToString());

            // start instruction
            MCInstructionF3 i1 = new MCInstructionF3(4, "10110", UnitConverter.ByteToBinary(ra));
            MCInstructionF3 i2 = new MCInstructionF3(4, "10110", UnitConverter.ByteToBinary(rb));

            Console.WriteLine(i1);
            Console.WriteLine(i2);

            // assert that program counter starts at 0
            Assert.AreEqual(0, micro.ProgramCounter);

            // set conditional to true
            micro.ConditionalBit = true;
            Console.WriteLine(micro);

            // when instructions executes PC will change
            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine(micro);
            Assert.AreEqual(v1, micro.ProgramCounter);

            // cond bit is false, pc will not change
            InstructionSetExe.ExecuteInstruction(i2, micro);
            Console.WriteLine($"CondBit False: {micro}");
            Assert.AreEqual(v1, micro.ProgramCounter);

            // now set to true
            micro.ConditionalBit = true;

            InstructionSetExe.ExecuteInstruction(i2, micro);

            Console.WriteLine(micro);
            Assert.AreEqual(v2, micro.ProgramCounter);
        }

        [TestMethod]
        public void FlowControlOperationsTests_JCONDADDR_Success()
        {
            // JCONDADDR addr {F3} If cond then [pc] <- address

            ushort a1 = 233;
            ushort a2 = 325;

            string addressBin1 = UnitConverter.IntToBinary(a1);
            string addressBin2 = UnitConverter.IntToBinary(a2);

            // start instruction
            MCInstructionF3 i1 = new MCInstructionF3(4, "10111", addressBin1);
            MCInstructionF3 i2 = new MCInstructionF3(4, "10111", addressBin2);

            Console.WriteLine(i1);
            Console.WriteLine(i2);

            // assert that program counter starts at 0
            Assert.AreEqual(0, micro.ProgramCounter);

            // set conditional to true
            micro.ConditionalBit = true;
            Console.WriteLine(micro);

            // when instructions executes PC will change
            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine(micro);
            Assert.AreEqual(a1, micro.ProgramCounter);

            // cond bit is false, pc will not change
            InstructionSetExe.ExecuteInstruction(i2, micro);
            Console.WriteLine($"CondBit False: {micro}");
            Assert.AreEqual(a1, micro.ProgramCounter);

            // now set to true
            micro.ConditionalBit = true;

            InstructionSetExe.ExecuteInstruction(i2, micro);

            Console.WriteLine(micro);
            Assert.AreEqual(a2, micro.ProgramCounter);
        }

        [TestMethod]
        public void FlowControlOperationsTests_LOOP_Success()
        {
            // LOOP Ra, address {F2} 
            // [R[ra]] <- [R[ra]] – 1 
            //If R[Ra] != 0 [pc] <- address 

            byte ra = 1;
            sbyte value = 100;

            string dataInRaBin = UnitConverter.ByteToBinary(value);

            string addressToGoHex = "AF";

            // set register
            micro.MicroRegisters.SetRegisterValue(ra, UnitConverter.BinaryToHex(dataInRaBin));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual("Registers[0,100,0,0,0,0,0,0]", micro.MicroRegisters.ToString());

            // start instruction
            MCInstructionF2 i1 = new MCInstructionF2(2, "11000", UnitConverter.ByteToBinary(ra),
                UnitConverter.HexToBinary(addressToGoHex));

            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine($"Expected PC: {UnitConverter.HexToInt(addressToGoHex)}");
            Console.WriteLine(micro);
            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(value - 1, UnitConverter.HexToInt(micro.MicroRegisters.GetRegisterValue(ra)));
            Assert.AreEqual(addressToGoHex, UnitConverter.IntToHex(micro.ProgramCounter));
        }

        [TestMethod]
        public void FlowControlOperationsTests_GRT_Success()
        {
            // GRT Ra, Rb {F1} Cond <- R[Ra] > R[Rb]
            string ra = "001"; // 1
            string rb = "011"; // 3

            string valInA = "00100100"; // 36
            string valInB = "11011011"; // -37

            bool result = true;

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(ra),
                UnitConverter.BinaryToHex(valInA));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb),
                UnitConverter.BinaryToHex(valInB));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual($"Registers[0,36,0,-37,0,0,0,0]", micro.MicroRegisters.ToString());

            // start instruction
            MCInstructionF1 instructionF1 = new MCInstructionF1(4, "11001", ra, rb);

            Console.WriteLine(instructionF1);

            InstructionSetExe.ExecuteInstruction(instructionF1, micro);

            Console.WriteLine(micro);

            Assert.AreEqual(result, micro.ConditionalBit);
        }

        [TestMethod]
        public void FlowControlOperationsTests_GRTEQ_IsNotGreater()
        {
            // GRTEQ Ra, Rb {F1} Cond <- R[Ra] >= R[Rb]
            string ra = "010"; // 2
            string rb = "101"; // 5

            string valInA = "00101101"; // 45
            string valInB = "00110010";

            bool result = false;

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                UnitConverter.BinaryToByte(ra),
                UnitConverter.BinaryToHex(valInA));

            micro.MicroRegisters.SetRegisterValue(
                UnitConverter.BinaryToByte(rb),
                UnitConverter.BinaryToHex(valInB));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual($"Registers[0,0,45,0,0,50,0,0]", micro.MicroRegisters.ToString());

            // start instruction
            MCInstructionF1 instructionF1 = new MCInstructionF1(4, "11010", ra, rb);

            Console.WriteLine(instructionF1);

            InstructionSetExe.ExecuteInstruction(instructionF1, micro);

            Console.WriteLine(micro);

            Assert.AreEqual(result, micro.ConditionalBit);
        }

        [TestMethod]
        public void FlowControlOperationsTests_EQ_AreNotEqual()
        {
            // EQ Ra, Rb {F1} Cond <- R[Ra] == R[Rb] 
            string ra = "010"; // 2
            string rb = "101"; // 5

            string valInA = "00101101"; // 45
            string valInB = "00110010"; // 50

            bool result = false;

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                UnitConverter.BinaryToByte(ra),
                UnitConverter.BinaryToHex(valInA));

            micro.MicroRegisters.SetRegisterValue(
                UnitConverter.BinaryToByte(rb),
                UnitConverter.BinaryToHex(valInB));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual($"Registers[0,0,45,0,0,50,0,0]", micro.MicroRegisters.ToString());

            // start instruction
            MCInstructionF1 instructionF1 = new MCInstructionF1(4, "11011", ra, rb);

            Console.WriteLine(instructionF1);

            InstructionSetExe.ExecuteInstruction(instructionF1, micro);

            Console.WriteLine(micro);

            Assert.AreEqual(result, micro.ConditionalBit);
        }

        [TestMethod]
        public void FlowControlOperationsTests_EQ_AreEqual()
        {
            // GRT Ra, Rb {F1} Cond <- R[Ra] > R[Rb]
            string ra = "010"; // 2
            string rb = "101"; // 5

            string valInA = "00101101"; // 45
            string valInB = "00101101"; // 45

            bool result = true;

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                UnitConverter.BinaryToByte(ra),
                UnitConverter.BinaryToHex(valInA));

            micro.MicroRegisters.SetRegisterValue(
                UnitConverter.BinaryToByte(rb),
                UnitConverter.BinaryToHex(valInB));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual($"Registers[0,0,45,0,0,45,0,0]", micro.MicroRegisters.ToString());

            // start instruction
            MCInstructionF1 instructionF1 = new MCInstructionF1(4, "11011", ra, rb);

            Console.WriteLine(instructionF1);

            InstructionSetExe.ExecuteInstruction(instructionF1, micro);

            Console.WriteLine(micro);

            Assert.AreEqual(result, micro.ConditionalBit);
        }

        [TestMethod]
        public void FlowControlOperationsTests_NEQ_AreNotEqual()
        {
            // NEQ Ra, Rb {F1} Cond <- R[Ra] != R[Rb] 
            string ra = "010"; // 2
            string rb = "101"; // 5

            string valInA = "00101101"; // 45
            string valInB = "00101101"; // 45

            bool result = false;

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                UnitConverter.BinaryToByte(ra),
                UnitConverter.BinaryToHex(valInA));

            micro.MicroRegisters.SetRegisterValue(
                UnitConverter.BinaryToByte(rb),
                UnitConverter.BinaryToHex(valInB));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual($"Registers[0,0,45,0,0,45,0,0]", micro.MicroRegisters.ToString());

            // start instruction
            MCInstructionF1 instructionF1 = new MCInstructionF1(4, "11100", ra, rb);

            Console.WriteLine(instructionF1);

            InstructionSetExe.ExecuteInstruction(instructionF1, micro);

            Console.WriteLine(micro);

            Assert.AreEqual(result, micro.ConditionalBit);
        }
    }
}
