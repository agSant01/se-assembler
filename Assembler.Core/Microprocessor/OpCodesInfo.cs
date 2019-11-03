using System.Collections.Generic;

namespace Assembler.Microprocessor
{
    /// <summary>
    /// Information of the Operator Codes.
    /// </summary>
    public class OpCodesInfo
    {
        /// <summary>
        /// Dictionary of Operator Codes Information
        /// </summary>
        private static readonly Dictionary<string, byte[]>
            operatorInfo = new Dictionary<string, byte[]>
            {
                // Data movement
                // { INSTRUCTION_FORMAT, NUM_OF_PARAMS }
                { "00000",  new byte[] { 2, 2 }},
                { "00001",  new byte[] { 2, 2 }},
                { "00010",  new byte[] { 2, 1 }},
                { "00011",  new byte[] { 2, 2 }},
                { "00100",  new byte[] { 2, 1 }},
                { "00101",  new byte[] { 1, 2 }},
                { "00110",  new byte[] { 1, 2 }},
                // Arithmetic Operations
                { "00111",  new byte[] { 1, 3 }},
                { "01000",  new byte[] { 1, 3 }},
                { "01001",  new byte[] { 2, 2 }},
                { "01010",  new byte[] { 2, 2 }},
                //Logic operations
                { "01011",  new byte[] { 1, 3 }},
                { "01100",  new byte[] { 1, 3 }},
                { "01101",  new byte[] { 1, 3 }},
                { "01110",  new byte[] { 1, 2 }},
                { "01111",  new byte[] { 1, 2 }},
                { "10000",  new byte[] { 1, 3 }},
                { "10001",  new byte[] { 1, 3 }},
                { "10010",  new byte[] { 1, 3 }},
                { "10011",  new byte[] { 1, 3 }},
                // Flow Control
                { "10100",  new byte[] { 1, 1 }},     // JMPRIND
                { "10101",  new byte[] { 3, 1 }},     // JMPADDR 
                { "10110",  new byte[] { 1, 1 }},
                { "10111",  new byte[] { 3, 1 }},
                { "11000",  new byte[] { 2, 2 }},
                { "11001",  new byte[] { 1, 2 }},
                { "11010",  new byte[] { 1, 2 }},
                { "11011",  new byte[] { 1, 2 }},
                { "11100",  new byte[] { 1, 2 }},
                { "11101",  new byte[] { 1, 1 }},
                { "11110",  new byte[] { 3, 1 }},
                { "11111",  new byte[] { 0, 0 }}
            };

        /// Dictionary of Operator Codes Names
        /// </summary>
        private static readonly Dictionary<string, string>
            operatorNames = new Dictionary<string, string>
            {
                // Data movement
                // { INSTRUCTION_FORMAT, NUM_OF_PARAMS }
                { "00000",  "LOAD"    },    
                { "00001",  "LOADIM"  },
                { "00010",  "POP"     },
                { "00011",  "STORE"   },
                { "00100",  "PUSH"    },
                { "00101",  "LOADRIND"},
                { "00110",  "STOREIND"},
                // Arithmetic Operations
                { "00111",  "ADD"     },
                { "01000",  "SUB"     },
                { "01001",  "ADDIM"   },
                { "01010",  "SUBIM"   },
                //Logic operations
                { "01011",  "AND"     },
                { "01100",  "OR"      },
                { "01101",  "XOR"     },
                { "01110",  "NOT"     },
                { "01111",  "NEG"     },
                { "10000",  "SHIFTR"  },
                { "10001",  "SHIFTL"  },
                { "10010",  "ROTAR"   },
                { "10011",  "ROTAL"   },
                // Flow Control
                { "10100", "JMPRIND"  },     // JMPRIND
                { "10101", "JMPADDR"  },     // JMPADDR 
                { "10110", "JCONDRIN" },
                { "10111", "JCONDADDR"},
                { "11000", "LOOP"     },
                { "11001", "GRT"      },
                { "11010", "GRTEQ"    },
                { "11011", "EQ"       },
                { "11100", "NEQ"      },
                { "11101", "NOP"      },
                { "11110", "CALL"     },
                { "11111", "RETURN"   }
            };

        /// <summary>
        /// Identifies if binary opcode is of any type of JMP
        /// </summary>
        /// <param name="binaryOpcode">5-bit OpCode string</param>
        /// <returns>True if string is any JMP instruction, false otherwise</returns>
        public static bool IsJump(string binaryOpcode)
        {
            return binaryOpcode == "10100" || binaryOpcode == "10101" || binaryOpcode == "10110" || binaryOpcode == "10111";
        }

        /// <summary>
        /// Used to identify the instruction format of the target operator
        /// </summary>
        /// <param name="binaryCode">5-bit OpCode string</param>
        /// <returns>Byte, representing the Instruction Format of the operator code</returns>
        public static byte GetInstructionFormat(string binaryCode)
        {
            return operatorInfo[binaryCode][0];
        }

        /// <summary>
        /// Identifies the number of parameters an operator requires
        /// </summary>
        /// <param name="binaryCode">5-bit OpCode string</param>
        /// <returns>The number of required parameters</returns>
        public static byte GetNumberOfParams(string binaryCode)
        {
            return operatorInfo[binaryCode][1];
        }

        /// <summary>
        /// Get Operator code name
        /// </summary>
        /// <param name="binaryCode">binary code of operator</param>
        /// <returns>Text representaion of operator</returns>
        public static string GetOpName(string binaryCode)
        {
            return operatorNames[binaryCode];
        }
    }
}
