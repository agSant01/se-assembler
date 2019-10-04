using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionFormats
{
    public class Label : IFormatInstructions
    {
=======

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
>>>>>>> master
        public Label(Token name)
        {
            Name = new VariableName(name);
        }

<<<<<<< HEAD
        public VariableName Name { get; }

        public Token Operator => null;

        public bool IsValid => Name.IsValid();

=======
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
>>>>>>> master
        public override string ToString()
        {
            return $"Label[name: {Name}, valid: '{IsValid}']";
        }
    }
}
