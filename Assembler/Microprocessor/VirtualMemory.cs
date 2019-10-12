using Assembler.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Microprocessor
{

    public class VirtualMemory
    {
        private string[] memoryBlocksInHexadecimal;
        private ushort lastUsedAddressDecimal = 0;
        private HashSet<ushort> addressesUsed = new HashSet<ushort>();

        public VirtualMemory(string[] lines, int kiloBytes = 4)
        {
            memoryBlocksInHexadecimal = new string[kiloBytes * 1024];
            
            // blockBitSize / (4bits/1hex)
            int requiredHexaChars = 16 / 4;

            // save lines to memory
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Replace(" ", "");

                if (line.Length != requiredHexaChars && line.Length > 0)
                    throw new OverflowException($"Writing of memory exection. Invalid block size: {line.Length}");
               
                memoryBlocksInHexadecimal[i*2] = $"{line[0]}{line[1]}";

                memoryBlocksInHexadecimal[i*2+1] = $"{line[2]}{line[3]}";

                if (lastUsedAddressDecimal < i)
                {
                    lastUsedAddressDecimal = (ushort) i;
                }

                addressesUsed.Add((ushort) (i * 2));
                addressesUsed.Add((ushort) (i * 2 + 1));
            }
        }

        /// <summary>
        /// A method for retrieving the contents from provided address.
        /// Throws an exception if provided address is not found within current VirtualMemory instance.
        /// </summary>
        /// <param name="decimalAddress">Int representing the location in memory to retrieve contents from</param>
        /// <exception cref="IndexOutOfRangeException">If invalid address</exception>
        /// <returns>string representation of contents in current VirtualMemory instance (if found).</returns>
        public string GetContentsInHex(int decimalAddress)
        {
            IsValidAddress(decimalAddress);

            return memoryBlocksInHexadecimal[decimalAddress];
        }

        /// <summary>
        /// A method for retrieving the contents from provided address.
        /// Throws an exception if provided address is not found within current VirtualMemory instance.
        /// </summary>
        /// <param name="hexAddress">The address (hexadecimal) to read from memory</param>
        /// <exception cref="IndexOutOfRangeException">If invalid address</exception>
        /// <returns>string representation of contents in current VirtualMemory in Hexadecimal.</returns>
        public string GetContentsInHex(string hexAddress)
        {
            int decimalAddress = UnitsConverter.HexToDecimal(hexAddress);

            return GetContentsInHex(decimalAddress);
        }

        /// <summary>
        /// A method for manipulating the contents of current VirtualMemory instance.
        /// </summary>
        /// <param name="decimalAddress">The address (decimal) into which to write the new contents.</param>
        /// <exception cref="IndexOutOfRangeException">If invalid address</exception>
        /// <param name="hexContent"> The new contents (hexadecimal) to be added into internal memory structure.</param>
        public void SetContentInMemory(int decimalAddress, string hexContent)
        {
            IsValidAddress(decimalAddress);

            if (lastUsedAddressDecimal < decimalAddress)
            {
                lastUsedAddressDecimal = (ushort) decimalAddress;
            }

            memoryBlocksInHexadecimal[decimalAddress] = hexContent;
        }

        /// <summary>
        /// A method for manipulating the contents of current VirtualMemory instance.
        /// </summary>
        /// <param name="hexAddress">The address (hexadecimal) into which to write the new contents.</param>
        /// <exception cref="IndexOutOfRangeException">If invalid address</exception>
        /// <param name="hexContent"> The new contents (hexadecimal) to be added into internal memory structure.</param>
        public void SetContentInMemory(string hexAddress, string hexContent)
        {
            int decimalAddress = UnitsConverter.HexToDecimal(hexAddress);

            SetContentInMemory(decimalAddress, hexContent);
        }

        /// <summary>
        /// Tells if a memory address has been proviously used.
        /// </summary>
        /// <param name="decimalAddress">Address (decimal) to know if contains data</param>
        /// <returns>True is previously used, False otherwise</returns>
        public bool IsInUse(int decimalAddress)
        {
            return addressesUsed.Contains((ushort) decimalAddress);
        }

        /// <summary>
        /// Tells if a memory address has been proviously used.
        /// </summary>
        /// <param name="hexAddress">Address (hexadecimal) to know if contains data</param>
        /// <returns>True is previously used, False otherwise</returns>
        public bool IsInUse(string hexAddress)
        {
            return IsInUse(UnitsConverter.HexToDecimal(hexAddress));
        }

        /// <summary>
        /// Amount of addresse blocks used in memory
        /// </summary>
        /// <returns>The amount of addresses already in use.</returns>
        public int AddressesUsed()
        {
            return addressesUsed.Count;
        }

        /// <summary>
        /// A string representation of the current instance of the VirtualMemory class
        /// </summary>
        /// <returns> string representation of the current instance of the Virtual Memory</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("VirtualMemory[");
            for(int i = 0; i < lastUsedAddressDecimal; i+=2)
            {
                builder.Append("\t");

                builder.Append(memoryBlocksInHexadecimal[i]);
                builder.Append(" ");
                builder.Append(memoryBlocksInHexadecimal[i+1]);

                builder.Append("\n");
            }

            builder.AppendLine("]");

            return builder.ToString();

        }

        /// <summary>
        /// Validates memory address
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">If index is out of bounds</exception>
        /// <param name="decimalAddress">Decimal Memory Address</param>
        private void IsValidAddress(int decimalAddress)
        {
            if (decimalAddress < 0 || decimalAddress >= memoryBlocksInHexadecimal.Length)
            {
                throw new IndexOutOfRangeException($"Invalid address: {UnitsConverter.DecimalToHex(decimalAddress)}, Decimal[{decimalAddress}]");
            }
        }
    }
}
