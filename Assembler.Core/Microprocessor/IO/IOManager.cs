using Assembler.Core.Microprocessor.IO;
using Assembler.Microprocessor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.Microprocessor
{
    public class IOManager
    {
        private readonly Dictionary<short, short> _portsAndDevices = new Dictionary<short, short>();

        private readonly Dictionary<short, IIODevice> _devicesAndIds = new Dictionary<short, IIODevice>();

        private short _deviceId = 0;

        private int _maxPort;

        public short ConnectedDevices => (short) _devicesAndIds.Count;

        public IOManager(int maxPort)
        {
            _maxPort = maxPort;
        }

        public void AddIODevice(short port, IIODevice device)
        {
            IsValidPort(port, device);

            _devicesAndIds.Add(_deviceId, device);

            for(short i = 0; i < device.IOPortLength; i++)
            {
                _portsAndDevices.Add( (short) (port + i), _deviceId);
            }

            _deviceId++;
        }

        public bool RemoveIODevice(short port)
        {
            if (_portsAndDevices.TryGetValue(port, out short deviceId))
            {
                IIODevice device = _devicesAndIds[deviceId];

                _devicesAndIds.Remove(deviceId);

                for (short i = 0; i < device.IOPortLength; i++)
                {
                    _portsAndDevices.Remove((short) (port + i));
                }

                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Contents returned in Hexadeciml
        /// </summary>
        /// <param name="port"></param>
        /// <returns>hexadecimal data representation</returns>
        public string ReadFromIO(short port)
        {
            IsValidPort(port, null);

            if (_portsAndDevices.TryGetValue(port, out short deviceId))
            {
                return _devicesAndIds[deviceId].ReadFromPort(port);
            }

            return null;
        }

        public bool WriteToIO(short port, string contentInHex)
        {
            IsValidPort(port, null);

            if (_portsAndDevices.TryGetValue(port, out short deviceId))
            {
                _devicesAndIds[deviceId].WriteInPort(port, contentInHex);

                return true;
            }

            return false;
        }

        public bool IsUsedPort(short port)
        {
            return _portsAndDevices.ContainsKey((short) port);
        }

        private void IsValidPort(short port, IIODevice device)
        {
            if (port < 0 || port > _maxPort)
            {
                throw new OverflowException($"Invalid port address: {port} (0x{Convert.ToString(port, 16).PadLeft(3, '0').ToUpper()}), for '{device.DeviceName}'.");
            }

            if (port + device?.IOPortLength >= _maxPort)
            {
                throw new OverflowException($"Invalid port address: {port} (0x{Convert.ToString(port, 16).PadLeft(3, '0').ToUpper()}), for '{device.DeviceName}'. " +
                    $"This device requires {device.IOPortLength} ports to work properly.");
            }
        } 

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("IOManager[");

            foreach (KeyValuePair<short, IIODevice> pair in _devicesAndIds)
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
