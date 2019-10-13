﻿using Assembler.Utils;
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

        /// <summary>
        /// Maximum value that can be stored in register
        /// </summary>
        private readonly sbyte maxValue;

        /// <summary>
        /// Minimum value that can be stored in the register
        /// </summary>
        private readonly sbyte minValue;

        public Registers(int numberOfRegisters = 8, byte registerBytes = 1)
        {
            registers = new sbyte[numberOfRegisters];
            registerByteSize = registerBytes;

            double N = Convert.ToDouble(registerByteSize * 8.0);

            maxValue = (sbyte)(Math.Pow(2.0, N - 1) - 1);

            minValue = (sbyte)(-1 * Math.Pow(2, N - 1));
        }

        /// <summary>
        /// Access contents in the registers
        /// </summary>
        /// <param name="registerNumber">Register Number</param>
        /// <exception cref="InvalidCastException">If the data is invalid for the size of the register</exception>
        /// <returns></returns>
        public string GetRegisterValue(byte registerNumber)
        {
            if (registerNumber < 1 || registerNumber > 7)
            {
                throw new IndexOutOfRangeException($"Invalid Register: {registerNumber}");
            }

            return UnitConverter.DecimalToHex(registers[registerNumber]).Replace("0x", "");
        }

        /// <summary>
        /// Set value to a designated Register
        /// </summary>
        /// <param name="registerNumber">the register Number</param>
        /// <param name="hexadecimalValue">Hexadecimal Value to store in the register</param>
        /// <exception cref="InvalidCastException">If the data is invalid for the size of the register</exception>
        public void SetRegisterValue(byte registerNumber, string hexadecimalValue)
        {
            if (registerNumber < 1 || registerNumber > 7)
            {
                throw new IndexOutOfRangeException($"Invalid Register: {registerNumber}");
            }

            IsValidData(hexadecimalValue);

            registers[registerNumber] = (sbyte) UnitConverter.HexToDecimal(hexadecimalValue);
        }

        /// <summary>
        /// Verifies if the value to store in the Register is supported size
        /// </summary>
        /// <param name="hexValue">Value (hexadecimal) to be evaluated</param>
        /// <exception cref="InvalidCastException">If the data is invalid for the size of the register</exception>
        private void IsValidData(string hexValue)
        {
            int decimalValue = UnitConverter.HexToDecimal(hexValue);

            if (decimalValue > maxValue || decimalValue < minValue)
            {
                throw new InvalidCastException($"The passed value '0x{hexValue}', " +
                    $"'Decimal:{decimalValue}' is invalid for this register of " +
                    $"{registerByteSize*8}-Bits.");
            }
        }
    }
}