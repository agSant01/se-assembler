using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Assembler.UnitTests
{
    [TestClass]
    public class AssemblyLoggerTest
    {
        private AssemblyLogger logger;

        [TestInitialize]
        public void TestInit()
        {
            logger = new AssemblyLogger("TEST");
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
        public void CompilationLogger_StatusUpdate_RegisterATestMessage()
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
        public void CompilationLogger_Warning_RegisterAWarnignMessage()
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

        [TestMethod]
        public void CompilationLogger_UseEnumerator_AddingToTheQueueAfterFinishing()
        {
            string msg = "Memory Overwrite";
            string adrs = "0x45";
            string old = "3524";
            string line = "54";
            string txt = "Started Parsing.";

            logger.Warning(msg, line, adrs, old);

            logger.Warning(msg, line, adrs, old);


            List<string> list = new List<string>();

            while (logger.MoveNext())
            {
                list.Add(logger.Current);
            }

            this.logger.StatusUpdate(txt);

            while (logger.MoveNext())
            {
                list.Add(logger.Current);
            }

            foreach (string item in list)
            {
                Console.WriteLine(item);
            }

            Assert.AreEqual(7, list.Count);
        }

        [TestMethod]
        public void CompilationLogger_UseEnumerator_AddingToTheQueue()
        {
            string msg = "Memory Overwrite";
            string adrs = "0x45";
            string old = "3524";
            string line = "54";

            logger.Warning(msg, line, adrs, old);

            logger.Warning(msg, line, adrs, old);


            List<string> list = new List<string>();

            while (logger.MoveNext())
            {
                list.Add(logger.Current);
            }

            Assert.AreEqual(3, list.Count);


            foreach (string item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
