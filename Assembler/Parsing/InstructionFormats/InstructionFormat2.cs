using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionFormats
{
    public class InstructionFormat2 : IFormatInstructions
    {

        public InstructionFormat2(Token opcode, Token registerA, Token constOrAddress)
        {
            Operator = opcode;
            if (opcode.Value.ToLower().Equals("store"))
            {
                RegisterA = new Register(constOrAddress);
                ConstOrAddress = new VariableName(registerA);
            }
            else
            {
                RegisterA = new Register(registerA);
                ConstOrAddress = new VariableName(constOrAddress);
            }
        }

        public Token Operator { get; }

        public Register RegisterA { get; }

        public VariableName ConstOrAddress { get; }

        public bool IsValid => RegisterA.IsValid() && ConstOrAddress.IsValid();
        
        public override string ToString()
        {
            return $"IF2[op: '{Operator.Value}', Ra: '{RegisterA}', Const/Address: '{ConstOrAddress}', valid: '{IsValid}']";
        }
    }
}
