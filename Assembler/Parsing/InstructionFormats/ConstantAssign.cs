using Assembler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionItems
{
    public class ConstantAssign : IFormatInstructions
    {
        public ConstantAssign(Token op, Token name, Token value)
        {
            Operator = op;
            Name = new VariableName(name);
            Value = new Hexa(value);
        }

        public Token Operator { get; }

        public VariableName Name { get; }

        public Hexa Value { get; }

        public bool IsValid => Name.IsValid() && Value.IsValid();

        public override string ToString()
        {
            return $"ConstantAssign[op:{Operator.Value}, name: {Name}, value: {Value}, valid: '{IsValid}']";
        }
    }
}
