﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Microprocessor.InstructionFormats
{
    public interface IMCInstruction
    {
        byte OpCode { get; }
    }
}
