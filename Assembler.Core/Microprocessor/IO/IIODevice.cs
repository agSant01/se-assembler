namespace Assembler.Core.Microprocessor.IO
{
    /// <summary>
    /// IODevice object. Used to implement any new I/O that will interact with the microprocessor
    /// </summary>
    public interface IIODevice
    {
        /// <summary>
        /// Device ID or name. Must be Implemented by each IODevice.
        /// </summary>
        public string DeviceName { get; }

        /// <summary>
        /// Connection port for the I/O
        /// </summary>
        public ushort IOPort { get; }

        /// <summary>
        /// Amount of consecutive ports that this I/O device requires to operate
        /// </summary>
        public ushort IOPortLength { get; }

        /// <summary>
        /// Has new data available
        /// </summary>
        public bool HasData { get; }

        /// <summary>
        /// Write data in Hexadecimal format to the specified port of the I/O device
        /// </summary>
        /// <param name="port">Port to write data in the device</param>
        /// <param name="contentInHex">Data to bre written in the device in Hexadecimal format</param>
        /// <returns>True if success, false other wise</returns>
        public bool WriteInPort(int port, string contentInHex);

        /// <summary>
        /// Read data from IO. Returned in Hexadecimal format
        /// </summary>
        /// <param name="port">IO port</param>
        /// <returns>hexadecimal data representation</returns>
        public string ReadFromPort(int port);

        /// <summary>
        /// Reset state of the I/O device
        /// </summary>
        /// <returns>True if success, false otherwise</returns>
        public bool Reset();
    }
}
