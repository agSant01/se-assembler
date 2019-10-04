using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionFormats
{
    public class InstructionFormat1 : IFormatInstructions
    {

        public InstructionFormat1(Token op, Token registerA, Token registerB, Token  registerC)
=======

namespace Assembler.Parsing.InstructionFormats
{
    /// <summary>
    /// Identifier for an Instruction of Format 1.
    /// </summary>
    public class InstructionFormat1 : IFormatInstructions
    {
        /// <summary>
        /// Creates an InstructionFormat1 instance
        /// </summary>
        /// <param name="op">Operator of the Instruction</param>
        /// <param name="registerA">Register used as RegisterA</param>
        /// <param name="registerB">Register used as RegisterB</param>
        /// <param name="registerC">Register used as RegisterC</param>
        public InstructionFormat1(Token op, Token registerA, Token registerB, Token registerC)
>>>>>>> master
        {
            Operator = op;
            RegisterA = new Register(registerA);
            RegisterB = new Register(registerB);
            RegisterC = new Register(registerC);
        }

<<<<<<< HEAD
        public Token Operator { get; }

        public Register RegisterA { get; }

        public Register RegisterB { get; }

        public Register RegisterC { get; }

        public bool IsValid => RegisterA.IsValid() && RegisterB.IsValid() && RegisterC.IsValid();

=======
        /// <summary>
        /// Getter for the instruction Operator token
        /// </summary>
        public Token Operator { get; }

        /// <summary>
        /// Getter for the RegisterA
        /// </summary>
        public Register RegisterA { get; }

        /// <summary>
        /// Getter for the RegisterB
        /// </summary>
        public Register RegisterB { get; }

        /// <summary>
        /// Getter for the RegisterC
        /// </summary>
        public Register RegisterC { get; }

        /// <summary>
        /// True if all the parameters are valid, False otherwise
        /// </summary>
        public bool IsValid => RegisterA.IsValid() &&
            RegisterB.IsValid() &&
            RegisterC.IsValid();

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of InstructionFormat1</returns>
>>>>>>> master
        public override string ToString()
        {
            return $"IF1[op: '{Operator.Value}', Ra: '{RegisterA}', Rb: '{RegisterB}', Rc: '{RegisterC}', valid: '{IsValid}']";
        }
    }
}
