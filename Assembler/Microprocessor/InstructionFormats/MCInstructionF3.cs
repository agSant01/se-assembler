using Assembler.Utils;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF3 : IMCInstruction
    {
        public MCInstructionF3(ushort decimalAddress, string opCodeBinary, string binaryAddress)
        {
            OpCode = (byte) UnitConverter.BinaryToDecimal(opCodeBinary);

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

        public override string ToString()
        {
            return $"MCInstructionF3[InstructionAddressDecimal: (decimal)'{InstructionAddressDecimal}', opcode:'{OpCode}', AddressParamHex:'{AddressParamHex}']";
        }
    }
}
