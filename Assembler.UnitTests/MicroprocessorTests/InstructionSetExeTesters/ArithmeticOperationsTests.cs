using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Assembler.UnitTests.MicroprocessorTests.InstructionSetExeTesters
{
    [TestClass]
    public class ArithmeticOperationsTests
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
        public void InstructionSetExeTester_ADD_Success()
        {
            // ADD Ra, Rb, Rc {F1} R[Ra]<- R[Rb]+R[Rc] 
            string ra = "110"; // 6
            string rb = "011"; // 3
            string rc = "101"; // 5

            sbyte valInB = 20;
            sbyte valInC = -83;

            byte resultA = (byte) (valInC + valInB);

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                (byte) UnitConverter.BinaryToInt(rb),
                UnitConverter.ByteToHex(valInB));

            micro.MicroRegisters.SetRegisterValue(
                (byte) UnitConverter.BinaryToInt(rc),
                UnitConverter.ByteToHex(valInC));
            
            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.IntToHex(valInB),
                micro.MicroRegisters.GetRegisterValue((byte) UnitConverter.BinaryToInt(rb))
                );

            Assert.AreEqual(
                UnitConverter.ByteToHex(valInC), 
                micro.MicroRegisters.GetRegisterValue((byte) UnitConverter.BinaryToInt(rc))
                );

            MCInstructionF1 i1 = new MCInstructionF1(3, "00111", ra, rb, rc);

            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.ByteToHex(resultA),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))
                );
        }

        [TestMethod]
        public void InstructionSetExeTester_SUB_Success()
        {
            // ADD Ra, Rb, Rc {F1} R[Ra]<- R[Rb]+R[Rc] 
            string ra = "110"; // 6
            string rb = "011"; // 3
            string rc = "101"; // 5

            sbyte valInB = 20;
            sbyte valInC = 83;

            sbyte resultA = (sbyte)(valInB - valInC);

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb),
                UnitConverter.IntToHex(valInB));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc),
                UnitConverter.IntToHex(valInC));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.IntToHex(valInB),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rb))
                );

            Assert.AreEqual(
                UnitConverter.IntToHex(valInC),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rc))
                );

            MCInstructionF1 i1 = new MCInstructionF1(3, "01000", ra, rb, rc);

            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.ByteToHex(resultA),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))
                );
        }

        [TestMethod]
        public void InstructionSetExeTester_ADDIM_Success()
        {
            // ADDIM Ra, cons {F2} R[Ra] <- R[Ra]+cons
            string ra = "110"; // 6

            sbyte valInA = 20;
            sbyte constVal = -33;

            sbyte resultA = (sbyte)(valInA + constVal);

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                (byte) UnitConverter.BinaryToInt(ra),
                UnitConverter.ByteToHex(valInA));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.ByteToHex(valInA),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))
                );

            MCInstructionF2 i2 = new MCInstructionF2(3, "01001", ra, 
                UnitConverter.ByteToBinary(constVal)
                );

            Console.WriteLine(i2);

            InstructionSetExe.ExecuteInstruction(i2, micro);

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.ByteToHex(resultA),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))
                );
        }

        [TestMethod]
        public void InstructionSetExeTester_SUBIM_Success()
        {
            // SUBIM Ra,  cons {F2} R[Ra] <- R[Ra]-cons 
            string ra = "001"; // 1

            sbyte valInA = 20;
            sbyte constVal = -33;

            sbyte resultA = (sbyte)(valInA - constVal);

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(ra),
                UnitConverter.ByteToHex(valInA));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.IntToHex(valInA),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))
                );

            MCInstructionF2 i2 = new MCInstructionF2(3, "01010", ra,
                UnitConverter.ByteToBinary(constVal)
                );
            Console.WriteLine(i2);

            // execute instruction
            
            InstructionSetExe.ExecuteInstruction(i2, micro);

            Console.WriteLine(micro.MicroRegisters);
            Console.WriteLine($"Result in 0x{UnitConverter.ByteToHex((byte)resultA)}");
            Console.WriteLine($"Result in binary: {UnitConverter.ByteToHex((byte)resultA)}");

            Assert.AreEqual(
                UnitConverter.ByteToHex((byte) resultA),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))
                );
        }
    }
}
