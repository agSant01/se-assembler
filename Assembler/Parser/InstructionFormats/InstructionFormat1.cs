using Assembler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parser.InstructionFormats
{
    public class InstructionFormat1 : IFormatInstructions
    {

        public InstructionFormat1(string opcode, int registerA, int registerB, int registerC )
        {
            OperatorCode = opcode;
            RegisterA = registerA;
            RegisterB = registerB;
            RegisterC = registerC;
        }

        public string OperatorCode { get; }

        public int RegisterA { get; }

        public int RegisterB { get; }

        public int RegisterC { get; }
    }
}
