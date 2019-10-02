using Assembler.Interfaces;
using Assembler.Parsing;
using Assembler.Parsing.InstructionItems;
using System;

namespace Assembler.Parsing.InstructionFormats
{
    class OriginCmd : IFormatInstructions
    {
        public OriginCmd(Token op, Token address)
        {
            Operator = op;
            Address = new Hexa(address);
        }

        public Token Operator { get; }

        public Hexa Address { get; }

        public bool IsValid => Address.IsValid();

        public override string ToString()
        {
            return $"ORIGIN[op: '{Operator.Value}', address: '{Address}', valid: '{IsValid}']";
        }
    }
}
