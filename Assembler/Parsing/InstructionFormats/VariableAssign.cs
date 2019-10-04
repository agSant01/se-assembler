using Assembler.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Parsing.InstructionItems
{
    public class VariableAssign : IFormatInstructions
    {
        public VariableAssign(Token op, Token name, Token[] values)
        {
            Operator = op;
            Name = new VariableName(name);
            Values = Hexa.ToArray(values);
        }

        public Token Operator { get; }

        public VariableName Name { get; }

        public Hexa[] Values { get; }

        public bool IsValid
        {
            get
            {
                // if only one invalid hex return false
                foreach(Hexa hex in Values)
                {
                    if (hex == null) continue;

                    if (!hex.IsValid())
                        return false;
                }

                // variable name is optional

                return true;
            }
        }

        public override string ToString()
        {
            return $"VariableAssign[op: {Operator.Value}, name: {Name}, values: {ArrayToString(Values)}, valid: '{IsValid}']";
        }

        private string ArrayToString(object[] arr)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("[");

            builder.AppendJoin(",", arr);

            builder.Append("]");

            return builder.ToString();
        }
    }
}
