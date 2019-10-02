using Assembler.Interfaces;
using Assembler.Parsing.InstructionFormats;
using Assembler.Parsing.InstructionItems;
using System;
using System.Collections.Generic;

namespace Assembler.Parsing
{
    public class Parser
    {
        IFormatInstructions[] instructionList = new IFormatInstructions[10];

        private int sizeCounter = 0;

        private int _current = -1;

        public Parser(Lexer lexer)
        {
            ParseInstructions(lexer);
            lexer.Reset();
        }

        private void ParseInstructions(Lexer lexer)
        {
            IFormatInstructions instruction;

            lexer.SkipCommas = true;

            while (lexer.MoveNext())
            {
                Token currToken = lexer.CurrrentToken;


                if (currToken.Type == TokenType.ORIGIN) {
                    lexer.MoveNext();

                    Token address = lexer.CurrrentToken;

                    AddInstruction(new OriginCmd(currToken, address));
                }
                else if (OperatorsInfo.IsOperator(currToken.Value))
                {
                    MakeInstruction(lexer);
                }
                else if (currToken.Type == TokenType.IDENTIFIER)
                {
                    List<Token> list = new List<Token>();
                    if (lexer.PeekNext().Type == TokenType.VARIABLE_ASSIGN)
                    {
                        lexer.MoveNext();
                        Token name = lexer.CurrrentToken;
                        while (lexer.CurrrentToken.Type != TokenType.NEW_LINE)
                        {
                            list.Add(lexer.CurrrentToken);
                            lexer.MoveNext();
                        }

                        instruction = new VariableAssign(name, currToken, list.ToArray());

                        AddInstruction(instruction);
                    } else if (lexer.PeekNext().Type == TokenType.COLON)
                    {
                        Token labelName = lexer.CurrrentToken;

                        // start label block
                        AddInstruction(new Label(labelName));

                        // ignore ':', WHITE_SPACE, and NEW_LINE
                        while (lexer.CurrrentToken.Type != TokenType.TAB)
                        {
                            lexer.MoveNext();
                        }                
                        
                        // first instruction under label
                        while(true)
                        {
                            // each instruction under label
                            if (lexer.CurrrentToken.Type == TokenType.TAB)
                            {
                                // move to beginning of instruction
                                lexer.MoveNext();

                                // add instruction to list
                                MakeInstruction(lexer);

                                lexer.MoveNext();
                                lexer.MoveNext();
                            } else
                            {
                                // no more instructions under label
                                // end label and exit
                                break;
                            }
                        }
                        AddInstruction(new EndLabel(labelName));
                    }
                }
            }

            lexer.SkipCommas = false;
        }

        private void MakeInstruction(Lexer lexer)
        {
            Token[] tempList = new Token[3];
            IFormatInstructions instruction;
            int parameters = 0;
            Token currToken = lexer.CurrrentToken;

            switch (OperatorsInfo.GetInstructionFormat(currToken.Value))
            {
                case EInstructionFormat.FORMAT_1:
                    // extract possible registers
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken.Value))
                    {
                        lexer.MoveNext();
                        tempList[parameters] = lexer.CurrrentToken;
                        parameters++;
                    }

                    instruction = new InstructionFormat1(
                        currToken,
                        tempList[0],
                        tempList[1],
                        tempList[2]
                        );

                    AddInstruction(instruction);

                    break;

                case EInstructionFormat.FORMAT_2:
                    // extract possible registers
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken.Value))
                    {
                        lexer.MoveNext();
                        tempList[parameters] = lexer.CurrrentToken;
                        parameters++;
                    }

                    instruction = new InstructionFormat2(
                        currToken,
                        tempList[0],
                        tempList[1]
                        );
                    
                    AddInstruction(instruction);

                    break;

                case EInstructionFormat.FORMAT_3:
                    // extract possible registers
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken.Value))
                    {
                        lexer.MoveNext();
                        tempList[parameters] = lexer.CurrrentToken;
                        parameters++;
                    }

                    instruction = new InstructionFormat3(
                        currToken,
                        tempList[0]
                        );

                    AddInstruction(instruction);

                    break;

                default:
                    break;
            }
        }


        private void AddInstruction(IFormatInstructions instruction) {
            // verify size of token array and increment if necessary
            if (sizeCounter >= instructionList.Length / 2)
            {
                Array.Resize(ref instructionList, instructionList.Length * 2);
            }

            instructionList[sizeCounter] = instruction;
            sizeCounter++;
        }


        public IFormatInstructions CurrentInstruction
        {
            get
            {
                if (_current == -1)
                {
                    _current++;
                }
                return instructionList[_current];
            }
        }

        public IFormatInstructions Previous
        {
            get
            {
                if (_current <= 0)
                    return null;

                return instructionList[_current - 1];
            }
        }

        public IFormatInstructions PeekNext()
        {
            if (_current + 1 >= sizeCounter)
                return null;

            return instructionList[_current + 1];
        }

        public bool MoveNext()
        {
            if (_current + 1 >= sizeCounter) return false;
            _current++;
            return true;
        }

        public void MoveBack()
        {
            if (_current - 1 < 0) return;
            _current--;
        }
    }
}
