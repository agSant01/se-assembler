using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using System;
using System.Collections.Generic;


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
                    
                    string valueInMicro = micro.ReadFromMemory(UnitConverter.HexToInt(microAddress));
                    
                    micro.MicroRegisters.SetRegisterValue(instructionF2.Ra, valueInMicro);

                    if (instructionF2.Ra == 7)
                    {
                        MCInstructionF2 mCInstructionF2 = new MCInstructionF2(0,"00100","R7", "0000000");

                        operatorFunctions["00100"](mCInstructionF2, micro);
                    }

                    return true;
                }},
                { "00001",      (IMCInstruction instruction, MicroSimulator micro) => {
                    //F2; LOADIM Ra, const
                    MCInstructionF2 instructionF2 = (MCInstructionF2) instruction;

                    micro.MicroRegisters.SetRegisterValue(instructionF2.Ra, instructionF2.AddressParamHex);

                    if (instructionF2.Ra == 7)
                    {
                        MCInstructionF2 mCInstructionF2 = new MCInstructionF2(0,"00100","R7", "0000000");

                        operatorFunctions["00100"](mCInstructionF2, micro);
                    }

                    return true;
                }},
                { "00010",     (IMCInstruction instruction, MicroSimulator micro) => {
                   //F2; POP Ra
                    MCInstructionF2 instructionF2 = (MCInstructionF2) instruction;
                    micro.MicroRegisters.SetRegisterValue(instructionF2.Ra, micro.ReadFromMemory(micro.StackPointer));
                    micro.StackPointer++;
                    return true; 
                }},
                { "00011",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // STORE mem, Ra  {F2} [mem] <- R[Ra]
                    MCInstructionF2 instructionF2 = (MCInstructionF2) instruction;

                    string registerAValue = micro.MicroRegisters.GetRegisterValue(instructionF2.Ra);

                    micro.WriteToMemory(
                        UnitConverter.HexToInt(instructionF2.AddressParamHex),
                        registerAValue
                    );
                    return true;
                }},
                { "00100",     (IMCInstruction instruction, MicroSimulator micro) => {
                    //F2; PUSH Ra
                    MCInstructionF2 instructionF2 = (MCInstructionF2) instruction;

                    micro.StackPointer--;
                    
                    micro.WriteToMemory(
                        micro.StackPointer,
                        micro.MicroRegisters.GetRegisterValue(instructionF2.Ra)
                        );
                    
                    return true; 
                }},
                { "00101",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // LOADRIND Ra,Rb  {F1} R[Ra] <- mem[R[Rb]]
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    string valueRegisterBHex = micro.MicroRegisters.GetRegisterValue(instructionF1.Rb);

                    string memoryData = micro.ReadFromMemory(UnitConverter.HexToInt(valueRegisterBHex));

                    micro.MicroRegisters.SetRegisterValue( instructionF1.Ra, memoryData);

                    if (instructionF1.Ra == 7)
                    {
                        MCInstructionF2 mCInstructionF2 = new MCInstructionF2(0,"00100","R7", "0000000");

                        operatorFunctions["00100"](mCInstructionF2, micro);
                    }

                    return true;
                }},
                { "00110",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // STORERIND Ra,Rb  {F1} mem[R[Ra]] <- R[Rb]
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    string registerAValue = micro.MicroRegisters.GetRegisterValue(instructionF1.Ra);

                    string registerBValue = micro.MicroRegisters.GetRegisterValue(instructionF1.Rb);

                    micro.WriteToMemory(
                           UnitConverter.HexToInt(registerAValue),
                           registerBValue
                    );

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

                    string resultHex = ArithmeticOperations.HexSubstract(
                        micro.MicroRegisters.GetRegisterValue(ra),
                        instructionF2.AddressParamHex
                        );

                    micro.MicroRegisters.SetRegisterValue(ra, resultHex);

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

                    micro.MicroRegisters.SetRegisterValue(ra, UnitConverter.ByteToHex(resultForA));

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
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte ra = instructionF1.Ra;
                    byte rb = instructionF1.Rb;
                    byte rc = instructionF1.Rc;

                    byte timesToRotate = UnitConverter.HexToByte(micro.MicroRegisters.GetRegisterValue(rc));

                    char[] bitArray = UnitConverter.HexToBinary(micro.MicroRegisters.GetRegisterValue(rb)).ToCharArray();
                    char[] newRotatedBitArr = new char[bitArray.Length];

                    byte currentReadingPosition = (byte) (bitArray.Length - timesToRotate);
                    
                    for (byte writingPositon = 0; writingPositon < bitArray.Length; writingPositon++)
                    {
                        newRotatedBitArr[writingPositon] = bitArray[currentReadingPosition % bitArray.Length];
                        ++currentReadingPosition;
                    }

                    string newDataInBinary = string.Join("", newRotatedBitArr);

                    string newDataInHex = UnitConverter.BinaryToHex(newDataInBinary);

                    micro.MicroRegisters.SetRegisterValue(ra, newDataInHex);

                    return true;
                }},
                { "10011",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // ROTAL Ra, Rb, Rc {F1} R[Ra]<- R[Rb] rtr R[Rc]
                    
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    byte ra = instructionF1.Ra;
                    byte rb = instructionF1.Rb;
                    byte rc = instructionF1.Rc;

                    byte timesToRotate = UnitConverter.HexToByte(micro.MicroRegisters.GetRegisterValue(rc));


                    char[] bitArray = UnitConverter.HexToBinary(micro.MicroRegisters.GetRegisterValue(rb)).ToCharArray();
                    char[] newRotatedBitArr = new char[bitArray.Length];

                    byte currentReadingPosition = timesToRotate;

                    for (byte writingPositon = 0; writingPositon < bitArray.Length; writingPositon++)
                    {
                        newRotatedBitArr[writingPositon] = bitArray[currentReadingPosition % bitArray.Length];
                        ++currentReadingPosition;
                    }

                    string newDataInBinary = string.Join("", newRotatedBitArr);

                    string newDataInHex = UnitConverter.BinaryToHex(newDataInBinary);

                    micro.MicroRegisters.SetRegisterValue(ra, newDataInHex);

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
                { "10101",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // JMPADDR addr {F3} [pc] <- address
                    MCInstructionF3 instructionF3 = (MCInstructionF3) instruction;

                    string address = instructionF3.AddressParamHex;

                    ushort addressShort = (ushort) UnitConverter.HexToInt(address);

                    micro.ProgramCounter = addressShort;

                    return true;
                }},
                { "10110",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    //JCONDRIN Ra {F1} If cond then [pc] <- [R[ra]] 

                    if (micro.ConditionalBit)
                    {
                        MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                        byte ra = instructionF1.Ra;

                        string hexaAddress = micro.MicroRegisters.GetRegisterValue(ra);

                        micro.ProgramCounter = (ushort) UnitConverter.HexToInt(hexaAddress);
                    } else
                    {
                        micro.ProgramCounter += 2;
                    }

                    micro.ConditionalBit = false;

                    return true;
                }},
                { "10111",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // JCONDADDR addr {F3} If cond then [pc] <- address
                    if (micro.ConditionalBit)
                    {
                        string addressHex = ((MCInstructionF3)instruction).AddressParamHex;

                        micro.ProgramCounter = (ushort) UnitConverter.HexToInt(addressHex);
                    } else
                    {
                        micro.ProgramCounter += 2;
                    }

                    micro.ConditionalBit = false;

                    return true;
                }},
                { "11000",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // LOOP Ra, address {F2} 
                    // [R[ra]] <- [R[ra]] – 1 
                    // If R[Ra] != 0 [pc] <- address 

                    MCInstructionF2 instructionF2 = (MCInstructionF2) instruction;

                    // R[Ra]
                    string valueHex = micro.MicroRegisters.GetRegisterValue(instructionF2.Ra);

                    // R[Ra] =- 1
                    string valueMinus1Hex = ArithmeticOperations.HexSubstract(valueHex, "1");
                    micro.MicroRegisters.SetRegisterValue(instructionF2.Ra, valueMinus1Hex);

                    // If R[Ra] != 0
                    if (UnitConverter.HexToSByte(valueMinus1Hex) != 0) {
                        // [pc] <- address
                        micro.ProgramCounter = (ushort) UnitConverter.HexToInt(instructionF2.AddressParamHex);
                    }

                    return true;
                }},
                { "11001",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // GRT Ra, Rb {F1} Cond <- R[Ra] > R[Rb]
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    sbyte raData = UnitConverter.HexToSByte(
                        micro.MicroRegisters.GetRegisterValue(instructionF1.Ra));

                    sbyte rbData = UnitConverter.HexToSByte(
                        micro.MicroRegisters.GetRegisterValue(instructionF1.Rb));

                    micro.ConditionalBit = raData > rbData;

                    return true;
                }},
                { "11010",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // GRTEQ Ra, Rb {F1} Cond <- R[Ra] >= R[Rb]
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    sbyte raData = UnitConverter.HexToSByte(
                        micro.MicroRegisters.GetRegisterValue(instructionF1.Ra));

                    sbyte rbData = UnitConverter.HexToSByte(
                        micro.MicroRegisters.GetRegisterValue(instructionF1.Rb));

                    micro.ConditionalBit = raData >= rbData;

                    return true;
                }},
                { "11011",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // EQ Ra, Rb {F1} Cond <- R[Ra] == R[Rb] 
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    sbyte raData = UnitConverter.HexToSByte(
                        micro.MicroRegisters.GetRegisterValue(instructionF1.Ra));

                    sbyte rbData = UnitConverter.HexToSByte(
                        micro.MicroRegisters.GetRegisterValue(instructionF1.Rb));

                    micro.ConditionalBit = raData == rbData;

                    return true; }},
                { "11100",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // NEQ Ra, Rb {F1} Cond <- R[Ra] != R[Rb] 
                    MCInstructionF1 instructionF1 = (MCInstructionF1) instruction;

                    sbyte raData = UnitConverter.HexToSByte(
                        micro.MicroRegisters.GetRegisterValue(instructionF1.Ra));

                    sbyte rbData = UnitConverter.HexToSByte(
                        micro.MicroRegisters.GetRegisterValue(instructionF1.Rb));

                    micro.ConditionalBit = raData != rbData;

                    return true; 
                }},
                { "11101",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // NOP  {F1} Do nothing
                    return true; }},
                { "11110",     (IMCInstruction instruction, MicroSimulator micro) => { 
                    // CALL address {F3} 
                    // SP <- SP - 2 
                    // mem[SP] <- PC 
                    // PC <- address
                    MCInstructionF3 instructionF3 = (MCInstructionF3) instruction;

                    micro.StackPointer -= 2;
                    
                    micro.WriteToMemory(micro.StackPointer, UnitConverter.IntToHex(micro.ProgramCounter));

                    micro.ProgramCounter = (ushort)UnitConverter.HexToInt(instructionF3.AddressParamHex);
                    
                    // to account for the already increasing Program counter in Micro
                    micro.ProgramCounter -= 2;

                    return true; 
                }},
                { "11111",     (IMCInstruction instruction, MicroSimulator micro) => {
                    // RETURN 
                    // PC <- mem[SP] 
                    // SP <- SP + 2  
                    micro.ProgramCounter = (ushort) UnitConverter.HexToInt(micro.ReadFromMemory(micro.StackPointer));

                    micro.StackPointer+=2;

                    return true; 
                }}
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
