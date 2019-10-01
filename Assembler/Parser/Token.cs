using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parser
{
    public class Token
    {
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Value { get; }

        public TokenType Type { get; }

        public override string ToString()
        {
            return $"Token(type: {Type.ToString()}, val: '{Value}')";
        }

        public override bool Equals(object obj)
        {
            if (!this.GetType().Equals(obj.GetType()))
                return false;

            Token tok_obj = (Token)obj;

            return this.Type.Equals(tok_obj.Type)
                && this.Value.Equals(tok_obj.Value);
        }
    }
}
