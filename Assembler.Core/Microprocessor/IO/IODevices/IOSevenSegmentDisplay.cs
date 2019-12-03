using Assembler.Utils;
using System;

namespace Assembler.Core.Microprocessor.IO.IODevices
{
    public class IOSevenSegmentDisplay : IIODevice

    {
        public ushort IOPort { get; }

        public string DeviceName => "IO 7-Segment Display";

        public ushort IOPortLength => 1;

        public bool HasData => !string.IsNullOrEmpty(Data);

        public string Data { get; private set; } = string.Empty;
        
        public byte BufferSize => 1;

        public Action UpdateGui;

        public IOSevenSegmentDisplay(ushort ioPort)
        {
            IOPort = ioPort;
        }
        public bool WriteInPort(int port, string contentInHex)
        {
            Data = UnitConverter.HexToBinary(contentInHex);
            UpdateGui?.Invoke();
            return true;
        }

        public string ReadFromPort(int port)
        {
            throw new InvalidOperationException("Invalid call on a write only device");
        }

        public bool Reset()
        {
            Data = string.Empty;
            UpdateGui?.Invoke();
            return true;
        }

        public override string ToString()
        {
            return $"IOSevenSegmentDisplay[port: {IOPort}, binary content: {Data}]";
        }
    }
}