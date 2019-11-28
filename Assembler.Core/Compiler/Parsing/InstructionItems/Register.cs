using Assembler.Interfaces;
using System;

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
        private bool _isValid;

        /// <summary>
        /// Creates a Register instance
        /// </summary>
        /// <param name="token">Token of Register</param>
        public Register(Token token)
        {
            Token = token;

            if (Token == null)
            {
                _isValid = true;
            }
            else
            {
                string numericPart = Token.Value.Trim().ToLower().Replace("r", "");

                if (int.TryParse(numericPart, out int registerNumber))
                {
                    // registerNumber grater than 7 or less than 1
                    if (registerNumber > 7 || registerNumber < 0)
                        _isValid = false;
                    else
                        _isValid = true;
                } else
                {
                    _isValid = false;
                }
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
        public override string ToString()
        {
            return Token?.Value ?? "";
        }
    }
}
