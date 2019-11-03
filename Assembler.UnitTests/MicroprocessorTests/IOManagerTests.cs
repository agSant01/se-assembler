using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Assembler.UnitTests.MicroprocessorTests
{
    [TestClass]
    public class IOManagerTests
    {

        [TestMethod]
        public void IOManagerTests_AddAnIODevice_Success()
        {
            IOManager io = new IOManager();

            io.AddIODevice(80, new Device() { Id = 1 });
            io.AddIODevice(82, new Device() { Id = 2 });

            Console.WriteLine(io);

            Assert.AreEqual(2, io.ConnectedDevices);

            Assert.IsTrue(io.RemoveIODevice(80));

            Assert.AreEqual(1, io.ConnectedDevices);
        }
    }

    class Device : IODevice
    {
        public int Id { get; set; }

        short IODevice.IOPortLength => 1;

        bool IODevice.HasData => true;

        string IODevice.ReadFromPort(int port)
        {
            return "01100110";
        }

        bool IODevice.Reset()
        {
            return true;
        }

        bool IODevice.WriteInPort(int port, string contentInHex)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Device[Id:{Id}]";
        }
    }
}