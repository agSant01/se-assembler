using Assembler.Interfaces;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionItems
{
    public class Register : IFormatUnit
    {
        public Register(Token token)
        {
            Token = token;
        }

        public Token Token { get; }

        public bool IsValid()
        {
            if (Token == null) return true;

            if (!char.IsDigit(Token.Value[1])) return false;


            int registerNumber = (int)char.GetNumericValue(Token.Value[1]);

            if (registerNumber > 7 || registerNumber < 1)
                return false;

            return true;
        }

=======

namespace Assembler.Parsing.InstructionItems
{
    /// <summary>
    /// Identifier fo
    /// </summary>
    public class Register : IFormatUnit
    {
        /// <summary>
        /// True if value is a valid register
        /// </summary>
        private readonly bool _isValid;

        /// <summary>
        /// Creates a Register instance
        /// </summary>
        /// <param name="token">Token of Register</param>
        public Register(Token token)
        {
            Token = token;

            if (Token == null)
                _isValid = true;
            else if (char.IsDigit(Token.Value[1]))
            {
                int registerNumber = (int)char.GetNumericValue(Token.Value[1]);

                // registerNumber grater than 7 or less than 1
                if (registerNumber > 7 || registerNumber < 1)
                    _isValid = false;
                else
                    _isValid = true;
            }
            else
            {
                _isValid = false;
            }
        }

        /// <summary>
        /// Getter for Register Token
        /// </summary>
        public Token Token { get; }

        /// <summary>
        /// Getter for validity state
        /// </summary>
        /// <returns>True if a valid Register, False otherwise</returns>
        public bool IsValid() => _isValid;

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of Register</returns>
>>>>>>> master
        public override string ToString()
        {
            return Token?.Value;
        }
    }
}
