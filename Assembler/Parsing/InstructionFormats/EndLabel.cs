using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionFormats
{
    public class EndLabel : IFormatInstructions
    {
        public EndLabel(Token name)
        {
            Name = new VariableName(name);
        }

        public VariableName Name { get; }

        public Token Operator => null;

        public bool IsValid => Name.IsValid();

        public override string ToString()
        {
            return $"EndLabel[name: {Name}, valid:'{IsValid}']";
        }
    }
}
