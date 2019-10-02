using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parser
{
    public class OperatorsInfo
    {
        private static Dictionary<string, int[]> operatorInfo;

        private static void Init()
        {

            operatorInfo = new Dictionary<string, int[]>();

            // Data movement
            // { OP_CODE, INSTRUCTION_FORMAT }
            operatorInfo.Add("LOAD",        new int[] { 0, 2 });
            operatorInfo.Add("LOADIM",      new int[] { 1, 2 });
            operatorInfo.Add("POP",         new int[] { 2, 2 });
            operatorInfo.Add("STORE",       new int[] { 3, 2 });
            operatorInfo.Add("PUSH",        new int[] { 4, 2 });
            operatorInfo.Add("LOADRIND",    new int[] { 5, 1 });
            operatorInfo.Add("STOREIND",    new int[] { 6, 1 });

            // Arithmetic Operations
            operatorInfo.Add("ADD",         new int[] { 7, 1 });
            operatorInfo.Add("SUB",         new int[] { 8, 1 });
            operatorInfo.Add("ADDIM",       new int[] { 9, 2 });
            operatorInfo.Add("SUBIM",       new int[] { 10, 2 });

            //Logic operations
            operatorInfo.Add("AND",         new int[] { 11, 1 });
            operatorInfo.Add("OR",          new int[] { 12, 1 });
            operatorInfo.Add("XOR",         new int[] { 13, 1 });
            operatorInfo.Add("NOT",         new int[] { 14, 1 });
            operatorInfo.Add("NEG",         new int[] { 15, 1 });
            operatorInfo.Add("SHIFTR",      new int[] { 16, 1 });
            operatorInfo.Add("SHIFTL",      new int[] { 17, 1 });
            operatorInfo.Add("ROTAR",       new int[] { 18, 1 });
            operatorInfo.Add("ROTAL",       new int[] { 19, 1 });

            // Flow Control
            operatorInfo.Add("JMP",         new int[] { 20, 1 });
            operatorInfo.Add("JMPADDR",     new int[] { 21, 3 });
            operatorInfo.Add("JCONDRIN",    new int[] { 22, 3 });
            operatorInfo.Add("JCONDADDR",   new int[] { 23, 3 });
            operatorInfo.Add("LOOP",        new int[] { 24, 2 });
            operatorInfo.Add("GRT",         new int[] { 25, 1 });
            operatorInfo.Add("GRTEQ",       new int[] { 26, 1 });
            operatorInfo.Add("EQ",          new int[] { 27, 1 });
            operatorInfo.Add("NEQ",         new int[] { 28, 1 });
            operatorInfo.Add("NOP",         new int[] { 29, 1 });
            operatorInfo.Add("CALL",        new int[] { 30, 3 });
            operatorInfo.Add("RETURN",      new int[] { 31, -1 });
        }

        public static bool IsOperator(string value)
        {
            if (operatorInfo == null) Init();

            return operatorInfo.ContainsKey(value.ToUpper());
        } 

        public static EInstructionFormat GetInstructionFormat(string operatorCode)
        {
            if (operatorInfo == null) Init();

            // access second item in the array which is the format
            if (!IsOperator(operatorCode))
                return EInstructionFormat.INVALID;

            int formatNumer = operatorInfo[operatorCode.ToUpper()][1];

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

        public static int GetOPCode(string operatorCode)
        {
            if (operatorInfo == null) Init();

            return operatorInfo[operatorCode.ToUpper()][0];
        }
    }
}
