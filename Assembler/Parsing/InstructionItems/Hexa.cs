using Assembler.Interfaces;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Text;
=======
>>>>>>> master
using System.Text.RegularExpressions;

namespace Assembler.Parsing.InstructionItems
{
<<<<<<< HEAD
    public class Hexa : IFormatUnit
    {

        private bool _isValid;

=======
    /// <summary>
    /// Identifier for a Hexadecimal instruction parameter
    /// </summary>
    public class Hexa : IFormatUnit
    {
        /// <summary>
        /// True if value is a valid Hexadecimal number
        /// </summary>
        private readonly bool _isValid;

        /// <summary>
        /// Creates a Hexa instance
        /// </summary>
        /// <param name="token">Token to be wrapped</param>
>>>>>>> master
        public Hexa(Token token)
        {
            Token = token;

            _isValid = new Regex(@"^[a-fA-F0-9]*$").IsMatch(token.Value);
        }

<<<<<<< HEAD
=======
        /// <summary>
        /// Converts an array of Tokens to an array of Hexa's
        /// </summary>
        /// <param name="values">Token array</param>
        /// <returns>array of Hexa's</returns>
>>>>>>> master
        public static Hexa[] ToArray(Token[] values)
        {
            Hexa[] hexas = new Hexa[values.Length];

            int idx = 0;
            foreach (Token t in values)
            {
                hexas[idx] = new Hexa(t);
            }

            return hexas;
        }

<<<<<<< HEAD
        public Token Token { get; }

        public bool IsValid() => _isValid;

=======
        /// <summary>
        /// Getter for Token
        /// </summary>
        public Token Token { get; }

        /// <summary>
        /// Getter for validity state
        /// </summary>
        /// <returns>True if a valid hexadecimal number, False otherwise</returns>
        public bool IsValid() => _isValid;

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of Hexa</returns>
>>>>>>> master
        public override string ToString()
        {
            return Token?.Value;
        }
    }
}
