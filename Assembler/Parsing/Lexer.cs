using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing
{
    public class Lexer
    {
        private string[] text;

        private Token[] tokens = new Token[10];
        private int sizeCounter = 0;

        private int _current = -1;

        public Lexer(string[] text)
        {
            this.text = text;
            Start();
        }
       
        private void Start()
        {
            foreach (string line in text)
            {
                char[] charsInLine = line.ToCharArray();
                int leftIdx = 0;
                int rightIdx = 0;

                while (leftIdx < charsInLine.Length && rightIdx < charsInLine.Length)
                {
                    if (char.IsWhiteSpace(charsInLine[rightIdx]))
                    {
                        MakeToken(leftIdx, rightIdx - 1, ref charsInLine);

                        if (char.Equals(charsInLine[rightIdx], '\t'))
                            MakeToken(TokenType.TAB, char.ToString(charsInLine[rightIdx]));
                        //else
                        //    MakeToken(TokenType.WHITE_SPACE, char.ToString(charsInLine[rightIdx]));

                        leftIdx = rightIdx + 1;
                    } else if (charsInLine[rightIdx] == ',')
                    {
                        MakeToken(leftIdx, rightIdx - 1, ref charsInLine);
                        MakeToken(TokenType.COMMA, char.ToString(charsInLine[rightIdx]));
                        leftIdx = rightIdx + 1;
                    } else if (charsInLine[rightIdx] == ':')
                    {
                        MakeToken(leftIdx, rightIdx - 1, ref charsInLine);
                        MakeToken(TokenType.COLON, char.ToString(charsInLine[rightIdx]));
                        leftIdx = rightIdx + 1;
                    }

                    ++rightIdx;
                }

                if (charsInLine.Length != 0)
                    MakeToken(leftIdx, charsInLine.Length - 1, ref charsInLine);

                MakeToken(TokenType.NEW_LINE, "\\n");
            }
        }
        private void MakeToken(int leftIdx, int rightIdx, ref char[] charsInLine)
        {
            string value = new string(charsInLine, leftIdx, rightIdx - leftIdx + 1);

=======

namespace Assembler.Parsing
{
    /// <summary>
    /// Does lexical analysis by tokenizing a text input.
    /// </summary>
    public class Lexer
    {
        /// <summary>
        /// Representation of text stored as an array. 
        /// Every item representa a line
        /// </summary>
        private readonly string[] text;

        /// <summary>
        /// Token array
        /// </summary>
        private Token[] tokens = new Token[10];

        /// <summary>
        /// Size counter
        /// </summary>
        private int sizeCounter = 0;

        /// <summary>
        /// Current item in the Iterator fashion
        /// </summary>
        private int _current = -1;

        /// <summary>
        /// Creates a Lexer instance
        /// </summary>
        /// <param name="text">Array of strings. Every item should represent a line of the text</param>
        public Lexer(string[] text)
        {
            this.text = text;

            // start tokenization
            Start();
        }

        /// <summary>
        /// Tokenizes the text.
        /// </summary>
        private void Start()
        {
            // iterate over every line
            foreach (string line in text)
            {
                // get every character in the line
                char[] charsInLine = line.ToCharArray();

                // position indexes
                int leftIdx = 0;
                int rightIdx = 0;

                // iterate over the line character per character
                while (leftIdx < charsInLine.Length && rightIdx < charsInLine.Length)
                {
                    // is char is a WHITE_SPACE or TAB
                    if (char.IsWhiteSpace(charsInLine[rightIdx]))
                    {
                        // make a token out of the character group previous to the whitespace
                        MakeToken(leftIdx, rightIdx - 1, ref charsInLine);

                        // create a token either of a WHITE_SPACE or TAB
                        if (char.Equals(charsInLine[rightIdx], '\t'))
                            MakeToken(TokenType.TAB, char.ToString(charsInLine[rightIdx]));
                        else
                            MakeToken(TokenType.WHITE_SPACE, char.ToString(charsInLine[rightIdx]));

                        // move left index to the next of the current right index
                        leftIdx = rightIdx + 1;
                    }
                    // current char is a comma
                    else if (charsInLine[rightIdx] == ',')
                    {
                        // make a token out of the character group previous to the COMMA
                        MakeToken(leftIdx, rightIdx - 1, ref charsInLine);

                        // make a token of the COMMA
                        MakeToken(TokenType.COMMA, char.ToString(charsInLine[rightIdx]));

                        // move left index to the next of the current right index
                        leftIdx = rightIdx + 1;
                    }
                    // current char is a COLON
                    else if (charsInLine[rightIdx] == ':')
                    {
                        // make a token out of the character group previous to the COLON
                        MakeToken(leftIdx, rightIdx - 1, ref charsInLine);

                        // make a token of the COLON
                        MakeToken(TokenType.COLON, char.ToString(charsInLine[rightIdx]));

                        // move left index to the next of the current right index
                        leftIdx = rightIdx + 1;
                    }

                    // move right index to the next character
                    ++rightIdx;
                }

                // get the last character group
                if (charsInLine.Length != 0)
                    // make token of last character group
                    MakeToken(leftIdx, charsInLine.Length - 1, ref charsInLine);

                // make token of a new line
                MakeToken(TokenType.NEW_LINE, "\\n");
            }
        }

        /// <summary>
        /// Creates a token out of a character group
        /// </summary>
        /// <param name="leftIdx">First index of the group</param>
        /// <param name="rightIdx">Last index of the group</param>
        /// <param name="charsInLine">Reference to the char array that contains the character group</param>
        private void MakeToken(int leftIdx, int rightIdx, ref char[] charsInLine)
        {
            // create a string from the character group
            string value = new string(charsInLine, leftIdx, rightIdx - leftIdx + 1);

            // token is a comment character group
>>>>>>> master
            if (value.Contains("//"))
            {
                MakeToken(TokenType.LINE_COMMENT, value);
            }
<<<<<<< HEAD
            else if (OperatorsInfo.IsOperator(value))
            {
                MakeToken(TokenType.OPERATOR, value);
            } else if (isRegister(leftIdx, rightIdx, ref charsInLine))
            {
                MakeToken(TokenType.REGISTER, value);
            } else if (value.ToLower().Equals("db"))
            {
                MakeToken(TokenType.VARIABLE_ASSIGN, value);
            } else if (value.ToLower().Equals("const"))
            {
                MakeToken(TokenType.CONSTANT_ASSIGN, value);
            } else if (value.ToLower().Equals("org"))
            {
                MakeToken(TokenType.ORIGIN, value);
            }
=======
            // token is an operator
            else if (OperatorsInfo.IsOperator(value))
            {
                MakeToken(TokenType.OPERATOR, value);
            }
            // token is a register
            else if (IsRegister(leftIdx, rightIdx, ref charsInLine))
            {
                MakeToken(TokenType.REGISTER, value);
            }
            // token is a variable assign keyword
            else if (value.ToLower().Equals("db"))
            {
                MakeToken(TokenType.VARIABLE_ASSIGN, value);
            }
            // token is a constant assign keyword
            else if (value.ToLower().Equals("const"))
            {
                MakeToken(TokenType.CONSTANT_ASSIGN, value);
            }
            // token is a ORG command keyword
            else if (value.ToLower().Equals("org"))
            {
                MakeToken(TokenType.ORIGIN, value);
            }
            // token is an identifier
>>>>>>> master
            else if (!string.IsNullOrEmpty(value))
            {
                MakeToken(TokenType.IDENTIFIER, value);
            }
<<<<<<< HEAD
        }

        public bool SkipCommas { get; set; }

        public bool SkipTabs { get; set; }

        private bool isRegister(int leftIdx, int rightIdx, ref char[] charsInLine)
        {
            return rightIdx - leftIdx + 1 == 2 &&
                (
                charsInLine[leftIdx] == 'R' || charsInLine[leftIdx] == 'r'
                );
        }

=======

            // token is null or empty
            // do nothing
        }

        /// <summary>
        /// Creates a token out of a string and TokenType.
        /// </summary>
        /// <param name="type">Token type</param>
        /// <param name="value">String value of the token</param>
>>>>>>> master
        private void MakeToken(TokenType type, string value)
        {
            // verify size of token array and increment if necessary
            if (sizeCounter >= tokens.Length / 2)
            {
                Array.Resize(ref tokens, tokens.Length * 2);
            }

            tokens[sizeCounter] = new Token(type, value);
            sizeCounter++;
        }

<<<<<<< HEAD
        public Token CurrrentToken {
            get {
=======
        /// <summary>
        /// If true the lexer iterator will skip COMMAS
        /// </summary>
        public bool SkipCommas { get; set; }

        /// <summary>
        /// If true the lexer iterator will skip TABS
        /// </summary>
        public bool SkipTabs { get; set; }

        /// <summary>
        /// If true the lexer iterator will skip WHITE_SPACES
        /// </summary>
        public bool SkipWhiteSpaces { get; set; }

        /// <summary>
        /// Validates if the character group represents a token
        /// </summary>
        /// <param name="leftIdx">First index of the group</param>
        /// <param name="rightIdx">Last index of the group</param>
        /// <param name="charsInLine">Reference to the char array that contains the character group</param>
        /// <returns>True if represents a register</returns>
        private bool IsRegister(int leftIdx, int rightIdx, ref char[] charsInLine)
        {
            return rightIdx - leftIdx + 1 == 2 &&
                (
                charsInLine[leftIdx] == 'R' || charsInLine[leftIdx] == 'r'
                );
        }

        /// <summary>
        /// Current Token of the Iterator
        /// </summary>
        public Token CurrrentToken
        {
            get
            {
>>>>>>> master
                if (_current == -1)
                {
                    _current++;
                }
                return tokens[_current];
            }
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Previous Token of the Iterator
        /// </summary>
>>>>>>> master
        public Token Previous
        {
            get
            {
                if (_current <= 0)
                    return null;

                return tokens[_current - 1];
            }
        }

<<<<<<< HEAD
        public Token PeekNext()
        { 
=======
        /// <summary>
        /// Peek next Token of the Iterator, without moving the CurrentToken
        /// </summary>
        /// <returns>Next Token</returns>
        public Token PeekNext()
        {
>>>>>>> master
            if (_current + 1 >= sizeCounter)
                return null;

            return tokens[_current + 1];
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Move to the next Token of the Iterator
        /// </summary>
        /// <returns>True if there is a next Token, False otherwise</returns>
>>>>>>> master
        public bool MoveNext()
        {
            if (_current + 1 >= sizeCounter) return false;
            _current++;

            if (SkipCommas && CurrrentToken.Type == TokenType.COMMA)
                return MoveNext();

            if (SkipTabs && CurrrentToken.Type == TokenType.TAB)
                return MoveNext();

<<<<<<< HEAD
            return true;
        }

=======
            if (SkipWhiteSpaces && CurrrentToken.Type == TokenType.WHITE_SPACE)
                return MoveNext();

            return true;
        }

        /// <summary>
        /// Move back one Token
        /// </summary>
>>>>>>> master
        public void MoveBack()
        {
            if (_current - 1 < 0) return;
            _current--;
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Reset the CurrentToken to the first Token of the Iterator
        /// </summary>
>>>>>>> master
        public void Reset()
        {
            _current = -1;
        }
    }
}
