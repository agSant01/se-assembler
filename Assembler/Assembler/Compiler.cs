using System;
using System.Collections.Generic;
using System.Text;
using Assembler.Parsing;
using Assembler.Interfaces;
using Assembler.Parsing.InstructionFormats;
using Assembler.Parsing.InstructionItems;
using System.Text.RegularExpressions;

namespace Assembler.Assembler
{
    public class Compiler
    {
        //address 
        private int currentAddress;
        private int size;

        //content mapping
        private Dictionary<string, int> constants;
        private Dictionary<string, int> labels;
        private Dictionary<string, int> variables;
        private Dictionary<string, int> vMemory;
        private AssemblyLogger logger;

        //instructions list
        private int[] decimalInstuctions;
        private Parser parser;

        //initialize components

        /// <summary>
        /// Two pass Assembler
        /// <paramref name="parser"/>
        /// </summary>
        public Compiler(Parser parser)
        {
            currentAddress = 0;
            constants = new Dictionary<string, int>();
            labels = new Dictionary<string, int>();
            variables = new Dictionary<string, int>();
            this.parser = parser;
            decimalInstuctions = new int[10];
            logger = new AssemblyLogger();
            size = 0;

        }

        /// <summary>
        /// Two pass Assembler
        /// <paramref name="parser"/>
        /// <paramref name="logger"/>
        /// </summary>
        public Compiler(Parser parser, AssemblyLogger logger)
        {
            currentAddress = 0;
            constants = new Dictionary<string, int>();
            labels = new Dictionary<string, int>();
            variables = new Dictionary<string, int>();
            this.parser = parser;
            decimalInstuctions = new int[10];
            this.logger = logger;
            size = 0;

        }


        /// <summary>
        /// First Pass Over Code
        /// Load addresses and Constants on dictionary
        /// </summary>
        private void LoadConstantsAndLabels()
        {
            int lineCount = 0;
            parser.Reset();
            while (parser.MoveNext())
            {
                if(parser.CurrentInstruction.Operator == null && parser.CurrentInstruction.GetType().Name == "Label")
                {
                    //Load Labels and check if next element is an instruction
                    labels.Add(((Label)parser.CurrentInstruction).Name.ToString(), 
                        ((currentAddress)%2==0)?(currentAddress): (currentAddress + 1));
                }
                else
                {
                    //Update address counter
                    if (parser.CurrentInstruction.Operator.Type == TokenType.ORIGIN)
                    {
                        currentAddress = Convert.ToInt32(((OriginCmd)parser.CurrentInstruction).Address.ToString(),16);
                    }
                    else 
                    {
                        //load constant references
                        if (parser.CurrentInstruction.Operator.Type == TokenType.CONSTANT_ASSIGN)
                        {
                            constants.Add(((ConstantAssign)parser.CurrentInstruction).Name.ToString(),
                                Convert.ToInt32(((ConstantAssign)parser.CurrentInstruction).Value.ToString(),16));
                            
                        }
                        else if (parser.CurrentInstruction.Operator.Type == TokenType.VARIABLE_ASSIGN)
                        {
                            
                            if (variables.ContainsValue(currentAddress))
                                logger.Warning("Memory Overide",lineCount.ToString(),currentAddress.ToString(),vMemory[((VariableAssign)parser.CurrentInstruction).Name.ToString()].ToString());

                            variables.Add(((VariableAssign)parser.CurrentInstruction).Name.ToString(), currentAddress);
                            currentAddress+= ((VariableAssign)parser.CurrentInstruction).Values.Length;
                            vMemory.Add(((VariableAssign)parser.CurrentInstruction).Name.ToString(), Convert.ToInt32(((VariableAssign)parser.CurrentInstruction).Values.ToString(),16));
                        }
                        else if (parser.CurrentInstruction.Operator.Type == TokenType.OPERATOR)
                        {
                            ///--------------OPERATORS MUST BE ON EVEN ADDRESSES--------------------
                            currentAddress += (currentAddress % 2 == 0) ? 1 : 2;
                            currentAddress++;
                        }
                        
                    }
                }
                lineCount++;
            }

            
        }



        private void AddInstruction(int decimalInstruction)
        {
            // verify size of token array and increment if necessary
            if (size >= decimalInstuctions.Length / 2)
            {
                Array.Resize(ref decimalInstuctions, decimalInstuctions.Length * 2);
            }

            decimalInstuctions[size] = decimalInstruction;
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
        ///TODO: 
        public string[] GetOutput()
        {
            string[] lines = new string[size/2];
            int currentLine = 0;
            for (int i = 0; i < size; i++)
            {
                lines[currentLine] += Convert.ToString(decimalInstuctions[i],16).PadLeft(2,'0') + " ";
                if (i % 2 != 0)
                    currentLine += (currentLine<size/2)? 1:0;

            }
            Console.WriteLine($"usage = {size} bytes");
            return lines;
        }

        public int Size()
        {
            return size;
        }

        public bool Compile()
        {
            LoadConstantsAndLabels();

            parser.Reset();
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
                            if((currentAddress % 2 == 0))
                            {
                                currentAddress += 1;
                            }
                            else
                            {
                                AddInstruction(0);
                                currentAddress += 2;
                            }

                            AddOperator(parser.CurrentInstruction);
                            currentAddress++;
                        }
                        else if (parser.CurrentInstruction.Operator.Type == TokenType.VARIABLE_ASSIGN)
                        {
                           
                            foreach(Hexa variable in ((VariableAssign)parser.CurrentInstruction).Values)
                            {
                                AddInstruction(Convert.ToInt32(variable.ToString(), 16));
                                currentAddress++;
                            }
                               
                        }
                    }
                }

            }

            return true;
        }

        private void AddOperator(IFormatInstructions _operator)
        {
            string binInstruction = GetBinaryFormat(_operator);
            
            if(binInstruction.Length != 16)
            {
                throw new Exception("invalid bytes");
            }
            string firstByte = binInstruction.Substring(0,8);
            AddInstruction(Convert.ToInt32(firstByte, 2));
            string secondByte = binInstruction.Substring(8,8);
            AddInstruction(Convert.ToInt32(secondByte, 2));

            

        }

        private string GetBinaryFormat(IFormatInstructions _operator)
        {
            int opcode = OperatorsInfo.GetOPCode(_operator.Operator);

            switch (OperatorsInfo.GetInstructionFormat(_operator.Operator))
            {
                case EInstructionFormat.FORMAT_1:
                    {
                        InstructionFormat1 format = (InstructionFormat1)_operator;
                        int Ra = (format.RegisterA.ToString() == "") ? 0 :Convert.ToInt32(Regex.Replace(format.RegisterA.ToString(), @"[.\D+]", ""));
                        int Rb = (format.RegisterB.ToString() == "") ? 0 : Convert.ToInt32(Regex.Replace(format.RegisterA.ToString(), @"[.\D+]", ""));
                        int Rc = (format.RegisterC.ToString() == "") ? 0 : Convert.ToInt32(Regex.Replace(format.RegisterA.ToString(), @"[.\D+]", ""));

                        return $"{Convert.ToString(opcode, 2).PadLeft(5,'0')}" +
                            $"{Convert.ToString(Ra, 2).PadLeft(3,'0')}" +
                            $"{Convert.ToString(Rb, 2).PadLeft(3, '0')}" +
                            $"{Convert.ToString(Rc, 2).PadLeft(3, '0')}00" ;
                    }
                case EInstructionFormat.FORMAT_2:
                    {
                        InstructionFormat2 format = (InstructionFormat2)_operator;
                        int Ra = (format.RegisterA.ToString() == "") ? 0 : Convert.ToInt32(Regex.Replace(format.RegisterA.ToString(), @"[.\D+]", ""));


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
                                constOrAddr = Convert.ToInt32(format.ConstOrAddress.ToString().Replace("#",""), 16);
                            }
                            catch
                            {
                                //send error message (undefined variable)
                                logger.Error("Variable not defined",currentAddress.ToString(),"Variable called but never defined");
                                return null;
                            }
                        }

                        
                        return $"{Convert.ToString(opcode, 2).PadLeft(5, '0')}" +
                            $"{Convert.ToString(Ra, 2).PadLeft(3, '0')}" +
                            $"{Convert.ToString(constOrAddr, 2).PadLeft(8, '0')}";
                           
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

        public long OutputSizeInBytes()
        {
            return Size();
        }



        


    }
}
