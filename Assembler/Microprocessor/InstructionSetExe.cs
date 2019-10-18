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
                    //F2; LOAD Ra, address
                  
                    MCInstructionF2 instructionF2 = (MCInstructionF2) instruction;
                    string microAddress = instructionF2.AddressParamHex;
                    string valueInMicro = micro.MicroVirtualMemory.GetContentsInHex(microAddress);
                    micro.MicroRegisters.SetRegisterValue(instructionF2.Ra, valueInMicro);

                    return true;
                }},
                { "00001",      (IMCInstruction instruction, MicroSimulator micro) => {
                    //F2; LOADIM Ra, const
                    MCInstructionF2 instructionF2 = (MCInstructionF2) instruction;

                    byte registerA = instructionF2.Ra;

                    micro.MicroRegisters.SetRegisterValue(registerA, instructionF2.AddressParamHex);

                    return true;
                }},
                { "00010",     (IMCInstruction instruction, MicroSimulator micro) => {
                   //TODO: implement POP
                    return true; }},
                { "00011",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // STORE mem, Ra  {F2} [mem] <- R[Ra]
                    MCInstructionF2 instructionF2 = (MCInstructionF2) instruction;

                    byte registerA = instructionF2.Ra;

                    string registerAValue = micro.MicroRegisters.GetRegisterValue(registerA);

                    micro.MicroVirtualMemory.SetContentInMemory(instructionF2.AddressParamHex, registerAValue);
                    return true;
                }},
                { "00100",     (IMCInstruction instruction, MicroSimulator micro) => {
                    //TODO: implement PUSH
                    return true; }},
                { "00101",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // LOADRIND Ra,Rb  {F1} R[Ra] <- mem[R[Rb]]
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte registerA = instructionF1.Ra;
                    byte registerB = instructionF1.Rb;

                    string valueRegisterB = micro.MicroRegisters.GetRegisterValue(registerB);

                    string memoryData = micro.MicroVirtualMemory.GetContentsInHex(valueRegisterB);

                    micro.MicroRegisters.SetRegisterValue(registerA, memoryData);
                    return true;
                }},
                { "00110",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // STORERIND Ra,Rb  {F1} R[Rb] <- mem[R[Ra]]
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte registerA = instructionF1.Ra;
                    byte registerB = instructionF1.Rb;

                    string valueRegisterA = micro.MicroRegisters.GetRegisterValue(registerA);

                    string valueInMemory  = micro.MicroVirtualMemory.GetContentsInHex(valueRegisterA);

                    micro.MicroRegisters.SetRegisterValue(registerB, valueInMemory);

                    return true;
                }},
                // Arithmetic Operations
                { "00111",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // ADD Ra, Rb, Rc {F1} R[Ra]<- R[Rb]+R[Rc] 
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte rb = instructionF1.Rb;
                    byte rc = instructionF1.Rc;

                    string result = ArithmeticOperations.HexAdd(
                        micro.MicroRegisters.GetRegisterValue(rb),
                        micro.MicroRegisters.GetRegisterValue(rc)
                        );

                    micro.MicroRegisters.SetRegisterValue(instructionF1.Ra, result);

                    return true;
                }},
                { "01000",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // SUB Ra, Rb, Rc {F1} R[Ra]<- R[Rb]-R[Rc]
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte rb = instructionF1.Rb;
                    byte rc = instructionF1.Rc;

                    string result = ArithmeticOperations.HexSubstract(
                        micro.MicroRegisters.GetRegisterValue(rb),
                        micro.MicroRegisters.GetRegisterValue(rc)
                        );

                    micro.MicroRegisters.SetRegisterValue(instructionF1.Ra, result);

                    return true;
                }},
                { "01001",     (IMCInstruction instruction, MicroSimulator micro) => {
                   // ADDIM Ra, cons {F2} R[Ra] <- R[Ra]+cons

                    MCInstructionF2 instructionF2 = (MCInstructionF2) instruction;

                    byte ra = instructionF2.Ra;

                    string result = ArithmeticOperations.HexAdd(
                        micro.MicroRegisters.GetRegisterValue(ra),
                        instructionF2.AddressParamHex
                        );

                    micro.MicroRegisters.SetRegisterValue(ra, result);
                    
                    return true;
                }},
                { "01010",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // SUBIM Ra,  cons {F2} R[Ra]<- R[Ra]-cons
                    MCInstructionF2 instructionF2 = (MCInstructionF2) instruction;

                    byte ra = instructionF2.Ra;

                    string result = ArithmeticOperations.HexSubstract(
                        micro.MicroRegisters.GetRegisterValue(ra),
                        instructionF2.AddressParamHex
                        );

                    micro.MicroRegisters.SetRegisterValue(ra, result);

                    return true;
                }},
                //Logic operations
                { "01011",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // AND Ra, Rb, Rc {F1} R[Ra]<- R[Rb]*R[Rc]
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte ra = instructionF1.Ra;
                    byte rb = instructionF1.Rb;
                    byte rc = instructionF1.Rc;

                    sbyte valueInB = UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rb));
                    sbyte valueInC = UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rc));

                    sbyte resultForA = (sbyte) (valueInB & valueInC);

                    micro.MicroRegisters.SetRegisterValue(ra, UnitConverter.IntToHex(resultForA));

                    return true; 
                }},
                { "01100",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // OR Ra, Rb, Rc {F1} R[Ra]<- R[Rb]+R[Rc] 
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte ra = instructionF1.Ra;
                    byte rb = instructionF1.Rb;
                    byte rc = instructionF1.Rc;

                    sbyte valueInB = UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rb));
                    sbyte valueInC = UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rc));

                    sbyte resultForA = (sbyte) (valueInB | valueInC);

                    micro.MicroRegisters.SetRegisterValue(ra, UnitConverter.ByteToHex(resultForA));

                    return true; 
                }},
                { "01101",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // XOR Ra, Rb, Rc {F1} R[Ra]<- R[Rb] XOR [Rc] 
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte ra = instructionF1.Ra;
                    byte rb = instructionF1.Rb;
                    byte rc = instructionF1.Rc;

                    sbyte valueInB = UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rb));
                    sbyte valueInC = UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rc));

                    sbyte resultForA = (sbyte) (valueInB ^ valueInC);

                    micro.MicroRegisters.SetRegisterValue(ra, UnitConverter.ByteToHex(resultForA));

                    return true; 
                }},
                { "01110",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // NOT Ra,Rb {F1}   R[Ra] <- Complement(R[Rb])
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte ra = instructionF1.Ra;
                    byte rb = instructionF1.Rb;

                    sbyte valueInB = UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rb));

                    sbyte resultForA = (sbyte) (~valueInB);
                    
                    micro.MicroRegisters.SetRegisterValue(ra, UnitConverter.ByteToHex(resultForA));

                    return true; 
                }},
                { "01111",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // NEG Ra,Rb {F1} R[Ra]<- - R[Rb]
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte ra = instructionF1.Ra;
                    byte rb = instructionF1.Rb;

                    sbyte valueInB = UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rb));

                    sbyte resultForA = (sbyte) (-valueInB);

                    micro.MicroRegisters.SetRegisterValue(ra, UnitConverter.ByteToHex(resultForA));

                    return true; 
                }},
                { "10000",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // SHIFTR Ra, Rb, Rc {F1} R[Ra]<- R[Rb] shr R[Rc]
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte ra = instructionF1.Ra;
                    byte rb = instructionF1.Rb;
                    byte rc = instructionF1.Rc;

                    byte valueInB = (byte) UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rb));
                    byte valueInC = (byte) UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rc));

                    // bitwise shift right operator
                    byte resultForA = (byte) (valueInB >> valueInC);

                    micro.MicroRegisters.SetRegisterValue(ra, UnitConverter.ByteToHex(resultForA));

                    return true; 
                }},
                { "10001",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // SHIFTL Ra, Rb, Rc {F1} R[Ra]<- R[Rb] shl R[Rc] 
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte ra = instructionF1.Ra;
                    byte rb = instructionF1.Rb;
                    byte rc = instructionF1.Rc;

                    byte valueInB = (byte)UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rb));
                    byte valueInC = (byte)UnitConverter.HexToSByte(micro.MicroRegisters.GetRegisterValue(rc));

                    // bitwise shift left operator
                    byte resultForA = (byte) (valueInB << valueInC);

                    micro.MicroRegisters.SetRegisterValue(ra, UnitConverter.ByteToHex(resultForA));

                    return true; 
                }},
                { "10010",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // ROTAR Ra, Rb, Rc {F1} R[Ra]<- R[Rb] rtr R[Rc]
                    
                    // call SHIFTR
                    operatorFunctions["10000"](instruction, micro);

                    return true; 
                }},
                { "10011",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // ROTAL Ra, Rb, Rc {F1} R[Ra]<- R[Rb] rtr R[Rc]
                    
                    // call SHIFTL
                    operatorFunctions["10001"](instruction, micro);

                    return true; 
                }},
                // Flow Control
                { "10100",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // JMPRIND Ra {F1} [pc] <- [R[ra]] 

                    byte ra = ((MCInstructionF1)instruction).Ra;

                    string hexaAddress = micro.MicroRegisters.GetRegisterValue(ra);

                    micro.ProgramCounter = (ushort) UnitConverter.HexToInt(hexaAddress); 
                    
                    return true; 
                }},
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
            string opCode = UnitConverter.IntToBinary(instruction.OpCode, defaultWidth: 5);

            if (!operatorFunctions.ContainsKey(opCode))
                return false;
            return operatorFunctions[opCode](instruction, microSimulator);
        }
    }
}
