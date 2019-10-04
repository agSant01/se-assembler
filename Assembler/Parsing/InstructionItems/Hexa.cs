using Assembler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Assembler.Parsing.InstructionItems
{
    public class Hexa : IFormatUnit
    {

        private bool _isValid;

        public Hexa(Token token)
        {
            Token = token;

            _isValid = new Regex(@"^[a-fA-F0-9]*$").IsMatch(token.Value);
        }

        public static Hexa[] ToArray(Token[] values)
        {
            Hexa[] hexas = new Hexa[values.Length];

            int idx = 0;
            foreach (Token t in values)
            {
                hexas[idx] = new Hexa(t);
            }

            return hexas;
        }

        public Token Token { get; }

        public bool IsValid() => _isValid;

        public override string ToString()
        {
            return Token?.Value;
        }
    }
}
