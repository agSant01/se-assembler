using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.Microprocessor.IO
{
    public interface IIODevice
    {
        public short IOPortLength { get; }

        public bool HasData { get; }

        public bool WriteInPort(int port, string contentInHex);

        public string ReadFromPort(int port);

        public bool Reset();
    }
}
