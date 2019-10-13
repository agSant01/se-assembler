using Assembler.Utils;
using System;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF1 : IMCInstruction
    {
        public MCInstructionF1(string opCodeBinary, string Ra, string Rb = null, string Rc = null)
        {
            OpCode = (byte) UnitConverter.BinaryToDecimal(opCodeBinary);

            this.Ra = (byte) UnitConverter.BinaryToDecimal(Ra);
            this.Rb = (byte) UnitConverter.BinaryToDecimal(Rb);
            this.Rc = (byte) UnitConverter.BinaryToDecimal(Rc);
        }

        public byte OpCode { get; }

        public byte Ra { get; }

        public byte Rb { get; }
        
        public byte Rc { get; }
    }
}
