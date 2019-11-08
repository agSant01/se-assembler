using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.Microprocessor.IO.IODevices
{
    class IOBinSemaforo: IIODevice
    {
        public short IOPort { get; }

        public short IOPortLength => 1;

        public bool HasData => _bitContent == null;

        public string DeviceName => "IO Bin Semaforo";

        private char[] _bitContent;

        public IOBinSemaforo(short ioPort)
        {
            IOPort = ioPort;
        }


        public bool WriteInPort(int port, string contentInHex)
        {
            string binVal = Utils.UnitConverter.HexToBinary(contentInHex);
            _bitContent = binVal.ToCharArray();
            return true;
        }

        public bool Reset()
        {
            _bitContent = null;
            return true;
        }

        public string ReadFromPort(int port)
        {
            throw new InvalidOperationException("Invalid call on a write only device");
        }
    }
}
