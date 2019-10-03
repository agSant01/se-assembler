using Assembler.Parsing;

namespace Assembler.Interfaces
{
    /// <summary>
    /// Interface used for defining an Assembly Instruction.
    /// </summary>
    public interface IFormatInstructions
    {
        /// <summary>
        /// Getter for the instruction Operator token
        /// </summary>
        Token Operator { get; }

        /// <summary>
        /// Getter for the validity of the Instruction.
        /// Returns false if every parameter of the instruction is valid, 
        /// false otherwise.
        /// </summary>
        bool IsValid { get; }
    }
}
