using Assembler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parser.InstructionFormats
{
    public class InstructionFormat2 : IFormatInstructions
    {

        public InstructionFormat2(string opcode, int registerA, string constOrAddress)
        {
            OperatorCode = opcode;
            RegisterA = registerA;
            ConstOrAddress = constOrAddress;
        }

        public string OperatorCode { get; }

        public int RegisterA { get; }

        public string ConstOrAddress { get; }
    }
}
