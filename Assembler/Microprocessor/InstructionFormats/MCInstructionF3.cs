using Assembler.Utils;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF3 : IMCInstruction
    {
        public MCInstructionF3(ushort decimalAddress, string opCodeBinary, string binaryAddress)
        {
            OpCode = (byte) UnitConverter.BinaryToDecimal(opCodeBinary);

            HexAddress = UnitConverter.BinaryToHex(binaryAddress);

            MemoryAddressDecimal = decimalAddress;
        }

        public byte OpCode { get; }

        public string HexAddress { get; }

        public ushort MemoryAddressDecimal { get; }

        public override bool Equals(object obj)
        {
            return obj is MCInstructionF3 f &&
                   OpCode == f.OpCode &&
                   HexAddress == f.HexAddress &&
                   MemoryAddressDecimal == f.MemoryAddressDecimal;
        }

        public override string ToString()
        {
            return $"MCInstructionF1[MemoryAddress: (decimal)'{MemoryAddressDecimal}', opcode:'{OpCode}', Address:'{HexAddress}']";
        }
    }
}
