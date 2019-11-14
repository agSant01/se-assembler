namespace Assembler.Microprocessor.InstructionFormats
{
    public interface IMCInstruction
    {
        byte OpCode { get; }

        ushort InstructionAddressDecimal { get; }

        public static bool AsmTextPrint { get; set; } = false;
    }
}
