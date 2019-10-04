using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;

namespace Assembler.Parsing.InstructionFormats
{
<<<<<<< HEAD
    public class InstructionFormat3 : IFormatInstructions
    {

=======
    /// <summary>
    /// Identifier for an Instruction of Format 3.
    /// </summary>
    public class InstructionFormat3 : IFormatInstructions
    {
        /// <summary>
        /// Creates an InstructionFormat2 instance
        /// </summary>
        /// <param name="op">Operator if the Instruction</param>
        /// <param name="constOrAddress">Address of memory or constant name</param>
>>>>>>> master
        public InstructionFormat3(Token op, Token constOrAddress)
        {
            Operator = op;
            ConstOrAddress = new VariableName(constOrAddress);
        }

<<<<<<< HEAD
        public Token Operator { get; }

        public VariableName ConstOrAddress { get; }

        public bool IsValid => ConstOrAddress.IsValid();

=======
        /// <summary>
        /// Getter for the instruction Operator token
        /// </summary>
        public Token Operator { get; }

        /// <summary>
        /// Getter for the address of memory or constant name
        /// </summary>
        public VariableName ConstOrAddress { get; }

        /// <summary>
        /// True if all the parameters are valid, False otherwise
        /// </summary>
        public bool IsValid => ConstOrAddress.IsValid();

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of InstructionFormat3</returns>
>>>>>>> master
        public override string ToString()
        {
            return $"IF3[op: '{Operator.Value}', Const/Address: '{ConstOrAddress}', valid: '{IsValid}']";
        }
    }
}
