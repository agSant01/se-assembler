namespace Assembler.Microprocessor
{
    public class MicroSimulator
    {
        private readonly MCLoader _mcLoader;

        public MicroSimulator(VirtualMemory virtualMemory)
        {
            MicroVirtualMemory = virtualMemory;

            MicroRegisters = new Registers();

            _mcLoader = new MCLoader(virtualMemory, this);
        }

        public Registers MicroRegisters { get; }

        public VirtualMemory MicroVirtualMemory { get; }

        public ushort ProgramCounter { get; set; }
    }
}
