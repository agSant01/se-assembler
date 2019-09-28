using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Assembler.UnitTests
{
    [TestClass]
    public class AssemblyLoggerTest
    {
        private AssemblyLogger logger;

        [TestInitialize]
        public void TestInit()
        {
            logger = new AssemblyLogger();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            logger = null;
        }

        [TestMethod]
        public void CompilationLogger_StartDateCreation_CreatesADateString()
        {
            string[] data = logger.GetLines();

            Assert.AreEqual(1, data.Length);

            Assert.IsNotNull(data[0]);

            Console.WriteLine(data[0]);
        }

        [TestMethod]
        public void CompilationLogger_AddStatus_RegisterATestMessage()
        {
            string txt = "Started Parsing.";
            this.logger.StatusUpdate(txt);

            string[] data = logger.GetLines();

            Assert.AreEqual(2, data.Length);

            Assert.IsNotNull(data[0]);

            Assert.AreEqual($"[STATUS] {txt}", data[1]);


            foreach (string d in data)
            {
                Console.WriteLine(d);
            }

        }

        [TestMethod]
        public void CompilationLogger_AddWarning_RegisterAWarnignMessage()
        {
            string msg = "Memory Overwrite";
            string adrs = "0x45";
            string old = "3524";
            string line = "54";

            this.logger.Warning(msg, line, adrs, old);

            string[] data = logger.GetLines();

            Assert.AreEqual(2, data.Length);

            Assert.AreEqual($"[WARNING] {msg}. Address {adrs} overwrite [content: '{old}'] at line {line}", data[1]);

            foreach (string d in data)
            {
                Console.WriteLine(d);
            }

        }
    }
}
