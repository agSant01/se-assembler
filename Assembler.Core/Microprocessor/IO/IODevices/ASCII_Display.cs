using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO;
using Assembler.Microprocessor;
using Assembler.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.Microprocessor.IO.IODevices
{
    public class ASCII_Display : IIODevice
    {

        private string[] characters;

        private Queue<string> _buffer = new Queue<string>();

        public short IOPortLength => 8;//bytes

        public bool HasData => characters.Length > 0;

        public byte BufferSize => (byte)characters.Length;

        public short IOPort {get;}

        public string DeviceName => "ASCII Display";

        public ASCII_Display(short port)
        {
            if (port < 0 )
                //port *= -1;// we flip it to positive
                throw new ArgumentOutOfRangeException("Provided a negative port number!\n");

            if (port == short.MaxValue || port + 7 > short.MaxValue)
                throw new ArgumentOutOfRangeException("Provided an overflowable port number!\n");

            this.IOPort = port;
            this.characters = new string[8];
        }


        /// <summary>
        /// Verify the validity of the port being provided
        /// </summary>
        /// <param name="port">Port whose internal validity we wish to check</param>
        /// <returns>Boolean representation of the port's validity</returns>
        private bool IsValidPort(int port)//TEST THIS
        {
            if (port == IOPort || port == IOPort + 1  ||
                port == IOPort + 2 || port == IOPort + 3 ||
                port == IOPort + 4 || port==IOPort + 5 ||
                port == IOPort + 6 || port == IOPort + 7)
                return true;

            return false;
        }

        /// <summary>
        /// Reads all data stored in ASCII Display
        /// </summary>
        /// <param name="port">A valid port used by the ASCII Display</param>
        /// <returns>String array representing all the data in the ASCII Display</returns>
        public string[] ReadAllFromPort(int port)
        {
            if (!IsValidPort(port))
                throw new ArgumentException($"Invalid port \n");
            return this.characters;
        }

        /// <summary>
        /// Clears all the data from the internal array of characters.
        /// </summary>
        /// <returns> Boolean representing that the reset was successful</returns>
        public bool Reset()
        {
            //_buffer.Clear();
            this.characters = new string[8];
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

            else
            {
                return port % IOPort;
            }
        }

        /// <summary>
        /// Method to retrieve the internal representation of the ASCII Display
        /// </summary>
        /// <returns> String representation of the ASCII Display's data</returns>
        public override string ToString()
        {
            return $"ASCII_Display[port: {IOPort} , characters: {String.Join(", ",characters)}]";
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

            this.characters[ConvertPortToIndex((short)port)] = contentInHex;
            return true;
        }

        /*string[] IIODevice.ReadFromPort(int port)
        {
            //throw new NotImplementedException();
            if(port == IOPort)
            {
                return this.characters;
            }
            else
            {
                throw new ArgumentException($"Invalid port number {port}");
            }
        }*/
        /// <summary>
        /// Reading the data contained in the ASCII Display's port.
        /// </summary>
        /// <param name="port">Port whose data we wish to read.</param>
        /// <returns>String representation of the data stored in the port.</returns>
        public string ReadFromPort(int port)
        {
            if (IsValidPort(port))
            {
                return this.characters[ConvertPortToIndex((short)port)];
            }
            else
            {
                throw new ArgumentException($"Invalid port \n");
            }
        }

        /// <summary>
        /// Reading the data contained in the ASCII Display's port.
        /// </summary>
        /// <param name="port">Port whose data we wish to read.</param>
        /// <returns>String representation of the data stored in the port.</returns>
        string IIODevice.ReadFromPort(int port)
        {
           return ReadFromPort(port);
        }
    }
}
