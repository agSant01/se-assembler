using Assembler.Interfaces;
using Assembler.Parsing.InstructionItems;

namespace Assembler.Parsing.InstructionFormats
{
    public class InstructionFormat3 : IFormatInstructions
    {

        public InstructionFormat3(Token op, Token constOrAddress)
        {
            Operator = op;
            ConstOrAddress = new VariableName(constOrAddress);
        }

        public Token Operator { get; }

        public VariableName ConstOrAddress { get; }

        public bool IsValid => ConstOrAddress.IsValid();

        public override string ToString()
        {
            return $"IF3[op: '{Operator.Value}', Const/Address: '{ConstOrAddress}', valid: '{IsValid}']";
        }
    }
}
