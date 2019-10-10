using Assembler.Interfaces;
using System.Text.RegularExpressions;

namespace Assembler.Parsing.InstructionItems
{
    public class VariableName : IFormatUnit
    {
        /// <summary>
        /// True if value is a valid variable name
        /// </summary>
        private readonly bool _isValid;

        /// <summary>
        /// Creates a VariableName instance
        /// </summary>
        /// <param name="token">Variable name Token</param>
        public VariableName(Token token)
        {
            Token = token;

            _isValid = new Regex(@"^[a-zA-Z0-9#]*$").IsMatch(token.Value);
        }

        /// <summary>
        /// Getter for Variable name Token
        /// </summary>
        public Token Token { get; }

        /// <summary>
        /// Getter for validity state
        /// </summary>
        /// <returns>True if a valid variable name, False otherwise</returns>
        public bool IsValid() => _isValid;

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of VariableName</returns>
        public override string ToString()
        {
            return Token?.Value;
        }
    }
}
