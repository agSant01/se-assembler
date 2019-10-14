using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using System;
using System.Collections.Generic;
using System.Text;


namespace Assembler.Microprocessor
{
    public static class InstructionSetExe
    {
        private static readonly Dictionary<string, Func<IMCInstruction, MicroSimulator, bool>>
           operatorFunctions = new Dictionary<string, Func<IMCInstruction, MicroSimulator, bool>>
           {
                // Data movement
                // { OP_CODE, INSTRUCTION_FORMAT, NUM_OF_PARAMS }
                { "00000",     (IMCInstruction instruction, MicroSimulator micro) => {
                    //LOAD F2
                    string value  = micro.MicroVirtualMemory.GetContentsInHex(((MCInstructionF2)instruction).AddressParamHex);
                    micro.MicroRegisters.SetRegisterValue(((MCInstructionF2)instruction).Ra,value);
                    return true; }},


                { "00001",      (IMCInstruction instruction, MicroSimulator micro) => {
                    //LOADIM F2
                    micro.MicroRegisters.SetRegisterValue(((MCInstructionF2)instruction).Ra,((MCInstructionF2)instruction).AddressParamHex);
                    return true; }},

                { "00010",     (IMCInstruction instruction, MicroSimulator micro) => {
                    //TODO: implement POP
                    return true; }},

                { "00011",     (IMCInstruction instruction, MicroSimulator micro) => {
                    //STORE F2
                    micro.MicroVirtualMemory.SetContentInMemory(((MCInstructionF2)instruction).AddressParamHex,Convert.ToString(((MCInstructionF2)instruction).Ra,16));
                    return true; }},

                { "00100",     (IMCInstruction instruction, MicroSimulator micro) => {
                    //TODO: implement PUSH
                    return true; }},

                { "00101",     (IMCInstruction instruction, MicroSimulator micro) => {
                    //LOADIND
                    string value  = micro.MicroVirtualMemory.GetContentsInHex(micro.MicroRegisters.GetRegisterValue(((MCInstructionF1)instruction).Rb));
                    micro.MicroRegisters.SetRegisterValue(((MCInstructionF1)instruction).Ra,value);
                    return true; }},

                { "00110",     (IMCInstruction instruction, MicroSimulator micro) => {
                    //STORERIND
                    string value  = micro.MicroVirtualMemory.GetContentsInHex(micro.MicroRegisters.GetRegisterValue(((MCInstructionF1)instruction).Ra));
                    micro.MicroRegisters.SetRegisterValue(((MCInstructionF1)instruction).Rb,value);
                    return true; }},


                // Arithmetic Operations
                { "00111",     (IMCInstruction instruction, MicroSimulator micro) => {
                    //ADD
                    int element1 = Convert.ToInt32(micro.MicroRegisters.GetRegisterValue(((MCInstructionF1)instruction).Rb),16);
                    int element2 = Convert.ToInt32(micro.MicroRegisters.GetRegisterValue(((MCInstructionF1)instruction).Rc),16);
                    micro.MicroRegisters.SetRegisterValue(((MCInstructionF1)instruction).Ra, (element1 + element2).ToString());
                    return true; }},
                { "01000",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "01001",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "01010",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                //Logic operations
                { "01011",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "01100",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "01101",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "01110",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "01111",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "10000",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "10001",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "10010",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "10011",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                // Flow Control
                { "10100",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "10101",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "10110",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "10111",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "11000",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "11001",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "11010",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "11011",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "11100",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "11101",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "11110",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }},
                { "11111",     (IMCInstruction instruction, MicroSimulator micro) => { return true; }}
           };

        public static bool ExecuteInstruction(IMCInstruction instruction, MicroSimulator microSimulator)
        {
            string opCode = UnitConverter.DecimalToBinary(instruction.OpCode, defaultWidth: 5);

            if (!operatorFunctions.ContainsKey(opCode))
                return false;
            return operatorFunctions[opCode](instruction, microSimulator);
        }
    }
}
