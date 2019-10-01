using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parser
{
    public class Lexer
    {
        private HashSet<string> operators = new HashSet<string>();

        private string[] text;

        private Token[] tokens = new Token[10];
        private int sizeCounter = 0;

        private int _current = -1;

        public Lexer(string[] text)
        {
            this.text = text;
            Init();
            Start();
        }

        private void Init()
        {
            // Data movement
            operators.Add("JMP");
            operators.Add("LOAD");
            operators.Add("LOADIM");
            operators.Add("POP");
            operators.Add("STORE");
            operators.Add("PUSH");
            operators.Add("LOADRIND");
            operators.Add("STOREIND");

            // Arithmetic Operations
            operators.Add("ADD");
            operators.Add("SUB");
            operators.Add("ADDIM");
            operators.Add("SUBIM");

            //Logic operations
            operators.Add("OR");
            operators.Add("XOR");
            operators.Add("NOT");
            operators.Add("NEG");
            operators.Add("SHIFTR");
            operators.Add("SHIFTL");
            operators.Add("ROTAR");
            operators.Add("ROTAL");

            // Flow Control
            operators.Add("JMPRIND");
            operators.Add("JMPADDR");
            operators.Add("JCONDRIN");
            operators.Add("JCONDADDR");
            operators.Add("LOOP");
            operators.Add("GRT");
            operators.Add("GRTEQ");
            operators.Add("EQ");
            operators.Add("NEQ");
            operators.Add("NOP");
            operators.Add("CALL");
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
                        else
                            MakeToken(TokenType.WHITE_SPACE, char.ToString(charsInLine[rightIdx]));

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

            if (value.Contains("//"))
            {
                MakeToken(TokenType.LINE_COMMENT, value);
            }
            else if (operators.Contains(value))
            {
                MakeToken(TokenType.OPERATOR, value);
            } else if (isRegister(leftIdx, rightIdx, ref charsInLine))
            {
                MakeToken(TokenType.REGISTER, value);
            }
            else if (!string.IsNullOrEmpty(value))
            {
                MakeToken(TokenType.IDENTIFIER, value);
            }
        }

        private bool isRegister(int leftIdx, int rightIdx, ref char[] charsInLine)
        {
            return rightIdx - leftIdx + 1 == 2 &&
                (
                charsInLine[leftIdx] == 'R' || charsInLine[leftIdx] == 'r'
                );
        }

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

        public Token CurrrentToken {
            get {
                if (_current == -1)
                {
                    _current++;
                }
                return tokens[_current];
            }
        }

        public bool MoveNext()
        {
            if (_current + 1 >= sizeCounter) return false;
            _current++;
            return true;
        }
    }
}
