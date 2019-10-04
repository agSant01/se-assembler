using Assembler.Interfaces;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionItems
{
    public class ConstantAssign : IFormatInstructions
    {
=======

namespace Assembler.Parsing.InstructionItems
{
    /// <summary>
    /// Identifier for the instruction of assigning a constant value.
    /// </summary>
    public class ConstantAssign : IFormatInstructions
    {
        /// <summary>
        /// Creates a ConstantAssign instantance
        /// </summary>
        /// <param name="op">Operator of the instruction</param>
        /// <param name="name">Name of the constant.</param>
        /// <param name="value">Value of the constant,in Hexadecimal format.</param>
>>>>>>> master
        public ConstantAssign(Token op, Token name, Token value)
        {
            Operator = op;
            Name = new VariableName(name);
            Value = new Hexa(value);
        }

<<<<<<< HEAD
        public Token Operator { get; }

        public VariableName Name { get; }

        public Hexa Value { get; }

        public bool IsValid => Name.IsValid() && Value.IsValid();

=======
        /// <summary>
        /// Getter for the instruction Operator token
        /// </summary>
        public Token Operator { get; }

        /// <summary>
        /// Getter for the name of the constant
        /// </summary>
        public VariableName Name { get; }

        /// <summary>
        /// Getter for the value of the constant
        /// </summary>
        public Hexa Value { get; }

        /// <summary>
        /// True if all the parameters are valid, False otherwise
        /// </summary>
        public bool IsValid => Name.IsValid() && Value.IsValid();

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of ConstantAssign</returns>
>>>>>>> master
        public override string ToString()
        {
            return $"ConstantAssign[op:{Operator.Value}, name: {Name}, value: {Value}, valid: '{IsValid}']";
        }
    }
}
