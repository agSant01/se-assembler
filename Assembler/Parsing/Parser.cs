using Assembler.Interfaces;
using Assembler.Parsing.InstructionFormats;
using Assembler.Parsing.InstructionItems;
using System;
using System.Collections.Generic;

namespace Assembler.Parsing
{
<<<<<<< HEAD
    public class Parser
    {
        IFormatInstructions[] instructionList = new IFormatInstructions[10];

        private int sizeCounter = 0;

        private int _current = -1;

        public Parser(Lexer lexer)
        {
            lexer.SkipCommas = true;
            lexer.SkipTabs = true;

            ParseInstructions(lexer);

            lexer.SkipCommas = false;
            lexer.SkipTabs = false;

            lexer.Reset();
        }

=======
    /// <summary>
    /// Parses a Lexer into a set of instructions
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Array of instructions
        /// </summary>
        IFormatInstructions[] instructionList = new IFormatInstructions[10];

        /// <summary>
        /// Size Counter
        /// </summary>
        private int sizeCounter = 0;

        /// <summary>
        /// Current item in the Iterator fashion
        /// </summary>
        private int _current = -1;

        /// <summary>
        /// Creates a Parser instance
        /// </summary>
        /// <param name="lexer">Lexer to be used</param>
        public Parser(Lexer lexer)
        {
            // set lexer to skip non-important characters
            lexer.SkipCommas = true;
            lexer.SkipTabs = true;
            lexer.SkipWhiteSpaces = true;

            ParseInstructions(lexer);

            // restore lexert to original state
            lexer.SkipCommas = false;
            lexer.SkipTabs = false;
            lexer.SkipWhiteSpaces = false;

            // reset lexer iterator
            lexer.Reset();
        }

        /// <summary>
        /// Parses the tokens provided by a lexer
        /// </summary>
        /// <param name="lexer"></param>
>>>>>>> master
        private void ParseInstructions(Lexer lexer)
        {
            IFormatInstructions instruction;

<<<<<<< HEAD
=======
            // iterate over every token
>>>>>>> master
            while (lexer.MoveNext())
            {
                Token currToken = lexer.CurrrentToken;

<<<<<<< HEAD
                // found comment line
=======
                // found a comment line
>>>>>>> master
                if (currToken.Type == TokenType.LINE_COMMENT)
                {
                    // ignore every token in that line
                    while (lexer.CurrrentToken.Type != TokenType.NEW_LINE)
                    {
                        lexer.MoveNext();
                    }

                    // now CurrentToken is a new Line
                }
                // found and origin
<<<<<<< HEAD
                else if (currToken.Type == TokenType.ORIGIN) {
=======
                else if (currToken.Type == TokenType.ORIGIN)
                {
>>>>>>> master
                    lexer.MoveNext();

                    // get the next token (address) associated with the ORG
                    Token address = lexer.CurrrentToken;

                    AddInstruction(new OriginCmd(currToken, address));
                }
                // found an operator
                else if (OperatorsInfo.IsOperator(currToken.Value))
                {
                    // make instruction
                    MakeInstruction(lexer);
                }
                // found variable assign without name
                else if (currToken.Type == TokenType.VARIABLE_ASSIGN)
                {
                    List<Token> variableList = new List<Token>();

                    // move lexer to first param
                    lexer.MoveNext();

                    // add params to list
                    while (lexer.CurrrentToken.Type != TokenType.NEW_LINE)
                    {
                        variableList.Add(lexer.CurrrentToken);
                        lexer.MoveNext();
                    }

                    // add instruction to list
                    AddInstruction(new VariableAssign(currToken, null, variableList.ToArray()));

                    variableList.Clear();
                }
                // found constant assignment
                else if (currToken.Type == TokenType.CONSTANT_ASSIGN)
                {
                    // move to name
                    lexer.MoveNext();
                    Token name = lexer.CurrrentToken;

                    // move to value
                    lexer.MoveNext();
                    Token value = lexer.CurrrentToken;

                    AddInstruction(new ConstantAssign(currToken, name, value));
                }
                // found a possible label, variable name for db, or possible typo in an operator
                else if (currToken.Type == TokenType.IDENTIFIER)
                {
                    // found db after identifier.
                    if (lexer.PeekNext().Type == TokenType.VARIABLE_ASSIGN)
                    {
                        List<Token> paramList = new List<Token>();
<<<<<<< HEAD
                        
=======

>>>>>>> master
                        // move lexer to db keyword
                        lexer.MoveNext();

                        Token dbKeyword = lexer.CurrrentToken;

                        // move lexer to first param
                        lexer.MoveNext();

                        // add params to list
                        while (lexer.CurrrentToken.Type != TokenType.NEW_LINE)
                        {
                            paramList.Add(lexer.CurrrentToken);

                            // iterate
                            lexer.MoveNext();
                        }

                        instruction = new VariableAssign(dbKeyword, currToken, paramList.ToArray());

                        AddInstruction(instruction);
                    }
                    // found a colon after identifier this means it is a label
                    else if (lexer.PeekNext().Type == TokenType.COLON)
                    {
                        Token labelName = lexer.CurrrentToken;
<<<<<<< HEAD
                        
                        // start label block
                        // add current label to the instruction list
                        AddInstruction(new Label(labelName));
                    } else
=======

                        // start label block
                        // add current label to the instruction list
                        AddInstruction(new Label(labelName));
                    }
                    else
>>>>>>> master
                    {
                        List<Token> paramList = new List<Token>();

                        Token invalidInstruction = lexer.CurrrentToken;

                        lexer.MoveNext();

                        // add params to list
                        while (lexer.CurrrentToken.Type != TokenType.NEW_LINE)
                        {
                            paramList.Add(lexer.CurrrentToken);
                            lexer.MoveNext();
                        }

                        AddInstruction(new InvalidInstruction(invalidInstruction, paramList.ToArray()));
                    }
                }
            }
        }

<<<<<<< HEAD
        private void MakeInstruction(Lexer lexer)
        {
            Token[] tempList = new Token[3];
            IFormatInstructions instruction;
            int parameters = 0;
            Token currToken = lexer.CurrrentToken;

            switch (OperatorsInfo.GetInstructionFormat(currToken))
<<<<<<< HEAD
            {
                case EInstructionFormat.FORMAT_1:
                    // extract possible registers
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken))
=======
        /// <summary>
        /// Creates an instruction
        /// </summary>
        /// <param name="lexer">Lexer to be used</param>
        private void MakeInstruction(Lexer lexer)
        {
            // tmep list to store possible parameters
            Token[] tempList = new Token[3];

            // holder of instruction
            IFormatInstructions instruction;

            // parameter count
            int parameters = 0;

            // current token
            Token currToken = lexer.CurrrentToken;

            switch (OperatorsInfo.GetInstructionFormat(currToken.Value))
            {
                case EInstructionFormat.FORMAT_1:
                    // extract possible registers
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken.Value))
>>>>>>> master
=======
            {
                case EInstructionFormat.FORMAT_1:
                    // extract possible registers
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken))
>>>>>>> origin/LuisAndParser
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
<<<<<<< HEAD
<<<<<<< HEAD
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken))
=======
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken.Value))
>>>>>>> master
=======
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken))
>>>>>>> origin/LuisAndParser
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
<<<<<<< HEAD
                    
=======

>>>>>>> master
                    AddInstruction(instruction);

                    break;

                case EInstructionFormat.FORMAT_3:
                    // extract possible registers
<<<<<<< HEAD
<<<<<<< HEAD
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken))
=======
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken.Value))
>>>>>>> master
=======
                    while (parameters < OperatorsInfo.GetNumberOfParams(currToken))
>>>>>>> origin/LuisAndParser
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

<<<<<<< HEAD

        private void AddInstruction(IFormatInstructions instruction) {
=======
        /// <summary>
        /// Add instruction to instruction list
        /// </summary>
        /// <param name="instruction"></param>
        private void AddInstruction(IFormatInstructions instruction)
        {
>>>>>>> master
            // verify size of token array and increment if necessary
            if (sizeCounter >= instructionList.Length / 2)
            {
                Array.Resize(ref instructionList, instructionList.Length * 2);
            }

            instructionList[sizeCounter] = instruction;
            sizeCounter++;
        }

<<<<<<< HEAD

=======
        /// <summary>
        /// Current Instruction of the Iterator
        /// </summary>
>>>>>>> master
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

<<<<<<< HEAD
=======
        /// <summary>
        /// Previous Instruction of the Iterator
        /// </summary>
>>>>>>> master
        public IFormatInstructions Previous
        {
            get
            {
                if (_current <= 0)
                    return null;

                return instructionList[_current - 1];
            }
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Peek next Instruction of the Iterator, without moving the CurrentInstruction
        /// </summary>
        /// <returns></returns>
>>>>>>> master
        public IFormatInstructions PeekNext()
        {
            if (_current + 1 >= sizeCounter)
                return null;

            return instructionList[_current + 1];
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Move to the next Instruction of the Iterator
        /// </summary>
        /// <returns>True if there is a next Token, False otherwise</returns>
>>>>>>> master
        public bool MoveNext()
        {
            if (_current + 1 >= sizeCounter) return false;
            _current++;
            return true;
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Move back one Instruction
        /// </summary>
>>>>>>> master
        public void MoveBack()
        {
            if (_current - 1 < 0) return;
            _current--;
        }

<<<<<<< HEAD
<<<<<<< HEAD
=======
        /// <summary>
        /// Reset the CurrentInstruction to the first Instruction of the Iterator
        /// </summary>
>>>>>>> master
=======
>>>>>>> origin/LuisAndParser
        public void Reset()
        {
            _current = -1;
        }
    }
}
