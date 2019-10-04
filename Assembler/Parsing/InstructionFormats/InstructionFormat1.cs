using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;

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
        {
            Operator = op;
            RegisterA = new Register(registerA);
            RegisterB = new Register(registerB);
            RegisterC = new Register(registerC);
        }

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
        public override string ToString()
        {
            return $"IF1[op: '{Operator.Value}', Ra: '{RegisterA}', Rb: '{RegisterB}', Rc: '{RegisterC}', valid: '{IsValid}']";
        }
    }
}
