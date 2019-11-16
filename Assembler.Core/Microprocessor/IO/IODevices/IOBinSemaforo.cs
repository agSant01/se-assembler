using System;

namespace Assembler.Core.Microprocessor.IO.IODevices
{
    public class IOBinSemaforo : IIODevice, IObservable<char[]>
    {
        public short IOPort { get; }

        public short IOPortLength => 1;

        public bool HasData { get; private set; } = false;

        public string DeviceName => "IO Bin Semaforo";

        private string content = "";

        public char[] BitContent { get; set; } = { '0', '0', '0', '0', '0', '0', '0', '0' };

        public Action GotBinContent;

        private readonly bool _debug;

        public IOBinSemaforo(short ioPort)
        {
            IOPort = ioPort;
            _debug = false;
        }

        public IOBinSemaforo(short ioPort, string debug)
        {
            IOPort = ioPort;
            if (debug == "#Debug")
                _debug = true;
        }

        public bool WriteInPort(int port, string contentInHex)
        {
            string binVal = Utils.UnitConverter.HexToBinary(contentInHex);
            BitContent = binVal.ToCharArray();
            content = contentInHex;
            HasData = true;
            if (!_debug)
                GotBinContent();
            return true;
        }

        public bool Reset()
        {
            HasData = false;
            BitContent = new char[] { '0', '0', '0', '0', '0', '0', '0', '0' };
            if (!_debug)
                GotBinContent();
            return true;
        }

        public string ReadFromPort(int port)
        {
            throw new InvalidOperationException("Invalid call on a write only device");
        }

        public IDisposable Subscribe(IObserver<char[]> observer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"IOBinSemaforo[port: {IOPort}, binary content: {content}]";
        }
    }
}
