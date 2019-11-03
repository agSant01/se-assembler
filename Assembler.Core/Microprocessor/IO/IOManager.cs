using Assembler.Core.Microprocessor.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.Microprocessor
{
    public class IOManager
    {
        private readonly Dictionary<short, short> _portsAndDevices = new Dictionary<short, short>();

        private readonly Dictionary<short, IODevice> _devicesAndIds = new Dictionary<short, IODevice>();

        private short _deviceId = 0;

        public short ConnectedDevices => (short) _devicesAndIds.Count;

        public void AddIODevice(int v, Func<object> p)
        {
            throw new NotImplementedException();
        }

        public void AddIODevice(short port, IODevice device)
        {
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
                IODevice device = _devicesAndIds[deviceId];

                _devicesAndIds.Remove(deviceId);

                for (short i = 0; i < device.IOPortLength; i++)
                {
                    _portsAndDevices.Remove((short) (port + i));
                }

                return true;
            }

            return false;
        }

        public string ReadFromIO(short port)
        {
            if (_portsAndDevices.TryGetValue(port, out short deviceId))
            {
                return _devicesAndIds[deviceId].ReadFromPort(port);
            }

            return null;
        }

        public bool WriteToIO(short port, string contentInHex)
        {
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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("IOManager[");

            foreach (KeyValuePair<short, IODevice> pair in _devicesAndIds)
            {
                builder.Append("\t");

                builder.Append($"Id: {pair.Key}, Device: {pair.Value.ToString()}");

                builder.Append("\n");
            }

            builder.Append("]");

            return builder.ToString();
        }
    }
}
