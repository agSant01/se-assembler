using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;

namespace Assembler.Parsing.InstructionFormats
{
    /// <summary>
    /// Identifier for a label
    /// </summary>
    public class Label : IFormatInstructions
    {
        /// <summary>
        /// Creates a Label instance
        /// </summary>
        /// <param name="name">Label name</param>
        public Label(Token name)
        {
            Name = new VariableName(name);
        }

        /// <summary>
        /// Getter for the label name
        /// </summary>
        public VariableName Name { get; }

        /// <summary>
        /// Label does not contain an operator. This value is: null
        /// </summary>
        public Token Operator => null;

        /// <summary>
        /// True if the name is valid, False otherwise
        /// </summary>
        public bool IsValid => Name.IsValid();

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of Label</returns>
        public override string ToString()
        {
            return $"Label[name: {Name}, valid: '{IsValid}']";
        }
    }
}
