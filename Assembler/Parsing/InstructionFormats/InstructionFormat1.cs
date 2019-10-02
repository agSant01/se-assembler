using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionFormats
{
    public class InstructionFormat1 : IFormatInstructions
    {

        public InstructionFormat1(Token op, Token registerA, Token registerB, Token  registerC)
        {
            Operator = op;
            RegisterA = new Register(registerA);
            RegisterB = new Register(registerB);
            RegisterC = new Register(registerC);
        }

        public Token Operator { get; }

        public Register RegisterA { get; }

        public Register RegisterB { get; }

        public Register RegisterC { get; }

        public bool IsValid => RegisterA.IsValid() && RegisterB.IsValid() && RegisterC.IsValid();

        public override string ToString()
        {
            return $"IF1[op: '{Operator.Value}', Ra: '{RegisterA}', Rb: '{RegisterB}', Rc: '{RegisterC}', valid: '{IsValid}']";
        }
    }
}
