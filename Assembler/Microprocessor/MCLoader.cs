using Assembler.Microprocessor.InstructionFormats;
using Assembler.Utils;
using System;

namespace Assembler.Microprocessor
{
    public class MCLoader
    {
        private readonly VirtualMemory _vm;

        private readonly MicroSimulator _simulator;

        public MCLoader(VirtualMemory virtualMemory, MicroSimulator simulator)
        {
            _vm = virtualMemory;

            _simulator = simulator;
        }

        public IMCInstruction RunAll()
        {
            while (true)
            {
                NextInstruction();
            }
        }

        public IMCInstruction NextInstruction()
        {
            ushort currProgramCounter = _simulator.ProgramCounter;

            // even block containing the instruction
            string evenBlock = _vm.GetContentsInBin(currProgramCounter);
            // odd block with data and other params
            string oddBlock = _vm.GetContentsInBin(currProgramCounter + 1);

            // 16-bit instruction block
            string completeBlock = evenBlock + oddBlock;

            // get first 5-bits to use as OpCode
            string opcode = evenBlock.Substring(0, 5);

            // instruction format
            byte instructionFormat = OpCodesInfo.GetInstructionFormat(opcode);
            byte numberOfParams = OpCodesInfo.GetNumberOfParams(opcode);

            byte count = 0;
            string[] paramList = new string[3];

            IMCInstruction instructionToExecute = null;

            switch (instructionFormat)
            {
                case 1:
                    while (count < numberOfParams)
                    {
                        paramList[count] = completeBlock.Substring(count * 3 + 6, 3);
                        count++;
                    }
                    
                    instructionToExecute = new MCInstructionF1(
                          decimalAddress: currProgramCounter,
                          opCodeBinary: opcode,
                          Ra: paramList[0],
                          Rb: paramList[1],
                          Rc: paramList[2]
                     );
                    break;

                case 2:
                    while (count < numberOfParams)
                    {
                        if (count == 0)
                        {
                            // get 'Ra'
                            paramList[count] = completeBlock.Substring(6, 3);
                        }
                        else if (count == 1)
                        {
                            paramList[count] = evenBlock;
                        }

                        count++;
                    }

                    instructionToExecute = new MCInstructionF2(
                          decimalAddress: currProgramCounter,
                          opCodeBinary: opcode,
                          Ra: paramList[0],
                          binaryAddress: paramList[1]
                    );

                    break;

                case 3:
                    instructionToExecute = new MCInstructionF3(
                          decimalAddress: currProgramCounter,
                          opCodeBinary: opcode,
                          binaryAddress: completeBlock.Substring(6, 10)
                    );
                    break;
            }

            // TODO: add executer
            InstructionSetExe.ExecuteInstruction(instructionToExecute, _simulator);

            return instructionToExecute;
        }
    }
}
