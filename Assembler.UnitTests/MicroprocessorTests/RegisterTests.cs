using Assembler.Microprocessor;
using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Assembler.UnitTests.MicroprocessorTests
{
    [TestClass]
    public class RegisterTests
    {
        private Registers registers;

        [TestInitialize]
        public void TestSetup()
        {
            registers = new Registers();
        }

        [TestCleanup]
        public void Cleanup()
        {
            registers = null;
        }

        [TestMethod]
        public void RegisterTests_SaveData_Success()
        {
            string test = "25";
            byte registerToSave = 2;

            registers.SetRegisterValue(registerToSave, test);

            string result = registers.GetRegisterValue(registerToSave);

            Assert.AreEqual(test, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException), "Should have thrown Exception. The saved data is to big for this regiter.")]
        public void RegisterTests_SaveInvalidData_Success()
        {
            string test = "1A5";

            registers.SetRegisterValue(2, test);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException), "Should have thrown Exception. The saved data is to big for this register.")]
        public void RegisterTests_SaveNegativeInvalidData_Success()
        {
            string hexTest = UnitConverter.DecimalToHex(-129);

            registers.SetRegisterValue(2, hexTest);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "Should have thrown Exception. Invalid Register Number.")]
        public void RegisterTests_SaveToInvalidRegister_Success()
        {
            string test = "25";
            byte registerToSave = 9;

            registers.SetRegisterValue(registerToSave, test);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "Should have thrown Exception. Invalid Register Number.")]
        public void RegisterTests_GetFromInvalidIndex_Success()
        {
            string test = "25";
            byte registerToSave = 2;

            registers.SetRegisterValue(registerToSave, test);

            registers.GetRegisterValue(8);
        }

        [TestMethod]
        public void RegisterTests_GetFromEmptyRegister_Success()
        {
            string expected = "0";

            string result = registers.GetRegisterValue(3);

            Console.WriteLine($"Expected: {expected}, Result: {result}");

            Assert.AreEqual(expected, result);
        }
    }
}
