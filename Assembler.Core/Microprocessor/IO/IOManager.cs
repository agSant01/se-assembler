﻿using Assembler.Core.Microprocessor.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.Microprocessor
{
    public class IOManager
    {
        /// <summary>
        /// Dictionary that maps the ports to their corresponding DeviceId. Used to identify the 
        /// requiered IODevice to access
        /// </summary>
        private readonly Dictionary<ushort, ushort> _portsAndDevices = new Dictionary<ushort, ushort>();

        /// <summary>
        /// Dictionary that maps the DeviceId to its corresponding IODevice object.
        /// </summary>
        private readonly Dictionary<ushort, IIODevice> _devicesAndIds = new Dictionary<ushort, IIODevice>();

        /// <summary>
        /// Initial generatate DeviceId. It will increament every time a new IODevice is added.
        /// No two IODevice's will have the same DeviceId
        /// </summary>
        private ushort _deviceId = 0;

        /// <summary>
        /// The last port available for the hardware. 
        /// The only ports avaiable go from: 0 <= Port <= maxPort
        /// </summary>
        private readonly int _maxPort;

        /// <summary>
        /// The amount of IODevices connected at a given time
        /// </summary>
        public short ConnectedDevices => (short)_devicesAndIds.Count;

        /// <summary>
        /// Creates a new IOManager instance
        /// </summary>
        /// <param name="maxPort">Max port available</param>
        public IOManager(int maxPort)
        {
            _maxPort = maxPort;
        }

        /// <summary>
        /// Add device to the I/O registry
        /// </summary>
        /// <param name="port">Assigned port for the IODevice</param>
        /// <param name="device">IODevice instance</param>
        /// <returns>True if port was assigned successfully, false if por was previously assigned</returns>
        public bool AddIODevice(ushort port, IIODevice device)
        {
            // validate port validity
            IsValidPort(port, device);

            // is used
            if (IsUsedPort(port, device))
            {
                throw new Exception($"IO Device requires {device.IOPortLength} ports. The selected port does not meet the device requirements.");
            }

            // verify if port was previously assiged to another I/O device
            if (_portsAndDevices.ContainsKey(port))
            {
                return false;
            }

            // assign the device a DeviceId
            _devicesAndIds.Add(_deviceId, device);

            // reserve all the required ports for the I/O device, even more that one
            for (short i = 0; i < device.IOPortLength; i++) 
            {
                _portsAndDevices.Add((ushort)(port + i), _deviceId);
            }

            // increment DeviceId
            _deviceId++;

            return true;
        }

        /// <summary>
        /// Remove device from I/O registry
        /// </summary>
        /// <param name="port">port of the device</param>
        /// <returns>True if successfull, false if port is not assigned to an I/O device or otherwise</returns>
        public bool RemoveIODevice(ushort port)
        {
            if (_portsAndDevices.TryGetValue(port, out ushort deviceId))
            {
                // get IODevice instance
                IIODevice device = _devicesAndIds[deviceId];

                // remove from devices registry
                _devicesAndIds.Remove(deviceId);

                // remove all the assigned ports
                for (short i = 0; i < device.IOPortLength; i++)
                {
                    _portsAndDevices.Remove((ushort)(port + i));
                }

                return true;
            }

            // if port is not assigned to any device
            return false;
        }

        /// <summary>
        /// Read data from the IODevice in the specified port
        /// </summary>
        /// <param name="port">Port to read from</param>
        /// <returns>Data in hexadecimal representation, null if the port is not assigned to an I/O device</returns>
        public string ReadFromIO(ushort port)
        {
            IsValidPort(port, null);

            if (_portsAndDevices.TryGetValue(port, out ushort deviceId))
            {
                return _devicesAndIds[deviceId].ReadFromPort(port);
            }

            return null;
        }

        /// <summary>
        /// Write data to I/O device in the specified port
        /// </summary>
        /// <param name="port">Port to write data</param>
        /// <param name="contentInHex">Data in Hexadecimal format</param>
        /// <returns>True if success, false write was not a success or if the port is not assigned to an I/O device</returns>
        public bool WriteToIO(ushort port, string contentInHex)
        {
            IsValidPort(port, null);

            if (_portsAndDevices.TryGetValue(port, out ushort deviceId))
            {
                // return IODevice result
                return _devicesAndIds[deviceId].WriteInPort(port, contentInHex);
            }

            // port is not assigned to an IODevice
            return false;
        }

        /// <summary>
        /// Verify if port is assigned.
        /// </summary>
        /// <param name="port">Port to verify</param>
        /// <returns>Returns true if port is already assigned, false if not</returns>
        public bool IsUsedPort(ushort port)
        {
            return _portsAndDevices.ContainsKey(port);
        }

        /// <summary>
        /// Verify if port is assigned.
        /// </summary>
        /// <param name="port">Port to verify</param>
        /// <returns>Returns true if port is already assigned, false if not</returns>
        public bool IsUsedPort(ushort port, IIODevice device)
        {
            for (ushort i = port; i < device.IOPortLength+port-1; i++)
            {
                if (_portsAndDevices.ContainsKey(i))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ResetIOs()
        {
            bool[] results = new bool[_devicesAndIds.Count];
            int count = 0;

            foreach (IIODevice device in _devicesAndIds.Values)
            {
                results[count] = device.Reset();
                ++count;
            }
            
            foreach(bool result in results)
            {
                // could not reset at least one IO Device
                if (!result)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Validate port for I/O device
        /// </summary>
        /// <param name="port">port to assigned</param>
        /// <param name="device">IODevice object</param>
        private void IsValidPort(ushort port, IIODevice device)
        {
            if (port < 0 || port > _maxPort)
            {
                throw new OverflowException($"Invalid port address: {port} (0x{Convert.ToString(port, 16).PadLeft(3, '0').ToUpper()}), for '{device.DeviceName}'.");
            }

            if (port + device?.IOPortLength - 1> _maxPort)
            {
                throw new OverflowException($"Invalid port address: {port} (0x{Convert.ToString(port, 16).PadLeft(3, '0').ToUpper()}), for '{device.DeviceName}'. " +
                    $"This device requires {device.IOPortLength} port(s) to work properly.");
            }
        }

        /// <summary>
        /// String representation of the object
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("IOManager[");

            foreach (KeyValuePair<ushort, IIODevice> pair in _devicesAndIds)
            {
                builder.Append("\t");

                builder.Append($"ID: {pair.Key}, Device: {pair.Value.ToString()}");

                builder.Append("\n");
            }

            builder.Append("]");

            return builder.ToString();
        }
    }
}
