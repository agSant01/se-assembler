using Assembler.Interfaces;
using System.Text.RegularExpressions;

namespace Assembler.Parsing.InstructionItems
{
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
        public Hexa(Token token)
        {
            Token = token;

            _isValid = new Regex(@"^[a-fA-F0-9]*$").IsMatch(token.Value);
        }

        /// <summary>
        /// Converts an array of Tokens to an array of Hexa's
        /// </summary>
        /// <param name="values">Token array</param>
        /// <returns>array of Hexa's</returns>
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
        public override string ToString()
        {
            return Token?.Value;
        }
    }
}
