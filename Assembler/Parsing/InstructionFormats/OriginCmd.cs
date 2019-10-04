using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;

namespace Assembler.Parsing.InstructionFormats
{
    /// <summary>
    /// Identifier for the Origin Command.
    /// </summary>
    class OriginCmd : IFormatInstructions
    {
        /// <summary>
        /// Creates an OriginCmd instance
        /// </summary>
        /// <param name="op">ORG operator token</param>
        /// <param name="address">Address</param>
        public OriginCmd(Token op, Token address)
        {
            Operator = op;
            Address = new Hexa(address);
        }

        /// <summary>
        /// Getter for the ORG Operator token
        /// </summary>
        public Token Operator { get; }

        /// <summary>
        /// Getter for the address
        /// </summary>
        public Hexa Address { get; }

        /// <summary>
        /// True if address is valid, False otherwise
        /// </summary>
        public bool IsValid => Address.IsValid();

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of OriginCmd</returns>
        public override string ToString()
        {
            return $"ORIGIN[op: '{Operator.Value}', address: '{Address}', valid: '{IsValid}']";
        }
    }
}
