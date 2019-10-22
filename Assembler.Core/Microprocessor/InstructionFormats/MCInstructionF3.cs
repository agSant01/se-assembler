using Assembler.Utils;
using System;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF3 : IMCInstruction
    {
        public MCInstructionF3(ushort decimalAddress, string opCodeBinary, string binaryAddress)
        {
            OpCode = UnitConverter.BinaryToByte(opCodeBinary);
            
            AddressParamHex = UnitConverter.BinaryToHex(binaryAddress);

            InstructionAddressDecimal = decimalAddress;
        }

        public byte OpCode { get; }

        public string AddressParamHex { get; }

        public ushort InstructionAddressDecimal { get; }

        public override bool Equals(object obj)
        {
            return obj is MCInstructionF3 f &&
                   OpCode == f.OpCode &&
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
                hash = hash * 23 + AddressParamHex.GetHashCode();
                hash = hash * 23 + InstructionAddressDecimal.GetHashCode();
                return hash;
            }
            //return HashCode.Combine(OpCode, AddressParamHex, InstructionAddressDecimal);
        }

        public override string ToString()
        {
            return $"MCInstructionF3[InstructionAddressDecimal: (decimal)'{InstructionAddressDecimal}', opcode:'{OpCode}', AddressParamHex:'{AddressParamHex}']";
        }
    }
}
