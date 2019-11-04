using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.Microprocessor.IO
{
    public interface IIODevice
    {
        public short IOPort { get; }

        public short IOPortLength { get; }

        public bool HasData { get; }

        public bool WriteInPort(int port, string contentInHex);

        /// <summary>
        /// Read data from IO. Returned in Hexadecimal format
        /// </summary>
        /// <param name="port">IO port</param>
        /// <returns>hexadecimal data representation</returns>
        public string ReadFromPort(int port);

        public bool Reset();

        public string DeviceName { get; }
    }
}
