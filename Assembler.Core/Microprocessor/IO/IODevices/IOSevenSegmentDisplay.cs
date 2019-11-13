using Assembler.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assembler.Core.Microprocessor.IO.IODevices
{
   public class IOSevenSegmentDisplay : IIODevice

    {
        public short IOPort { get; }

        public string DeviceName => "IO 7-Segment Display";

        private string _buffer = string.Empty;

        public short IOPortLength => 1;

        public bool HasData => !string.IsNullOrEmpty(_buffer);
        public string Data { get { return _buffer; } }
        public byte BufferSize => (byte)1;

        public Action UpdateGui;

        public IOSevenSegmentDisplay(short ioPort)
        {
            IOPort = ioPort;
        }
        public bool WriteInPort(int port, string contentInHex)
        {
            _buffer = UnitConverter.HexToBinary(contentInHex);
            UpdateGui?.Invoke();
            return true;
        }

        public string ReadFromPort(int port)
        {
            throw new InvalidOperationException("Invalid call on a write only device");
        }

        public bool Reset()
        {
            _buffer = string.Empty;
            return true;
        }

        public override string ToString()
        {
            return $"IOSevenSegmentDisplay[port: {IOPort}, binary content: {Data}]";
        }
    }
}