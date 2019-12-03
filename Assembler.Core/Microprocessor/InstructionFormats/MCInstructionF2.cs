using Assembler.Utils;
using System;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF2 : IMCInstruction
    {
        public MCInstructionF2(ushort decimalAddress, string opCodeBinary, string Ra, string binaryAddress)
        {
            OpCode = UnitConverter.BinaryToByte(opCodeBinary);

            this.Ra = Ra == null ? (byte) 8 : UnitConverter.BinaryToByte(Ra);

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
            return HashCode.Combine(OpCode, Ra, AddressParamHex, InstructionAddressDecimal);
        }

        public override string ToString()
        {
            if (IMCInstruction.AsmTextPrint)
            {
                if (OpCodesInfo.GetOpName(UnitConverter.ByteToBinary(OpCode, defaultWidth: 5)).ToLower().Equals("return"))
                {
                    return "RETURN";
                }

                string itr = $"{OpCodesInfo.GetOpName(UnitConverter.ByteToBinary(OpCode, defaultWidth: 5))}";

                if (Ra >= 0 && Ra <= 7) itr += $" R{Ra}";

                return itr;
            }

            return $"MCInstructionF2[InstructionAddressDecimal: (decimal)'{InstructionAddressDecimal}', opcode:'{OpCode}', Ra:'{Ra}', AddressParamHex:'{AddressParamHex}']";
        }
    }
}
