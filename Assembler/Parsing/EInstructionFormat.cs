using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing
{
    public enum EInstructionFormat
    {
        /// <summary>
        /// Instruction Format in Memory
        /// 16-bit Register [15-11 OPCODE | 10-8  Ra | 7-5  Rb | 4-2  Rc | 1-0 null ]
        /// </summary>
        FORMAT_1,

        /// <summary>
        /// Instruction Format in Memory
        /// 16-bit Register [15-11 OPCODE | 10-8  Ra | 7-0 const/address ]
        /// </summary>
        FORMAT_2,

        /// <summary>
        /// 16-bit Register [15-11 OPCODE | 10-0 const/address ]
        /// </summary>
        FORMAT_3,

        /// <summary>
        /// Not a valid Instruction Format
        /// </summary>
        INVALID
    }
}
