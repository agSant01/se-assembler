﻿using Assembler.Utils;
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

        public byte BufferSize => (byte)1;

        public short IOPort => throw new NotImplementedException();

        public string DeviceName => throw new NotImplementedException();

        public bool WriteInPort(int port, string contentInHex)
        {
            _buffer = contentInHex;
            return true;
        }

        public string ReadFromPort(int port)
        {
            if (HasData)
                return UnitConverter.HexToBinary(_buffer);
            else
                return "00";
        }

        public bool Reset()
        {
            _buffer = string.Empty;

            return true;
        }
    }
}


/*

    var loQueSeLeo = [something].Read();
    7segIoDevice.WriteInPort(0, loqueSeLeo);
    segmentDisplay.ShowBinary(7segIoDevice.ReadFromPort(0));

*/
