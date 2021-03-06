﻿using Assembler.Core.Microprocessor;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using System;

namespace Assembler.Microprocessor
{
    public class MicroSimulator
    {
        private readonly MCLoader _mcLoader;

        private readonly VirtualMemory _virtualMemory;

        private readonly IOManager _ioManager;

        private readonly ushort PC_SIZE = 11;

        private ushort _programCounter = 0;

        private ushort _stackPointer = 0;

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

        public ushort StackPointer
        {
            get => _stackPointer;
            set => _stackPointer = (ushort)(value % _virtualMemory.VirtualMemorySize);
        }

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

        public ushort? LastModifiedMemoryAddress { get; private set; } = null;

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
            if (_ioManager.IsUsedPort((ushort)decimalAddress))
            {
                _ioManager.WriteToIO((ushort)decimalAddress, contentInHex);
            }
            else
            {
                _virtualMemory.SetContentInMemory(decimalAddress: decimalAddress, hexContent: contentInHex);

                LastModifiedMemoryAddress = (ushort) decimalAddress;
            }
        }

        /// <summary>
        /// Read contents from Micro memory. Contents are returned in Hexadecimal
        /// </summary>
        /// <param name="decimalAddress">Decimal address to read content</param>
        /// <returns></returns>
        public string ReadFromMemory(int decimalAddress)
        {
            if (_ioManager.IsUsedPort((ushort)decimalAddress))
            {
                return _ioManager.ReadFromIO((ushort)decimalAddress);
            }
            return _virtualMemory.GetContentsInHex(decimalAddress: decimalAddress);
        }

        public void NextInstruction()
        {
            PreviousInstruction = CurrentInstruction;
            CurrentInstruction = _mcLoader.NextInstruction();
        }

        public IMCInstruction PeekNextInstruction()
        {
            return _mcLoader?.PeekNextInstruction();
        }
    }
}
