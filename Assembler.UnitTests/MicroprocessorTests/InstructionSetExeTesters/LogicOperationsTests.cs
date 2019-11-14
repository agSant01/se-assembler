using Assembler.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Assembler.UnitTests.MicroprocessorTests.InstructionSetExeTesters
{
    [TestClass]
    public class LogicOperationsTests
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
        public void LogicOperationsTests_AND_Success()
        {
            // AND Ra, Rb, Rc {F1} R[Ra]<- R[Rb]*R[Rc]
            string ra = "001"; // 1
            string rb = "011"; // 3
            string rc = "111"; // 7

            string valInB = "10100100";

            string valInC = "01101111";

            string resultA = "00100100";

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb),
                UnitConverter.BinaryToHex(valInB));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc),
                UnitConverter.BinaryToHex(valInC));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.BinaryToHex(valInB),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rb))
                );

            Assert.AreEqual(
               UnitConverter.BinaryToHex(valInC),
               micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rc))
               );

            MCInstructionF1 i1 = new MCInstructionF1(3, "01011", ra, rb, rc);

            Console.WriteLine(i1);

            // execute instruction
            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))
                );
        }

        [TestMethod]
        public void LogicOperationsTests_OR_Success()
        {
            // AND Ra, Rb, Rc {F1} R[Ra]<- R[Rb]*R[Rc]
            string ra = "001"; // 1
            string rb = "011"; // 3
            string rc = "111"; // 7

            string valInB = "00100100";

            string valInC = "11101111";

            string resultA = "11101111";

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb),
                UnitConverter.BinaryToHex(valInB));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc),
                UnitConverter.BinaryToHex(valInC));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.BinaryToHex(valInB),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rb))
                );

            Assert.AreEqual(
               UnitConverter.BinaryToHex(valInC),
               micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rc))
               );

            MCInstructionF1 i1 = new MCInstructionF1(3, "01100", ra, rb, rc);

            Console.WriteLine(i1);

            // execute instruction
            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))
                );
        }

        [TestMethod]
        public void LogicOperationsTests_XOR_Success()
        {
            // AND Ra, Rb, Rc {F1} R[Ra]<- R[Rb]*R[Rc]
            string ra = "001"; // 1
            string rb = "011"; // 3
            string rc = "111"; // 7

            string valInB = "00100100";

            string valInC = "11101111";

            string resultA = "11001011";

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb),
                UnitConverter.BinaryToHex(valInB));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc),
                UnitConverter.BinaryToHex(valInC));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.BinaryToHex(valInB),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rb))
                );

            Assert.AreEqual(
               UnitConverter.BinaryToHex(valInC),
               micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rc))
               );

            MCInstructionF1 i1 = new MCInstructionF1(3, "01101", ra, rb, rc);

            Console.WriteLine(i1);

            // execute instruction
            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))
                );
        }

        [TestMethod]
        public void LogicOperationsTests_NOT_Success()
        {
            // NOT Ra,Rb {F1}   R[Ra] <- Complement(R[Rb])
            string ra = "001"; // 1
            string rb = "011"; // 3

            string valInB = "00100100";

            string resultA = "11011011";

            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb),
                UnitConverter.BinaryToHex(valInB));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.BinaryToHex(valInB),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rb))
                );

            MCInstructionF1 i1 = new MCInstructionF1(3, "01110", ra, rb);

            Console.WriteLine(i1);

            // execute instruction
            InstructionSetExe.ExecuteInstruction(i1, micro);

            Console.WriteLine(micro.MicroRegisters);

            Console.WriteLine($"Expected: {UnitConverter.BinaryToHex(resultA)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))}");

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra))
                );
        }

        [TestMethod]
        public void LogicOperationsTests_NEG_Success()
        {
            // NEG Ra,Rb {F1} R[Ra]<- - R[Rb]
            string ra1 = "001"; // 1
            string rb1 = "011"; // 3

            string ra2 = "110"; // 6
            string rb2 = "111"; // 7


            string valInB1 = UnitConverter.ByteToBinary(-13);
            string resultA1 = UnitConverter.ByteToBinary(13);

            string valInB2 = UnitConverter.ByteToBinary(-45);
            string resultA2 = UnitConverter.ByteToBinary(45);


            // set data in register
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb1),
                UnitConverter.BinaryToHex(valInB1));

            micro.MicroRegisters.SetRegisterValue(
               (byte)UnitConverter.BinaryToInt(rb2),
               UnitConverter.BinaryToHex(valInB2));

            Console.WriteLine(micro.MicroRegisters);

            Assert.AreEqual(
                UnitConverter.BinaryToHex(valInB1),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rb1))
                );

            Assert.AreEqual(
               UnitConverter.BinaryToHex(valInB2),
               micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(rb2))
               );

            MCInstructionF1 i1 = new MCInstructionF1(3, "01111", ra1, rb1);
            MCInstructionF1 i2 = new MCInstructionF1(3, "01111", ra2, rb2);

            Console.WriteLine(i1);
            Console.WriteLine(i2);

            // execute instruction
            InstructionSetExe.ExecuteInstruction(i1, micro);
            InstructionSetExe.ExecuteInstruction(i2, micro);

            Console.WriteLine(micro.MicroRegisters);

            Console.WriteLine($"Expected1: {UnitConverter.BinaryToHex(resultA1)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra1))}");
            Console.WriteLine($"Expected2: {UnitConverter.BinaryToHex(resultA2)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra2))}");

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA1),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra1))
                );

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA2),
                micro.MicroRegisters.GetRegisterValue((byte)UnitConverter.BinaryToInt(ra2))
                );
        }

        [TestMethod]
        public void LogicOperationsTests_SHIFTR_Success()
        {
            // SHIFTR Ra, Rb, Rc {F1} R[Ra]<- R[Rb] shr R[Rc]
            string ra1 = "001"; // 1
            string rb1 = "010"; // 2
            string rc1 = "011"; // 3

            string ra2 = "101"; // 5
            string rb2 = "110"; // 6
            string rc2 = "111"; // 7


            string valInB1 = "00111100";
            string valInC1 = UnitConverter.ByteToBinary(2);
            string resultA1 = "00001111";

            string valInB2 = "10011100";
            string valInC2 = UnitConverter.ByteToBinary(2);
            string resultA2 = "00100111";


            // set data in register
            ////////////
            /// Test 1
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb1),
                UnitConverter.BinaryToHex(valInB1));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc1),
                UnitConverter.BinaryToHex(valInC1));
            ////////////

            ////////////
            /// Test 2
            micro.MicroRegisters.SetRegisterValue(
               (byte)UnitConverter.BinaryToInt(rb2),
               UnitConverter.BinaryToHex(valInB2));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc2),
                UnitConverter.BinaryToHex(valInC2));
            ///////////

            Console.WriteLine(micro.MicroRegisters);

            // Assert Registers
            Assert.AreEqual("Registers[0,0,60,2,0,0,-100,2]",
                micro.MicroRegisters.ToString());

            MCInstructionF1 i1 = new MCInstructionF1(3, "10000", ra1, rb1, rc1);
            MCInstructionF1 i2 = new MCInstructionF1(3, "10000", ra2, rb2, rc2);

            Console.WriteLine(i1);
            Console.WriteLine(i2);

            // execute instruction
            InstructionSetExe.ExecuteInstruction(i1, micro);
            InstructionSetExe.ExecuteInstruction(i2, micro);

            Console.WriteLine(micro.MicroRegisters);

            Console.WriteLine($"Expected1: {UnitConverter.BinaryToHex(resultA1)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra1))}");
            Console.WriteLine($"Expected2: {UnitConverter.BinaryToHex(resultA2)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra2))}");

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA1),
                micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra1))
                );

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA2),
                micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra2))
                );
        }

        [TestMethod]
        public void LogicOperationsTests_SHIFTL_Success()
        {
            // SHIFTR Ra, Rb, Rc {F1} R[Ra]<- R[Rb] shr R[Rc]
            string ra1 = "001"; // 1
            string rb1 = "010"; // 2
            string rc1 = "011"; // 3

            string ra2 = "101"; // 5
            string rb2 = "110"; // 6
            string rc2 = "111"; // 7


            string valInB1 = "00111100";
            string valInC1 = UnitConverter.ByteToBinary(2);
            string resultA1 = "11110000";

            string valInB2 = "10011100";
            string valInC2 = UnitConverter.ByteToBinary(2);
            string resultA2 = "01110000";


            // set data in register
            ////////////
            /// Test 1
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb1),
                UnitConverter.BinaryToHex(valInB1));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc1),
                UnitConverter.BinaryToHex(valInC1));
            ////////////

            ////////////
            /// Test 2
            micro.MicroRegisters.SetRegisterValue(
               (byte)UnitConverter.BinaryToInt(rb2),
               UnitConverter.BinaryToHex(valInB2));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc2),
                UnitConverter.BinaryToHex(valInC2));
            ///////////

            Console.WriteLine(micro.MicroRegisters);

            // Assert Registers
            Assert.AreEqual("Registers[0,0,60,2,0,0,-100,2]",
                micro.MicroRegisters.ToString());

            MCInstructionF1 i1 = new MCInstructionF1(3, "10001", ra1, rb1, rc1);
            MCInstructionF1 i2 = new MCInstructionF1(3, "10001", ra2, rb2, rc2);

            Console.WriteLine(i1);
            Console.WriteLine(i2);

            // execute instruction
            InstructionSetExe.ExecuteInstruction(i1, micro);
            InstructionSetExe.ExecuteInstruction(i2, micro);

            Console.WriteLine(micro.MicroRegisters);

            Console.WriteLine($"Expected1: {UnitConverter.BinaryToHex(resultA1)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra1))}");
            Console.WriteLine($"Expected2: {UnitConverter.BinaryToHex(resultA2)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra2))}");

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA1),
                micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra1))
                );

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA2),
                micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra2))
                );
        }

        [TestMethod]
        public void LogicOperationsTests_ROTAR_Success()
        {
            // ROTAR Ra, Rb, Rc {F1} R[Ra]<- R[Rb] rtr R[Rc]
            string ra1 = "001"; // 1
            string rb1 = "010"; // 2
            string rc1 = "011"; // 3

            string ra2 = "101"; // 5
            string rb2 = "110"; // 6
            string rc2 = "111"; // 7


            string valInB1 = "00111100";
            string valInC1 = UnitConverter.ByteToBinary(2);
            string resultA1 = "00001111";

            string valInB2 = "10011100";
            string valInC2 = UnitConverter.ByteToBinary(2);
            string resultA2 = "00100111";


            // set data in register
            ////////////
            /// Test 1
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb1),
                UnitConverter.BinaryToHex(valInB1));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc1),
                UnitConverter.BinaryToHex(valInC1));
            ////////////

            ////////////
            /// Test 2
            micro.MicroRegisters.SetRegisterValue(
               (byte)UnitConverter.BinaryToInt(rb2),
               UnitConverter.BinaryToHex(valInB2));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc2),
                UnitConverter.BinaryToHex(valInC2));
            ///////////

            Console.WriteLine(micro.MicroRegisters);

            // Assert Registers
            Assert.AreEqual("Registers[0,0,60,2,0,0,-100,2]",
                micro.MicroRegisters.ToString());

            MCInstructionF1 i1 = new MCInstructionF1(3, "10010", ra1, rb1, rc1);
            MCInstructionF1 i2 = new MCInstructionF1(3, "10010", ra2, rb2, rc2);

            Console.WriteLine(i1);
            Console.WriteLine(i2);

            // execute instruction
            InstructionSetExe.ExecuteInstruction(i1, micro);
            InstructionSetExe.ExecuteInstruction(i2, micro);

            Console.WriteLine(micro.MicroRegisters);

            Console.WriteLine($"Expected1: {UnitConverter.BinaryToHex(resultA1)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra1))}");
            Console.WriteLine($"Expected2: {UnitConverter.BinaryToHex(resultA2)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra2))}");

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA1),
                micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra1))
                );

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA2),
                micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra2))
                );
        }

        [TestMethod]
        public void LogicOperationsTests_ROTAL_Success()
        {
            // ROTAL Ra, Rb, Rc {F1} R[Ra]<- R[Rb] rtl R[Rc]
            string ra1 = "001"; // 1
            string rb1 = "010"; // 2
            string rc1 = "011"; // 3

            string ra2 = "101"; // 5
            string rb2 = "110"; // 6
            string rc2 = "111"; // 7


            MCInstructionF1 i1 = new MCInstructionF1(3, "10011", ra1, rb1, rc1);
            MCInstructionF1 i2 = new MCInstructionF1(3, "10011", ra2, rb2, rc2);

            string valInB1 = "00111100";
            string valInC1 = UnitConverter.ByteToBinary(2);
            string resultA1 = "11110000";

            string valInB2 = "10011100";
            string valInC2 = UnitConverter.ByteToBinary(2);
            string resultA2 = "01110000";


            // set data in register
            ////////////
            /// Test 1
            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rb1),
                UnitConverter.BinaryToHex(valInB1));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc1),
                UnitConverter.BinaryToHex(valInC1));
            ////////////

            ////////////
            /// Test 2
            micro.MicroRegisters.SetRegisterValue(
               (byte)UnitConverter.BinaryToInt(rb2),
               UnitConverter.BinaryToHex(valInB2));

            micro.MicroRegisters.SetRegisterValue(
                (byte)UnitConverter.BinaryToInt(rc2),
                UnitConverter.BinaryToHex(valInC2));
            ///////////

            Console.WriteLine(micro.MicroRegisters);

            // Assert Registers
            Assert.AreEqual("Registers[0,0,60,2,0,0,-100,2]",
                micro.MicroRegisters.ToString());
            Console.WriteLine(i1);
            Console.WriteLine(i2);

            // execute instruction
            InstructionSetExe.ExecuteInstruction(i1, micro);
            InstructionSetExe.ExecuteInstruction(i2, micro);

            Console.WriteLine(micro.MicroRegisters);

            Console.WriteLine($"Expected1: {UnitConverter.BinaryToHex(resultA1)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra1))}");
            Console.WriteLine($"Expected2: {UnitConverter.BinaryToHex(resultA2)}," +
                $"Actual: {micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra2))}");

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA1),
                micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra1))
                );

            Assert.AreEqual(
                UnitConverter.BinaryToHex(resultA2),
                micro.MicroRegisters.GetRegisterValue(UnitConverter.BinaryToByte(ra2))
                );
        }
    }
}
