using Assembler.Utils;
using System;
using System.Collections.Generic;

namespace Assembler.Core.Microprocessor.IO.IODevices
{
    public class IOHexKeyboard : IIODevice
    {
        private readonly Queue<string> _buffer = new Queue<string>();

        public short IOPortLength => 1;

        public bool HasData => _buffer.Count > 0;

        public byte BufferSize => (byte)_buffer.Count;

        public short IOPort { get; }

        public string DeviceName => "IO Hexadecimal Keyboard";

        public IOHexKeyboard(short ioPort)
        {
            IOPort = ioPort;
        }

        public string ReadFromPort(int port)
        {
            if (_buffer.Count == 0)
            {
                return UnitConverter.ByteToHex(0);
            }

            return _buffer.Dequeue();
        }

        public bool Reset()
        {
            _buffer.Clear();

            return true;
        }

        public bool WriteInPort(int port, string contentInHex)
        {
            throw new InvalidOperationException("This port is reserved for read-only operatoins");
        }

        public void KeyPress(string hexChar)
        {
            if (_buffer.Count < 4)
            {
                _buffer.Enqueue(hexChar + "1");
            }
        }

        public override string ToString()
        {
            return $"IOHexKeyboard[port: {IOPort}, buffer: {String.Join(", ", _buffer.ToArray())}]";
        }
    }
}
