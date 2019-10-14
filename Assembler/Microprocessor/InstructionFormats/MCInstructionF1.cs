using Assembler.Utils;
using System;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF1 : IMCInstruction
    {
        public MCInstructionF1(ushort decimalAddress, string opCodeBinary, string Ra, string Rb = null, string Rc = null)
        {
            OpCode = (byte) UnitConverter.BinaryToDecimal(opCodeBinary);

            this.Ra = (byte) UnitConverter.BinaryToDecimal(Ra);
            this.Rb = (byte) UnitConverter.BinaryToDecimal(Rb);
            this.Rc = (byte) UnitConverter.BinaryToDecimal(Rc);

            MemoryAddressDecimal = decimalAddress;
        }

        public byte OpCode { get; }

        public byte Ra { get; }

        public byte Rb { get; }
        
        public byte Rc { get; }

        public ushort MemoryAddressDecimal { get; }

        public override bool Equals(object obj)
        {
            return obj is MCInstructionF1 f &&
                   OpCode == f.OpCode &&
                   Ra == f.Ra &&
                   Rb == f.Rb &&
                   Rc == f.Rc &&
                   MemoryAddressDecimal == f.MemoryAddressDecimal;
        }

        public override string ToString()
        {
            return $"MCInstructionF1[MemoryAddress: (decimal)'{MemoryAddressDecimal}', opcode:'{OpCode}', Ra:'{Ra}', Rb:'{Rb}', Rc:'{Rc}']";
        }
    }
}
