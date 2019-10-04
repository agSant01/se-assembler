using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionFormats
{
    public class InstructionFormat2 : IFormatInstructions
    {

=======

namespace Assembler.Parsing.InstructionFormats
{
    /// <summary>
    /// Identifier for an Instruction of Format 2.
    /// </summary>
    public class InstructionFormat2 : IFormatInstructions
    {
        /// <summary>
        /// Creates an InstructionFormat2 instance
        /// </summary>
        /// <param name="opcode">Operator of the Instruction</param>
        /// <param name="registerA">Register used as RegisterA</param>
        /// <param name="constOrAddress">Address of memory or constant name</param>
>>>>>>> master
        public InstructionFormat2(Token opcode, Token registerA, Token constOrAddress)
        {
            Operator = opcode;
            if (opcode.Value.ToLower().Equals("store"))
            {
                RegisterA = new Register(constOrAddress);
                ConstOrAddress = new VariableName(registerA);
            }
            else
            {
                RegisterA = new Register(registerA);
                ConstOrAddress = new VariableName(constOrAddress);
            }
        }

<<<<<<< HEAD
        public Token Operator { get; }

        public Register RegisterA { get; }

        public VariableName ConstOrAddress { get; }

        public bool IsValid => RegisterA.IsValid() && ConstOrAddress.IsValid();
        
=======
        /// <summary>
        /// Getter for the instruction Operator token
        /// </summary>
        public Token Operator { get; }

        /// <summary>
        /// Getter for RegisterA
        /// </summary>
        public Register RegisterA { get; }

        /// <summary>
        /// Getter for the address of memory or constant name
        /// </summary>
        public VariableName ConstOrAddress { get; }

        /// <summary>
        /// True if all the parameters are valid, False otherwise
        /// </summary>
        public bool IsValid => RegisterA.IsValid() && ConstOrAddress.IsValid();

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of InstructionFormat2</returns>
>>>>>>> master
        public override string ToString()
        {
            return $"IF2[op: '{Operator.Value}', Ra: '{RegisterA}', Const/Address: '{ConstOrAddress}', valid: '{IsValid}']";
        }
    }
}
