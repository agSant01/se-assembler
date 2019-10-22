using Assembler.Utils;
using System;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF2 : IMCInstruction
    {
        public MCInstructionF2(ushort decimalAddress, string opCodeBinary, string Ra, string binaryAddress)
        {
            OpCode = UnitConverter.BinaryToByte(opCodeBinary);

            this.Ra = UnitConverter.BinaryToByte(Ra);

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

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + OpCode.GetHashCode();
                hash = hash * 23 + Ra.GetHashCode();
                hash = hash * 23 + AddressParamHex.GetHashCode();
                hash = hash * 23 + InstructionAddressDecimal.GetHashCode();
                return hash;
            }
            //return HashCode.Combine(OpCode, Ra, AddressParamHex, InstructionAddressDecimal);
        }

        public override string ToString()
        {
            return $"MCInstructionF2[InstructionAddressDecimal: (decimal)'{InstructionAddressDecimal}', opcode:'{OpCode}', Ra:'{Ra}', AddressParamHex:'{AddressParamHex}']";
        }
    }
}
