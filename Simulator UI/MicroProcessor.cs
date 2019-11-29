using Assembler.Microprocessor;
using Assembler;
using Assembler.Core.Microprocessor;
using Assembler.Parsing;
using Assembler.Assembler;
using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;

namespace Simulator_UI
{
    class MicroProcessor
    {
        public AssemblyLogger AssemblyLogger { get; private set; }
        
        public VirtualMemory VirtualMemory { get; set; }

        public IOManager IoManager;
        
        public MicroSimulator Micro { get; private set; }

        public string[] OBJFileLines { get; private set; }

        public string GetPerviousInstruction => GetPrettyInstruction(Micro?.PreviousInstruction);

        public string GetCurrentInstruction => GetPrettyInstruction(Micro?.CurrentInstruction);

        public string GetNextInstruction => GetPrettyInstruction(Micro?.PeekNextInstruction());

        public bool StopRun { get; set; } = true;

        public void InitializeMicroASM(string[] asmCodeLines, string asmFileName)
        {
            Clear();

            if (asmFileName == null)
            {
                AssemblyLogger = new AssemblyLogger("Assembly");
            } else
            {
                AssemblyLogger = new AssemblyLogger(asmFileName);
            }


            Lexer lexer = new Lexer(asmCodeLines);

            Parser parser = new Parser(lexer);

            Compiler compiler = new Compiler(parser, AssemblyLogger);

            compiler.Compile();

            OBJFileLines = compiler.GetOutput();

            //Micro simulator setup
            VirtualMemory = new VirtualMemory(OBJFileLines);

            // state the last port for the micro
            IoManager = new IOManager(VirtualMemory.VirtualMemorySize - 1);

            Micro = new MicroSimulator(VirtualMemory, IoManager);
        }

        public void InitializeMicroOBJ(string[] objFileLines)
        {
            Clear();

            OBJFileLines = objFileLines;

            //Micro simulator setup
            VirtualMemory = new VirtualMemory(OBJFileLines);

            // state the last port for the micro
            IoManager = new IOManager(VirtualMemory.VirtualMemorySize - 1);

            Micro = new MicroSimulator(VirtualMemory, IoManager);
        }

        public bool Reset()
        {
            if (!IsOn())
                return false;

            StopRun = true;

            VirtualMemory = new VirtualMemory(OBJFileLines);

            IoManager?.ResetIOs();

            Micro = new MicroSimulator(VirtualMemory, IoManager);

            return true;
        }

        private string GetPrettyInstruction(IMCInstruction instruction)
        {
            if (instruction == null)
            {
                return "";
            }

            string addressHex = UnitConverter.IntToHex(instruction.InstructionAddressDecimal, defaultWidth: 3);

            string contentHex = VirtualMemory.GetContentsInHex(instruction.InstructionAddressDecimal) +
                VirtualMemory.GetContentsInHex(instruction.InstructionAddressDecimal + 1);

            return $"{addressHex}: {contentHex}: {instruction.ToString()}";
        }

        public bool IsOn()
        {
            return Micro != null;
        }

        public void Clear()
        {
            StopRun = true;
            VirtualMemory = null;
            IoManager = null;
            Micro = null;
            OBJFileLines = null;
            AssemblyLogger = null;
        }
    }
}
