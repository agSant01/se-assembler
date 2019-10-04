namespace Assembler.Interfaces
{
    /// <summary>
    /// Used to define a type of parameters that is 
    /// used by the IFormatInstructions.
    /// </summary>
    public interface IFormatUnit
    {
        /// <summary>
        /// Used to determine if the value of the parameter is of valid type.
        /// </summary>
        /// <returns>True if the of this param value is valid for this format</returns>
        bool IsValid();
    }
}
