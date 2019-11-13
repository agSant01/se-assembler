using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO;
using Assembler.Microprocessor;
using Assembler.Utils;
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
            IOManager io = new IOManager(100);

            io.AddIODevice(80, new Device() { Id = 1 });
            io.AddIODevice(82, new Device() { Id = 2 });

            Console.WriteLine(io);

            Assert.AreEqual(2, io.ConnectedDevices);

            Assert.IsTrue(io.RemoveIODevice(80));

            Assert.AreEqual(1, io.ConnectedDevices);
        }

        [TestMethod]
        public void IOManagerTests_WriteToIODevice_Success()
        {
            IOManager io = new IOManager(100);

            Device d1 = new Device() { Id = 1 };
            Device d2 = new Device() { Id = 2 };

            io.AddIODevice(80, d1);
            io.AddIODevice(82, d2);

            MicroSimulator micro = new MicroSimulator(
                new VirtualMemory(new string[]{ }),
                io
            );

            micro.WriteToMemory(80, "F3");
            micro.WriteToMemory(82, "08");

            Console.WriteLine(d1);
            Console.WriteLine(d2);

            Assert.AreEqual("Device[Id: 1, Data: 243]", d1.ToString());
            Assert.AreEqual("Device[Id: 2, Data: 8]", d2.ToString());
            
            Console.WriteLine(micro.ReadFromMemory(80));
            Console.WriteLine(micro.ReadFromMemory(82));
        }
    }

    class Device : IIODevice
    {
        private byte binaryData = 0;

        public int Id { get; set; }

        short IIODevice.IOPortLength => 1;

        bool IIODevice.HasData => true;

        public short IOPort => throw new NotImplementedException();

        public string DeviceName => throw new NotImplementedException();

        string IIODevice.ReadFromPort(int port)
        {
            return UnitConverter.ByteToHex(binaryData);
        }

        bool IIODevice.Reset()
        {
            binaryData = 0;
            return true;
        }

        bool IIODevice.WriteInPort(int port, string contentInHex)
        {
            binaryData = UnitConverter.HexToByte(contentInHex);
            return true;
        }

        public override string ToString()
        {
            return $"Device[Id: {Id}, Data: {binaryData}]";
        }
    }
}