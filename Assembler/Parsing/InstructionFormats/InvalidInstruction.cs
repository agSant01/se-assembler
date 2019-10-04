<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Text;
using Assembler.Interfaces;

namespace Assembler.Parsing.InstructionFormats
{
    class InvalidInstruction : IFormatInstructions
    {

=======
﻿using Assembler.Interfaces;
using Assembler.Utils;

namespace Assembler.Parsing.InstructionFormats
{
    /// <summary>
    /// Identifier for an Invalid Instruction.
    /// </summary>
    class InvalidInstruction : IFormatInstructions
    {
        /// <summary>
        /// Creates an InvalidInstruction instance.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="parms"></param>
>>>>>>> master
        public InvalidInstruction(Token op, Token[] parms)
        {
            Operator = op;
            Values = parms;
        }

<<<<<<< HEAD
        public Token Operator { get; }

        public Token[] Values { get; }

        public bool IsValid => false;

        public override string ToString()
        {
            return $"InvalidInstruction[op: {Operator.Value}, values: {ArrayToString(Values)}, valid: '{IsValid}']";
        }

        private string ArrayToString(object[] arr)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("[");

            builder.AppendJoin(",", arr);

            builder.Append("]");

            return builder.ToString();
=======
        /// <summary>
        /// Getter for instruction Operator token
        /// </summary>
        public Token Operator { get; }

        /// <summary>
        /// Getter for array of parameter tokens
        /// </summary>
        public Token[] Values { get; }

        /// <summary>
        /// Returns False. Always invalid
        /// </summary>
        public bool IsValid => false;

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of InvalidInstruction</returns>
        public override string ToString()
        {
            return $"InvalidInstruction[op: {Operator.Value}, values: {ArrayUtils.ArrayToString(Values)}, valid: '{IsValid}']";
>>>>>>> master
        }
    }
}
