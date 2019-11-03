using Assembler.Utils;
using System;

namespace Assembler.Microprocessor.InstructionFormats
{
    public class MCInstructionF1 : IMCInstruction
    {
        public MCInstructionF1(ushort decimalAddress, string opCodeBinary, string Ra, string Rb = null, string Rc = null)
        {
            OpCode = (byte) UnitConverter.BinaryToInt(opCodeBinary);

            this.Ra = UnitConverter.BinaryToByte(Ra);
            this.Rb = UnitConverter.BinaryToByte(Rb);
            this.Rc = UnitConverter.BinaryToByte(Rc);

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
                string itr = $"{OpCodesInfo.GetOpName(UnitConverter.ByteToBinary(OpCode, defaultWidth: 5))} R{Ra}";

                if (Rb != 0) itr += $" R{Rb}";

                if (Rc != 0) itr += $" R{Rc}";
                
                return itr;
            }
            return $"MCInstructionF1[InstructionAddressDecimal: (decimal)'{InstructionAddressDecimal}', opcode:'{OpCode}', Ra:'{Ra}', Rb:'{Rb}', Rc:'{Rc}']";
        }
    }
}
