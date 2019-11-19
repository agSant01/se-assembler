using Assembler.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.Microprocessor.IO.IODevices
{
    public class ASCII_Display : IIODevice
    {
        private readonly bool _debug;

        public short IOPortLength => 8;//bytes

        public string[] DisplaySlots = new string[] { "", "", "", "", "", "", "", "" };

        public bool HasData { get; private set; }

        public short IOPort { get; }

        public Action GotHexData;
        public string DeviceName => "ASCII Display";

        public ASCII_Display(short port)
        {
            if (port < 0)
                //port *= -1;// we flip it to positive
                throw new ArgumentOutOfRangeException("Provided a negative port number!\n");

            if (port == short.MaxValue || port + 7 > short.MaxValue)
                throw new ArgumentOutOfRangeException("Provided an overflowable port number!\n");

            this.IOPort = port;
            this._debug = false;
        }

        public ASCII_Display(short port, bool deb)
        {
            if (port < 0)
                //port *= -1;// we flip it to positive
                throw new ArgumentOutOfRangeException("Provided a negative port number!\n");

            if (port == short.MaxValue || port + 7 > short.MaxValue)
                throw new ArgumentOutOfRangeException("Provided an overflowable port number!\n");

            this.IOPort = port;
            this._debug = deb;
        }


        /// <summary>
        /// Verify the validity of the port being provided
        /// </summary>
        /// <param name="port">Port whose internal validity we wish to check</param>
        /// <returns>Boolean representation of the port's validity</returns>
        private bool IsValidPort(int port)
        {
            return port >= IOPort &&
                   port < IOPort + IOPortLength;
        }
        
        /// <summary>
        /// Clears all the data from the internal array of characters.
        /// </summary>
        /// <returns> Boolean representing that the reset was successful</returns>
        public bool Reset()
        {
            DisplaySlots = new string[] { "", "", "", "", "", "", "", "" };

            if (!_debug)
                GotHexData();
            return true;
        }

        /// <summary>
        /// Returns index corresponding to appropriate ASCII Display port number.
        /// </summary>
        /// <param name="port"> The port whose data we want to retrieve.</param>
        /// <returns>Integer representing the internal index used to store the values.</returns>
        private int ConvertPortToIndex(short port)
        {
            if (!IsValidPort(port))
                throw new ArgumentException($"Invalid port \n");

            if (IsValidPort(port) & IOPort == 0)
            {
                return port;//something here
            }

            else
            {
                return port % IOPort;//BUG HERE WITH DIVISION BY 0
            }
        }

        /// <summary>
        /// Method to retrieve the internal representation of the ASCII Display
        /// </summary>
        /// <returns> String representation of the ASCII Display's data</returns>
        public override string ToString()
        {
            return $"ASCII_Display[port: {IOPort} , characters: [{String.Join(", ", DisplaySlots)}]]";
        }

        /// <summary>
        /// Method for writing data to the internal ports of the ASCII Display
        /// </summary>
        /// <param name="port">Integer representation of the </param>
        /// <param name="contentInHex"></param>
        /// <returns>Boolean representing if the writet operation was successful</returns>
        public bool WriteInPort(int port, string contentInHex)//FUCK THIS MIGHT BE WRONG DUE TO NEGATIVE NUMBERS, NEED TO FIX
        {
            if (!IsValidPort(port))
                return false;

            byte[] binary = new byte[] { UnitConverter.HexToByte(contentInHex) };

            DisplaySlots[port - IOPort] = Encoding.ASCII.GetString(binary);

            if (!_debug)
                GotHexData();

            return true;
        }

        /// <summary>
        /// Reading the data contained in the ASCII Display's port.
        /// </summary>
        /// <param name="port">Port whose data we wish to read.</param>
        /// <returns>String representation of the data stored in the port.</returns>
        public string ReadFromPort(int port)
        {
            if (IsValidPort(port))
            {
                if (DisplaySlots[ConvertPortToIndex((short)port)].Length > 0)
                {
                    char c = DisplaySlots[ConvertPortToIndex((short)port)].ToCharArray()[0];
                    return UnitConverter.ByteToHex((byte)c);
                }
                else
                    return UnitConverter.ByteToHex((byte)0);
            }
            else
            {
                throw new ArgumentException($"Invalid port \n");
            }
        }
    }
}
