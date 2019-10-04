using System;
using System.Collections.Generic;
using System.Text;
using Assembler.Interfaces;

namespace Assembler.Parsing.InstructionFormats
{
    class InvalidInstruction : IFormatInstructions
    {

        public InvalidInstruction(Token op, Token[] parms)
        {
            Operator = op;
            Values = parms;
        }

        public Token Operator { get; }

        public Token[] Values { get; }

        public bool IsValid => false;

        public override string ToString()
        {
            return $"InvalidInstruction[op: {Operator.Value}, values: {ArrayToString(Values)}, valid: '{IsValid}']";
        }

        private string ArrayToString(object[] arr)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("[");

            builder.AppendJoin(",", arr);

            builder.Append("]");

            return builder.ToString();
        }
    }
}
