using Assembler.Interfaces;
using Assembler.Utils;

namespace Assembler.Parsing.InstructionItems
{
    /// <summary>
    /// Identifier for the instruction of a variable assign.
    /// </summary>
    public class VariableAssign : IFormatInstructions
    {
        /// <summary>
        /// Creates a VariableAssign instance
        /// </summary>
        /// <param name="op">Variable assign operator Token</param>
        /// <param name="name">(Optional) Name of the Variable. A variable can be assigned without a name</param>
        /// <param name="values">Array of values as Tokens</param>
        public VariableAssign(Token op, Token name, Token[] values)
        {
            Operator = op;
            Name = new VariableName(name);
            Values = Hexa.ToArray(values);
        }

        /// <summary>
        /// Getter for the Variable assign operator Token
        /// </summary>
        public Token Operator { get; }

        /// <summary>
        /// Getter for the variable name
        /// </summary>
        public VariableName Name { get; }

        /// <summary>
        /// Getter for Values Array
        /// </summary>
        public Hexa[] Values { get; }

        /// <summary>
        /// True if all the parameters are valid, False otherwise
        /// </summary>
        public bool IsValid
        {
            get
            {
                // if only one invalid hex return false
                foreach (Hexa hex in Values)
                {
                    if (hex == null) continue;

                    if (!hex.IsValid())
                        return false;
                }

                // variable name is optional

                return true;
            }
        }

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of VariableAssign</returns>
        public override string ToString()
        {
            return $"VariableAssign[op: {Operator.Value}, name: {Name}, values: {ArrayUtils.ArrayToString(Values)}, valid: '{IsValid}']";
        }
    }
}
