using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.Microprocessor.IO.IODevices
{
    public class IOBinSemaforo: IIODevice , IObservable<char[]>
    {
        public short IOPort { get; }

        public short IOPortLength => 1;

        public bool HasData => BitContent == null;

        public string DeviceName => "IO Bin Semaforo";

        private string content = "";

        public char[] BitContent { get; set; }

        public Action GotBinContent;

        public IOBinSemaforo(short ioPort)
        {
            IOPort = ioPort;
        }

        public bool WriteInPort(int port, string contentInHex)
        {
            string binVal = Utils.UnitConverter.HexToBinary(contentInHex);
            BitContent = binVal.ToCharArray();
            content = contentInHex;
            GotBinContent();
            return true;
        }

        public bool Reset()
        {
            BitContent = null;
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
