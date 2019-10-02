using Assembler.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Interfaces
{
    public interface IFormatInstructions
    {
        Token Operator { get; }

        bool IsValid { get; }
    }
}
