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
    public class MicroSimulatorTests
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
    }
}