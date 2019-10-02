using Assembler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parser.InstructionFormats
{
    public class InstructionFormat3 : IFormatInstructions
    {

        public InstructionFormat3(string opcode, string constOrAddress)
        {
            OperatorCode = opcode;
            ConstOrAddress = constOrAddress;
        }

        public string OperatorCode { get; }

        public string ConstOrAddress { get; }
    }
}
