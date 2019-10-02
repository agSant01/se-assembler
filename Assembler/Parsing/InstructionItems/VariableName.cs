using Assembler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Assembler.Parsing.InstructionItems
{
    public class VariableName : IFormatUnit
    {
        private bool _isValid;

        public VariableName(Token token)
        {
            Token = token;

            _isValid = new Regex(@"^[a-zA-Z0-9#]*$").IsMatch(token.Value);
        }

        public Token Token { get; }

        public bool IsValid() => _isValid;

        public override string ToString()
        {
            return Token?.Value;
        }
    }
}
