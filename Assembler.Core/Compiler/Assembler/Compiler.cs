using Assembler.Interfaces;
using Assembler.Parsing;
using Assembler.Parsing.InstructionFormats;
using Assembler.Parsing.InstructionItems;
using Assembler.Utils;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Assembler.Assembler
{
    public class Compiler
    {
        //address 
        private int currentAddress;
        private int size;
        private readonly int totalAddressesKB;


        //content mapping
        private readonly Dictionary<string, int> constants;
        private readonly Dictionary<string, int> labels;
        private readonly Dictionary<string, int> variables;
        private readonly Dictionary<string, int> vMemory;

        public AssemblyLogger AsmLogger { get; }

        private string[] compiledLines;

        //instructions list
        private int[] decimalInstuctions;
        private readonly Parser parser;

        private int currentLine = 0;

        //initialize components

        /// <summary>
        /// Two pass Assembler
        /// <paramref name="parser"/>
        /// </summary>
        public Compiler(Parser parser, int sizeInKiloBytes = 4) :
            this(parser, new AssemblyLogger("ASM"), sizeInKiloBytes)
        { }

        /// <summary>
        /// Two pass Assembler
        /// <paramref name="parser"/>
        /// <paramref name="logger"/>
        /// </summary>
        public Compiler(Parser parser, AssemblyLogger logger, int sizeInKiloBytes = 4)
        {
            currentAddress = 0;
            constants = new Dictionary<string, int>();
            labels = new Dictionary<string, int>();
            variables = new Dictionary<string, int>();
            vMemory = new Dictionary<string, int>();
            this.parser = parser;
            decimalInstuctions = new int[10];
            this.AsmLogger = logger;
            size = 0;
            totalAddressesKB = sizeInKiloBytes * 1024;  // 1024 Equals 1KB
        }


        /// <summary>
        /// First Pass Over Code
        /// Load addresses and Constants on dictionary
        /// </summary>
        private void LoadConstantsAndLabels()
        {
            int lineCount = 0;
            parser.Reset();

            AsmLogger.StatusUpdate("Loading labels and constants");

            while (parser.MoveNext())
            {

                if (parser.CurrentInstruction.Operator == null && parser.CurrentInstruction.GetType().Name == "Label")
                {
                    //Load Labels and check if next element is an instruction
                    labels.Add(((Label)parser.CurrentInstruction).Name.ToString(),
                        ((currentAddress) % 2 == 0) ? (currentAddress) : (currentAddress + 1));
                }
                else if (parser.CurrentInstruction.Operator.Type == TokenType.ORIGIN)
                {
                    currentAddress = Convert.ToInt32(((OriginCmd)parser.CurrentInstruction).Address.ToString(), 16);
                }
                else if (parser.CurrentInstruction.Operator.Type == TokenType.CONSTANT_ASSIGN)
                {
                    constants.Add(((ConstantAssign)parser.CurrentInstruction).Name.ToString(),
                        Convert.ToInt32(((ConstantAssign)parser.CurrentInstruction).Value.ToString(), 16));
                }
                else if (parser.CurrentInstruction.Operator.Type == TokenType.VARIABLE_ASSIGN)
                {

                    if (variables.ContainsValue(currentAddress))
                        AsmLogger.Warning("Memory Overide", currentAddress.ToString(), currentAddress.ToString(), vMemory[((VariableAssign)parser.CurrentInstruction).Name.ToString()].ToString());

                    variables.Add(((VariableAssign)parser.CurrentInstruction).Name.ToString(), currentAddress);
                    currentAddress += ((VariableAssign)parser.CurrentInstruction).Values.Length;

                    string ofset = "";
                    foreach (Hexa variable in ((VariableAssign)parser.CurrentInstruction).Values)
                    {
                        vMemory.Add(((VariableAssign)parser.CurrentInstruction).Name.ToString() + ofset, Convert.ToInt32(variable.ToString(), 16));
                        ofset += "0";
                    }
                    //vMemory.Add(((VariableAssign)parser.CurrentInstruction).Name.ToString(), Convert.ToInt32(((VariableAssign)parser.CurrentInstruction).Values[0].ToString(), 16));
                }
                else if (parser.CurrentInstruction.Operator.Type == TokenType.OPERATOR)
                {
                    ///--------------OPERATORS MUST BE ON EVEN ADDRESSES--------------------
                    currentAddress += (currentAddress % 2 == 0) ? 1 : 2;
                    currentAddress++;
                }

                lineCount++;

                ++currentLine;
            }

            currentAddress = 0;

            AsmLogger.StatusUpdate("Finished loading labels and constants");
        }

        private bool HaveSyntaxErrors()
        {
            AsmLogger.StatusUpdate("Syntax analysis");
            int lineCount = 0;
            parser.Reset();
            currentLine = 0;
            while (parser.MoveNext())
            {
                if (!parser.CurrentInstruction.IsValid)
                {
                    AsmLogger.Error("Invalid syntax", currentLine.ToString(), $"{parser.CurrentInstruction.Operator.Value} is a syntax violation");
                };
                lineCount++;
                ++currentLine;
            }
            return false;
        }

        private void AddInstruction(int decimalInstruction)
        {
            // verify size of token array and increment if necessary
            if (currentAddress >= decimalInstuctions.Length)
            {
                Array.Resize(ref decimalInstuctions, decimalInstuctions.Length * 2);
            }

            decimalInstuctions[currentAddress] = decimalInstruction;
            size++;
        }

        public Dictionary<string, int> GetConstants()
        {
            return constants;
        }

        public Dictionary<string, int> GetLabels()
        {
            return labels;
        }
        public Dictionary<string, int> GetVariables()
        {
            return variables;
        }
        public int[] GetDecimalInstructions()
        {
            return decimalInstuctions;
        }

        /// <summary>
        /// Get Hex output lines 
        /// </summary>
        public string[] GetOutput()
        {
            return compiledLines;
        }

        public int Size()
        {
            return size;
        }
        /// <summary>
        /// Secund Pass Over Code
        /// get references and instructions and format
        /// </summary>
        public bool Compile()
        {
            AsmLogger.StatusUpdate("Assembly process started");

            HaveSyntaxErrors();

            LoadConstantsAndLabels();

            parser.Reset();

            currentLine = 0;

            while (parser.MoveNext())
            {
                if (parser.CurrentInstruction.Operator != null)
                {
                    //Update address counter
                    if (parser.CurrentInstruction.Operator.Type == TokenType.ORIGIN)
                    {
                        currentAddress = Convert.ToInt32(((OriginCmd)parser.CurrentInstruction).Address.ToString(), 16);
                    }
                    else
                    {
                        if (parser.CurrentInstruction.Operator.Type == TokenType.OPERATOR)
                        {
                            if (currentAddress % 2 == 1)
                            {
                                currentAddress += 1;
                            }

                            AddOperator(parser.CurrentInstruction);
                        }
                        else if (parser.CurrentInstruction.Operator.Type == TokenType.VARIABLE_ASSIGN)
                        {

                            foreach (Hexa variable in ((VariableAssign)parser.CurrentInstruction).Values)
                            {
                                AddInstruction(Convert.ToInt32(variable.ToString(), 16));
                                currentAddress++;
                            }

                        }
                    }
                }
                ++currentLine;
            }

            AsmLogger.StatusUpdate("Assembly process completed");
            AsmLogger.StatusUpdate("Generating of Object file");

            compiledLines = new string[(totalAddressesKB / 2)];

            int instruction = 0;
            for (int currentLine = 0; currentLine < (totalAddressesKB / 2); currentLine++)
            {
                if (instruction < decimalInstuctions.Length)
                {
                    compiledLines[currentLine] += Convert.ToString(decimalInstuctions[instruction], 16)
                        .PadLeft(2, '0').ToUpper() + " ";
                    instruction++;
                    if (instruction < decimalInstuctions.Length)
                    {
                        compiledLines[currentLine] += Convert.ToString(decimalInstuctions[instruction], 16)
                        .PadLeft(2, '0').ToUpper();
                        instruction++;
                    }
                    else
                    {
                        compiledLines[currentLine] += "00";
                    }
                }
                else
                {
                    compiledLines[currentLine] += "00 00";
                }
            }

            AsmLogger.StatusUpdate("Finished generating Object file");

            return true;
        }

        /// <summary>
        /// add operators to the instruction list in decimal
        /// </summary>
        private void AddOperator(IFormatInstructions _operator)
        {
            string binInstruction = GetBinaryFormat(_operator);

            //this will execute if it was an undefined variable
            if (binInstruction == null)
                return;

            if (binInstruction.Length != 16)
            {
                AsmLogger.Error($"Invalid bit size: {binInstruction.Length}", 
                    currentLine.ToString(), 
                    $"Malformatted Instruction {_operator}, Binary: {binInstruction}");
            } else if (!_operator.IsValid)
            {
                AsmLogger.Error($"Invalid instruction {_operator}",
                    currentLine.ToString(),
                    $"Invalid syntax or malformatted instruction"
                );
            }

            string firstByte = binInstruction.Substring(0, 8);
            AddInstruction(Convert.ToInt32(firstByte, 2));
            currentAddress++;

            string secondByte = binInstruction.Substring(8, 8);
            AddInstruction(Convert.ToInt32(secondByte, 2));
            currentAddress++;
        }

        /// <summary>
        /// get binary format of each instruction
        /// </summary>
        private string GetBinaryFormat(IFormatInstructions _operator)
        {
            int opcode = OperatorsInfo.GetOPCode(_operator.Operator);


            switch (OperatorsInfo.GetInstructionFormat(_operator.Operator))
            {
                case EInstructionFormat.FORMAT_1:
                    {
                        int Rc = 0;
                        int Ra = 0;
                        int Rb = 0;
                        InstructionFormat1 format = (InstructionFormat1)_operator;

                        if (format.RegisterA != null)
                            Ra = (format.RegisterA?.ToString() == "") ? 0 : Convert.ToInt32(Regex.Replace(format.RegisterA.ToString(), @"[.\D+]", ""));
                        if (format.RegisterC != null)
                            Rb = (format.RegisterB?.ToString() == "") ? 0 : Convert.ToInt32(Regex.Replace(format.RegisterB.ToString(), @"[.\D+]", ""));
                        if (format.RegisterC != null)
                            Rc = (format.RegisterC.ToString() == "") ? 0 : Convert.ToInt32(Regex.Replace(format.RegisterC.ToString(), @"[.\D+]", ""));
                        return $"{Convert.ToString(opcode, 2).PadLeft(5, '0')}" +
                            $"{Convert.ToString(Ra, 2).PadLeft(3, '0')}" +
                            $"{Convert.ToString(Rb, 2).PadLeft(3, '0')}" +
                            $"{Convert.ToString(Rc, 2).PadLeft(3, '0')}00";
                    }
                case EInstructionFormat.FORMAT_2:
                    {
                        InstructionFormat2 format = (InstructionFormat2)_operator;
                        int Ra = (format.RegisterA.ToString() == "") ? 0 : Convert.ToInt32(Regex.Replace(format.RegisterA.ToString(), @"[.\D+]", ""));

                        int constOrAddr;

                        if (constants.ContainsKey(format.ConstOrAddress.ToString()))
                            constOrAddr = constants[format.ConstOrAddress.ToString()];
                        else if (variables.ContainsKey(format.ConstOrAddress.ToString()))
                            constOrAddr = variables[format.ConstOrAddress.ToString()];
                        else if (labels.ContainsKey(format.ConstOrAddress.ToString()))
                            constOrAddr = labels[format.ConstOrAddress.ToString()];
                        else
                        {
                            //check if the constant or address is a direct input
                            try
                            {
                              constOrAddr = UnitConverter.BinaryToByte(
                                    UnitConverter.HexToBinary(
                                        format.ConstOrAddress.ToString().Replace("#", "")
                                        )
                                    );
                            } catch (OverflowException)
                            {
                                //send error message (undefined variable)
                                AsmLogger.Error($"Overflow in instruction: {_operator}",
                                    currentLine.ToString(),
                                    $"Value '{format.ConstOrAddress}'");

                                return null;
                            }
                            catch
                            {
                                //send error message (undefined variable)
                                AsmLogger.Error($"Variable not defined: {_operator}", 
                                    currentLine.ToString(), 
                                    "Variable called but never defined");
                                return null;
                            }
                        }
                        return $"{UnitConverter.IntToBinary(opcode, defaultWidth: 5)}" +
                            $"{UnitConverter.IntToBinary(Ra, defaultWidth: 3)}" +
                            $"{UnitConverter.IntToBinary(constOrAddr, defaultWidth: 8)}";

                    }
                case EInstructionFormat.FORMAT_3:
                    {
                        InstructionFormat3 format = (InstructionFormat3)_operator;

                        int constOrAddr = 0;
                        if (constants.ContainsKey(format.ConstOrAddress.ToString()))
                            constOrAddr = constants[format.ConstOrAddress.ToString()];
                        else if (variables.ContainsKey(format.ConstOrAddress.ToString()))
                            constOrAddr = variables[format.ConstOrAddress.ToString()];
                        else if (labels.ContainsKey(format.ConstOrAddress.ToString()))
                            constOrAddr = labels[format.ConstOrAddress.ToString()];
                        else
                        {
                            //check if the constant or address is a direct input
                            try
                            {
                                constOrAddr = Convert.ToInt32(format.ConstOrAddress.ToString().Replace("#", ""), 16);
                            }
                            catch
                            {
                                //send error message (undefined variable)
                                return null;
                            }
                        }


                        return $"{Convert.ToString(opcode, 2).PadLeft(5, '0')}" +
                            $"{Convert.ToString(constOrAddr, 2).PadLeft(11, '0')}";

                    }
                default:
                    {
                        throw new Exception("Token is not an instruction");
                    }
            }

        }

        /// <summary>
        /// size of file in bytes
        /// </summary>
        public long OutputSizeInBytes()
        {
            return Size();
        }
    }
}
