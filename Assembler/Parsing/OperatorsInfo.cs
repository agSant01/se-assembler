using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing
{
    public class OperatorsInfo
    {
        private static Dictionary<string, int[]> operatorInfo;

        private static void Init()
        {

            operatorInfo = new Dictionary<string, int[]>
            {

                // Data movement
                // { OP_CODE, INSTRUCTION_FORMAT, NUM_OF_PARAMS }
                { "LOAD",       new int[] { 0, 2, 2 } },
                { "LOADIM",     new int[] { 1, 2, 2 } },
                { "POP",        new int[] { 2, 2, 1 } },
                { "STORE",      new int[] { 3, 2, 2 } },
                { "PUSH",       new int[] { 4, 2, 1 } },
                { "LOADRIND",   new int[] { 5, 1, 2 } },
                { "STOREIND",   new int[] { 6, 1, 2 } },

                // Arithmetic Operations
                { "ADD",        new int[] { 7, 1, 3 } },
                { "SUB",        new int[] { 8, 1, 3 } },
                { "ADDIM",      new int[] { 9, 2, 2 } },
                { "SUBIM",      new int[] { 10, 2, 2 } },

                //Logic operations
                { "AND",        new int[] { 11, 1, 3 } },
                { "OR",         new int[] { 12, 1, 3 } },
                { "XOR",        new int[] { 13, 1, 3 } },
                { "NOT",        new int[] { 14, 1, 2 } },
                { "NEG",        new int[] { 15, 1, 2 } },
                { "SHIFTR",     new int[] { 16, 1, 3 } },
                { "SHIFTL",     new int[] { 17, 1, 3 } },
                { "ROTAR",      new int[] { 18, 1, 3 } },
                { "ROTAL",      new int[] { 19, 1, 3 } },

                // Flow Control
                { "JMPRIND",        new int[] { 20, 1, 1 } },
                { "JMPADDR",    new int[] { 21, 3, 1 } },
                { "JCONDRIN",   new int[] { 22, 3, 1 } },
                { "JCONDADDR",  new int[] { 23, 3, 1 } },
                { "LOOP",       new int[] { 24, 2, 2 } },
                { "GRT",        new int[] { 25, 1, 2 } },
                { "GRTEQ",      new int[] { 26, 1, 2 } },
                { "EQ",         new int[] { 27, 1, 2 } },
                { "NEQ",        new int[] { 28, 1, 2 } },
                { "NOP",        new int[] { 29, 1, 1 } },
                { "CALL",       new int[] { 30, 3, 1 } },
                { "RETURN",     new int[] { 31, -1, 0 } }
            };
        }

        public static bool IsOperator(string value)
        {
            if (operatorInfo == null) Init();

            return operatorInfo.ContainsKey(value.ToUpper());
        } 

        public static EInstructionFormat GetInstructionFormat(Token token)
        {
            string opcode = token.Value;

            if (operatorInfo == null) Init();

            // access second item in the array which is the format
            if (!IsOperator(opcode))
                return EInstructionFormat.INVALID;

            int formatNumer = operatorInfo[opcode.ToUpper()][1];

            switch (formatNumer)
            {
                case 1:
                    return EInstructionFormat.FORMAT_1;
                case 2:
                    return EInstructionFormat.FORMAT_2;
                case 3:
                    return EInstructionFormat.FORMAT_3;
                default:
                    return EInstructionFormat.INVALID;
            }
        }

        public static int GetOPCode(Token token)
        {
            if (operatorInfo == null) Init();


            string opcode = token.Value;
            return operatorInfo[opcode.ToUpper()][0];
        }

        public static int GetNumberOfParams(Token token)
        {
            if (operatorInfo == null) Init();

            string opcode = token.Value;
            return operatorInfo[opcode.ToUpper()][2];
        }
    }
}
