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
                { "10110",  new byte[] { 3, 1 }},
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

        /// <summary>
        /// Identifies if binary opcode is of any type of JMP
        /// </summary>
        /// <param name="binaryOpcode">5-bit OpCode string</param>
        /// <returns>True if string is any JMP instruction, false otherwise</returns>
        public static bool IsJump(string binaryOpcode)
        {
            return binaryOpcode == "10100" || binaryOpcode == "10101";
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
    }
}
