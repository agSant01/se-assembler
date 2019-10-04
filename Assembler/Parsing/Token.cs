
namespace Assembler.Parsing
{
    /// <summary>
    /// Representation of a token. Stores value and Token type.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Creates a Token instance
        /// </summary>
        /// <param name="type">Token type</param>
        /// <param name="value">value of the token</param>
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Getter for the Token value
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Getter for the token type
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// ToString Override
        /// </summary>
        /// <returns>String representation of VariableName</returns>
        public override string ToString()
        {
            return $"Token(type: {Type.ToString()}, val: '{Value}')";
        }

        /// <summary>
        /// GetHashCode Override
        /// </summary>
        /// <returns>Base HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Equals Override
        /// </summary>
        /// <returns>Equals of this Token and target token contain the same value and type</returns>
        public override bool Equals(object obj)
        {
            if (!this.GetType().Equals(obj.GetType()))
                return false;

            Token tok_obj = (Token)obj;

            return this.Type.Equals(tok_obj.Type)
                && this.Value.Equals(tok_obj.Value);
        }
    }
}
