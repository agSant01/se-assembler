using Assembler.Utils;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF3 : IMCInstruction
    {
        public MCInstructionF3(string opCodeBinary, string binaryAddress)
        {
            OpCode = (byte) UnitConverter.BinaryToDecimal(opCodeBinary);

            HexAddress = UnitConverter.BinaryToHex(binaryAddress);
        }

        public byte OpCode { get; }

        public string HexAddress { get; }
    }
}
