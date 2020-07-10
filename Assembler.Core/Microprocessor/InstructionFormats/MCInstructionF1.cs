using Assembler.Utils;
using System;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF1 : IMCInstruction
    {
        public MCInstructionF1(ushort decimalAddress, string opCodeBinary, string Ra, string Rb = null, string Rc = null)
        {
            OpCode = (byte)UnitConverter.BinaryToInt(opCodeBinary);

            this.Ra = Ra == null || Ra.Length == 0 ? (byte) 0 : UnitConverter.BinaryToByte(Ra);
            this.Rb = Rb == null ? (byte) 0 : UnitConverter.BinaryToByte(Rb);
            this.Rc = Rc == null ? (byte) 0 : UnitConverter.BinaryToByte(Rc);

            InstructionAddressDecimal = decimalAddress;
        }

        public byte OpCode { get; }

        public byte Ra { get; }

        public byte Rb { get; }

        public byte Rc { get; }

        public ushort InstructionAddressDecimal { get; }

        public override bool Equals(object obj)
        {
            return obj is MCInstructionF1 f &&
                   OpCode == f.OpCode &&
                   Ra == f.Ra &&
                   Rb == f.Rb &&
                   Rc == f.Rc &&
                   InstructionAddressDecimal == f.InstructionAddressDecimal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OpCode, Ra, Rb, Rc, InstructionAddressDecimal);
        }

        public override string ToString()
        {
            if (IMCInstruction.AsmTextPrint)
            {
                string itr = $"{OpCodesInfo.GetOpName(UnitConverter.ByteToBinary(OpCode, defaultWidth: 5))}";

                if (Ra >= 0 && Ra <= 7) itr += $" R{Ra}";
                
                if (Rb >= 0 && Rb <= 7) itr += $" R{Rb}";

                if (Rc >= 0 && Rc <= 7) itr += $" R{Rc}";

                return itr;
            }
            return $"MCInstructionF1[InstructionAddressDecimal: (decimal)'{InstructionAddressDecimal}', opcode:'{OpCode}', Ra:'{Ra}', Rb:'{Rb}', Rc:'{Rc}']";
        }
    }
}
