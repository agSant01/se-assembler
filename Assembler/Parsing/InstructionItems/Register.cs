using Assembler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionItems
{
    public class Register : IFormatUnit
    {
        public Register(Token token)
        {
            Token = token;
        }

        public Token Token { get; }

        public bool IsValid()
        {
            if (Token == null) return true;

            if (!char.IsDigit(Token.Value[1])) return false;


            int registerNumber = (int)char.GetNumericValue(Token.Value[1]);

            if (registerNumber > 7 || registerNumber < 1)
                return false;

            return true;
        }

        public override string ToString()
        {
            return Token?.Value;
        }
    }
}
