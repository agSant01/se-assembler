using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionFormats
{
    public class EndLabel : IFormatInstructions
    {
=======

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
>>>>>>> master
        public EndLabel(Token name)
        {
            Name = new VariableName(name);
        }

<<<<<<< HEAD
        public VariableName Name { get; }

        public Token Operator => null;

        public bool IsValid => Name.IsValid();

=======
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
>>>>>>> master
        public override string ToString()
        {
            return $"EndLabel[name: {Name}, valid:'{IsValid}']";
        }
    }
}
