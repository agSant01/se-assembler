using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionFormats
{
    public class Label : IFormatInstructions
    {
        public Label(Token name)
        {
            Name = new VariableName(name);
        }

        public VariableName Name { get; }

        public Token Operator => null;

        public bool IsValid => Name.IsValid();

        public override string ToString()
        {
            return $"Label[name: {Name}, valid: '{IsValid}']";
        }
    }
}
