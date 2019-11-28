using Assembler.Utils;
using System;

namespace Assembler.Microprocessor
{
    public class Registers
    {
        /// <summary>
        /// Registers representation. Data stored is represented in DecimalFormat.
        /// Make use of UnitConverter to change to other desired formats such
        /// as Binary or Hexadecimal
        /// </summary>
        private readonly sbyte[] registers;

        /// <summary>
        /// The size of each register in Bytes
        /// </summary>
        private readonly byte registerByteSize;
        public Registers(int numberOfRegisters = 8, byte registerBytes = 1)
        {
            registers = new sbyte[numberOfRegisters];
            registerByteSize = registerBytes;
        }

        /// <summary>
        /// Access contents in the registers in Hexadecimal format
        /// </summary>
        /// <param name="registerNumber">Register Number</param>
        /// <exception cref="InvalidCastException">If the data is invalid for the size of the register</exception>
        /// <returns>Contents of the registers in Hexadecimal format</returns>
        public string GetRegisterValue(byte registerNumber)
        {
            if (!IsValidAction(registerNumber, isWrite: false))
            {
                throw new IndexOutOfRangeException($"Read Action: Invalid Register '{registerNumber}'");
            }

            return UnitConverter.ByteToHex((byte)registers[registerNumber]).Replace("0x", "");
        }

        /// <summary>
        /// Set value to a designated Register
        /// </summary>
        /// <param name="registerNumber">the register Number</param>
        /// <param name="hexadecimalValue">Hexadecimal Value to store in the register</param>
        /// <exception cref="InvalidCastException">If the data is invalid for the size of the register</exception>
        public void SetRegisterValue(byte registerNumber, string hexadecimalValue)
        {
            if (!IsValidAction(registerNumber, isWrite: true))
            {
                throw new IndexOutOfRangeException($"Write Action: Invalid Register '{registerNumber}'");
            }

            IsValidData(hexadecimalValue);

            registers[registerNumber] = UnitConverter.HexToSByte(hexadecimalValue);
        }

        /// <summary>
        /// Verifies if the value to store in the Register is supported size
        /// </summary>
        /// <param name="hexValue">Value (hexadecimal) to be evaluated</param>
        /// <exception cref="InvalidCastException">If the data is invalid for the size of the register</exception>
        private void IsValidData(string hexValue)
        {
            try
            {
                sbyte decimalValue = UnitConverter.HexToSByte(hexValue);
            }
            catch (OverflowException)
            {
                throw new OverflowException($"The passed value '0x{hexValue}', " +
                       $"'{UnitConverter.HexToBinary(hexValue)}' is invalid for this register of " +
                       $"{registerByteSize * 8}-Bits.");
            }
        }

        /// <summary>
        /// Validates if the action can be performed on the register
        /// </summary>
        /// <returns>True if action can be performed, false otherwise.</returns>
        private bool IsValidAction(byte registerNumber, bool isWrite)
        {
            if (isWrite)
            {
                // can only write from R1-R7
                return registerNumber > 0 && registerNumber <= 7;
            }

            // can read from R0-R7
            return registerNumber >= 0 && registerNumber <= 7;
        }

        public override string ToString()
        {
            return $"Registers{ArrayUtils.ArrayToString(registers)}"; ;
        }
    }
}
