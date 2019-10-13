using Assembler.Utils;
using System;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF2 : IMCInstruction
    {
        public MCInstructionF2(string opCodeBinary, string Ra, string binaryAddress)
        {
            OpCode = (byte) UnitConverter.BinaryToDecimal(opCodeBinary);

            this.Ra = (byte) UnitConverter.BinaryToDecimal(Ra);

            HexAddress = UnitConverter.BinaryToHex(binaryAddress);
        }

        public byte OpCode { get; }

        public byte Ra { get; }

        public string HexAddress { get; }
    }
}
