using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using System;

namespace Assembler.Microprocessor
{
    public class MicroSimulator
    {
        public static readonly short SEGMENT_IO_PORT = 0;
        
        
        private readonly MCLoader _mcLoader;

        private readonly VirtualMemory _virtualMemory;

        private readonly IOManager _ioManager;

        private readonly ushort PC_SIZE = 11;

        private ushort _programCounter = 0;
               
        public MicroSimulator(VirtualMemory virtualMemory)
        {
            MicroRegisters = new Registers();

            _virtualMemory = virtualMemory;

            _mcLoader = new MCLoader(virtualMemory, this);

            _ioManager = new IOManager(_virtualMemory.VirtualMemorySize);
        }

        public MicroSimulator(VirtualMemory virtualMemory, IOManager iOManager) 
            : this(virtualMemory)
        {
            _ioManager = iOManager;
        }

        public Registers MicroRegisters { get; }

        public ushort StackPointer { get; set; }

        public ushort ProgramCounter
        {
            get { return _programCounter; }
            set
            {
                if (value >= Math.Pow(2, PC_SIZE))
                {
                    throw new OverflowException($"Value {value} is too big for ProgramCounter." +
                        $" Max Value is {Math.Pow(2, PC_SIZE) - 1}");
                }
                _programCounter = value;
            }
        }

        public bool ConditionalBit { get; set; } = false;

        public IMCInstruction CurrentInstruction { get; private set; }

        public IMCInstruction PreviousInstruction { get; private set; }

        public override string ToString()
        {
            return $"Microprocessor[PC={ProgramCounter}, CondBit={(ConditionalBit ? 1 : 0)}]";
        }

        /// <summary>
        /// Write contents in hexadecimal to Micro memory
        /// </summary>
        /// <param name="decimalAddress">Decimal address to write contents</param>
        /// <param name="contentInHex">Contents to write in Hexadecimal</param>
        public void WriteToMemory(int decimalAddress, string contentInHex)
        {
            if (_ioManager.IsUsedPort((short)decimalAddress))
            {
                _ioManager.WriteToIO((short)decimalAddress, contentInHex);
            } else { 
               _virtualMemory.SetContentInMemory(decimalAddress: decimalAddress, hexContent: contentInHex);
            }
        }

        /// <summary>
        /// Read contents from Micro memory. Contents are returned in Hexadecimal
        /// </summary>
        /// <param name="decimalAddress">Decimal address to read content</param>
        /// <returns></returns>
        public string ReadFromMemory(int decimalAddress)
        {
            if (_ioManager.IsUsedPort((short)decimalAddress))
            {
                return _ioManager.ReadFromIO((short)decimalAddress);
            }
            return _virtualMemory.GetContentsInHex(decimalAddress: decimalAddress);
        }

        /// <summary>
        /// Tries to connect an IO Device to the micro
        /// </summary>
        /// <returns>Whether the device was connected</returns>
        public bool AddDevice(short port, IIODevice device)
        {
            if (_ioManager.IsUsedPort(port))
                return false;
            _ioManager.AddIODevice(port, device);
            return true;
            
        }

        public void NextInstruction()
        {
            PreviousInstruction = CurrentInstruction;
            CurrentInstruction = _mcLoader.NextInstruction();

            if (!OpCodesInfo.IsJump(UnitConverter.IntToBinary(CurrentInstruction.OpCode, 5)))
            {
                ProgramCounter += 2;
            }
        }
    }
}
