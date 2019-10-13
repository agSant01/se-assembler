using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Microprocessor
{
    class InstructionSetExe
    {
        private static readonly Dictionary<string, int[]>
           operatorInfo = new Dictionary<string, int[]>
           {
                // Data movement
                // { OP_CODE, INSTRUCTION_FORMAT, NUM_OF_PARAMS }
                { "00000",       new int[] { 0, 2, 2 } },
                { "00001",     new int[] { 1, 2, 2 } },
                { "00010",        new int[] { 2, 2, 1 } },
                { "00011",      new int[] { 3, 2, 2 } },
                { "00100",       new int[] { 4, 2, 1 } },
                { "00101",   new int[] { 5, 1, 2 } },
                { "00110",   new int[] { 6, 1, 2 } },
                // Arithmetic Operations
                { "00111",        new int[] { 7, 1, 3 } },
                { "01000",        new int[] { 8, 1, 3 } },
                { "01001",      new int[] { 9, 2, 2 } },
                { "01010",      new int[] { 10, 2, 2 } },
                //Logic operations
                { "01011",        new int[] { 11, 1, 3 } },
                { "01100",         new int[] { 12, 1, 3 } },
                { "01101",        new int[] { 13, 1, 3 } },
                { "01110",        new int[] { 14, 1, 2 } },
                { "01111",        new int[] { 15, 1, 2 } },
                { "10000",     new int[] { 16, 1, 3 } },
                { "10001",     new int[] { 17, 1, 3 } },
                { "10010",      new int[] { 18, 1, 3 } },
                { "10011",      new int[] { 19, 1, 3 } },
                // Flow Control
                { "10100",        new int[] { 20, 1, 1 } },
                { "10101",    new int[] { 21, 3, 1 } },
                { "10110",   new int[] { 22, 3, 1 } },
                { "10111",  new int[] { 23, 3, 1 } },
                { "11000",       new int[] { 24, 2, 2 } },
                { "11001",        new int[] { 25, 1, 2 } },
                { "11010",      new int[] { 26, 1, 2 } },
                { "11011",         new int[] { 27, 1, 2 } },
                { "11100",        new int[] { 28, 1, 2 } },
                { "11101",        new int[] { 29, 1, 1 } },
                { "11110",       new int[] { 30, 3, 1 } },
                { "11111",     new int[] { 31, -1, 0 } }
           };
    }
}
