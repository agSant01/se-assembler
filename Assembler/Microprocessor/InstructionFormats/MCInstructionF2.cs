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

            AddressParamHex = UnitConverter.BinaryToHex(binaryAddress);

            InstructionAddressDecimal = decimalAddress;
        }

        public byte OpCode { get; }

        public byte Ra { get; }

        public string AddressParamHex { get; }

        public ushort InstructionAddressDecimal { get; }

        public override bool Equals(object obj)
        {
            return obj is MCInstructionF2 f &&
                   OpCode == f.OpCode &&
                   Ra == f.Ra &&
                   AddressParamHex == f.AddressParamHex &&
                   InstructionAddressDecimal == f.InstructionAddressDecimal;
        }

        public override string ToString()
        {
            return $"MCInstructionF2[InstructionAddressDecimal: (decimal)'{InstructionAddressDecimal}', opcode:'{OpCode}', Ra:'{Ra}', AddressParamHex:'{AddressParamHex}']";
        }
    }
}
