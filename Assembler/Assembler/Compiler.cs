using System;
using System.Collections.Generic;
using System.Text;
using Assembler.Parsing;
using Assembler.Interfaces;
using Assembler.Parsing.InstructionFormats;
using Assembler.Parsing.InstructionItems;

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

        //instructions list
        private int[] decimalInstuctions;
        private Parser parser;
        
        //initialize components
        public Compiler(Parser parser)
        {
            currentAddress = 0;
            constants = new Dictionary<string, int>();
            labels = new Dictionary<string, int>();
            variables = new Dictionary<string, int>();
            this.parser = parser;
            decimalInstuctions = new int[10];
            size = 0;

        }


        /// <summary>
        /// First Pass Over Code
        /// Load addresses and Constants on dictionary
        /// </summary>
        private void LoadConstantsAndLabels()
        {
            parser.Reset();
            while (parser.MoveNext())
            {
                if(parser.CurrentInstruction.Operator == null && parser.CurrentInstruction.GetType().Name == "Label")
                {
                    //Load Labels    
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
                            Console.WriteLine(parser.CurrentInstruction.GetType());
                            variables.Add(((VariableAssign)parser.CurrentInstruction).Name.ToString(), currentAddress);
                            currentAddress++;
                        }
                        else if (parser.CurrentInstruction.Operator.Type == TokenType.OPERATOR)
                        {
                            currentAddress += (currentAddress % 2 == 0) ? 1 : 2;
                            currentAddress++;
                        }
                        
                    }
                }
                
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
            return new string[12];
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

                            //AddOperator(parser.CurrentInstruction.Operator);
                            currentAddress++;
                        }
                        else if (parser.CurrentInstruction.Operator.Type == TokenType.VARIABLE_ASSIGN)
                        {
                            AddInstruction(Convert.ToInt32(((VariableAssign)parser.CurrentInstruction).Values.ToString(),16));
                            currentAddress++;
                        }
                    }
                }

            }

            return true;
        }

        private void AddOperator(Token _operator)
        {
            throw new NotImplementedException();
        }

        public long OutputSizeInBytes()
        {
            return 0;
        }



        


    }
}
