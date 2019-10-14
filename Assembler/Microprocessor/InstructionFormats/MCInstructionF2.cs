using Assembler.Utils;
using System;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF2 : IMCInstruction
    {
        public MCInstructionF2(ushort decimalAddress, string opCodeBinary, string Ra, string binaryAddress)
        {
            OpCode = (byte) UnitConverter.BinaryToDecimal(opCodeBinary);

            this.Ra = (byte) UnitConverter.BinaryToDecimal(Ra);

            HexAddress = UnitConverter.BinaryToHex(binaryAddress);

            MemoryAddressDecimal = decimalAddress;
        }

        public byte OpCode { get; }

        public byte Ra { get; }

        public string HexAddress { get; }

        public ushort MemoryAddressDecimal { get; }

        public override bool Equals(object obj)
        {
            return obj is MCInstructionF2 f &&
                   OpCode == f.OpCode &&
                   Ra == f.Ra &&
                   HexAddress == f.HexAddress &&
                   MemoryAddressDecimal == f.MemoryAddressDecimal;
        }

        public override string ToString()
        {
            return $"MCInstructionF1[MemoryAddress: (decimal)'{MemoryAddressDecimal}', opcode:'{OpCode}', Ra:'{Ra}', Address:'{HexAddress}']";
        }
    }
}
