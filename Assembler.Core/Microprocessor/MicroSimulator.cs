using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using System;

namespace Assembler.Microprocessor
{
    public class MicroSimulator
    {
        private readonly MCLoader _mcLoader;

        private readonly ushort PC_SIZE = 11;

        private ushort _programCounter = 0;

        public IMCInstruction currentInstruction, previousInstruction;


        public MicroSimulator(VirtualMemory virtualMemory)
        {
            MicroVirtualMemory = virtualMemory;

            MicroRegisters = new Registers();

            _mcLoader = new MCLoader(virtualMemory, this);
        }

        public Registers MicroRegisters { get; }

        public VirtualMemory MicroVirtualMemory { get; }

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

        public override string ToString()
        {
            return $"Microprocessor[PC={ProgramCounter}, CondBit={(ConditionalBit ? 1 : 0)}]";
        }

        public void NextInstruction()
        {
            previousInstruction = currentInstruction;
            currentInstruction = _mcLoader.NextInstruction();

            if (OpCodesInfo.IsJump(UnitConverter.IntToBinary(currentInstruction.OpCode, 5)))
            {
                this.ProgramCounter = (ushort)UnitConverter.HexToInt(
                    ((MCInstructionF3)currentInstruction).AddressParamHex);
            }
            else
            {
                this.ProgramCounter += 2;
            }
        }

    }
}
