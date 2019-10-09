using Assembler.Microprocessor.Interfaces;
using Assembler.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Microprocessor
{
    public class Disassembler : ICustomIterable<string>
    {
        public Disassembler(VirtualMemory virtualMemory)
        {

        }

        public void AddTest(string i) {
            Add(i);
        } 
    }
}
