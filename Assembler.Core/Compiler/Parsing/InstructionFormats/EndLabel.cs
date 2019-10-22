using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;

namespace Assembler.Parsing.InstructionFormats
{
    /// <summary>
    /// Identifier for the end of a label.
    /// </summary>
    public class EndLabel : IFormatInstructions
    {
        /// <summary>
        /// Creates an EndLabel instance
        /// </summary>
        /// <param name="name">Name of the label</param>
        public EndLabel(Token name)
        {
            Name = new VariableName(name);
        }

        /// <summary>
        /// Getter for variable name
        /// </summary>
        public VariableName Name { get; }

        /// <summary>
        /// Null. End Label does not have an Operator
        /// </summary>
        public Token Operator => null;

        /// <summary>
        /// True if all the parameters are valid, False otherwise
        /// </summary>
        public bool IsValid => Name.IsValid();

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of EndLabel</returns>
        public override string ToString()
        {
            return $"EndLabel[name: {Name}, valid:'{IsValid}']";
        }
    }
}
