using Assembler.Core.Microprocessor;
using Assembler.Core.Microprocessor.IO;
using Assembler.Microprocessor;
using Assembler.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.Microprocessor
{
    public class ASCII_Display: IIODevice
    {
        //private readonly string[] ascii_chars = {"A","B","C","D","E","F","G","H" };
        private bool[] active;
        private readonly int[] reserved_addresses;
        private readonly byte DEFAULT = 0;
        private byte[] characters;//these need to be mapped contigously on VirtualMemory
        private VirtualMemory mem;
        private Queue<string> _buffer = new Queue<string>();

        public short IOPortLength => 8;//bytes

        public bool HasData => _buffer.Count > 0;

        public byte BufferSize => (byte)_buffer.Count;

        public ASCII_Display(VirtualMemory mem)
        {
            //We need to find 8 memory locations in virutal memory to reserve the bytes...
            this.mem = mem;
            reserved_addresses = ReserveMemory();
            this.characters = new byte[8];
            this.active = new bool[8];
        }


        private int[] ReserveMemory()
        {
            int[] addresses_to_use = new int[8];
            ArrayList addresses = new ArrayList();
            int curr = 0;
            for(int i =1; i <= this.mem.VirtualMemorySize ; i++)//avoid using first address... may have a bug here cause we reach Last -1
            {
                if(!mem.IsInUse(i))
                {
                    addresses.Add(i);

                    if (addresses.Count == 8)
                        break;
                }
                else if (curr > 0 && mem.IsInUse(i))
                {
                    //we ignore it,empty our list and move on
                    addresses.Clear();
                    curr = 0;
                }
            }

            if(addresses.Count != 8)
                throw new Exception("ERROR: Contiguous Segments of Virtual Memory Not Found");

            int j = 0;
            foreach(int i in addresses)
            {
                addresses_to_use[j++] = i;
            }

            return addresses_to_use;
        }

        public void AddCharacter(int idx, byte b)
        {
            if (!IsValidIndex(idx))
                return;

            this.characters[idx] = b;
        }

        public void RemoveCharacter(int idx)
        {
            if (!IsValidIndex(idx))
                return;

            this.characters[idx] = DEFAULT;//Sets to default value when deleting
        }

        private bool IsValidIndex(int idx)
        {
            if (idx > this.characters.Length | idx == this.characters.Length | idx < 0)
                return false;
            return true;
        }

        public string GetChar(int idx)
        {
            if (!IsValidIndex(idx))
                return "";

            byte res = GetByte(idx);
            if ( res == DEFAULT)
            {
                return "";
            }

            else
            {
                return res.ToString();
            }
        }

        public byte GetByte(int idx)
        {
            if (!IsValidIndex(idx))
                return DEFAULT;

            return this.characters[idx];
        }

        public int[] ReservedAddresses()
        {
            return this.reserved_addresses;
        }

        public int[] ActiveCharactersIndexes()
        {
            ArrayList active = new ArrayList();
            int[] reserved = this.ReservedAddresses();
            for(int i=0; i <reserved.Length ; i++)
            {
                if(this.mem.IsInUse(reserved[i]))//checks if reserved memory is now in use
                    active.Add(i);//add to active index list
            }

            int size = active.Count;
            int[] res = new int[size];
            int k = 0;
            foreach(int j in active)
            {
                res[k++] = j;
            }
            return res;
        }

        public int[] InactiveCharactersIndexes()
        {
            ArrayList inactive = new ArrayList();
            int[] reserved = this.ReservedAddresses();
            for (int i = 0; i < reserved.Length; i++)
            {
                if (!this.mem.IsInUse(reserved[i]))//checks if reserved memory is now in use
                    inactive.Add(i);//add to active index list
            }

            int size = inactive.Count;
            int[] res = new int[size];
            int k = 0;
            foreach (int j in inactive)
            {
                res[k++] = j;
            }
            return res;
        }

        public string ReadFromPort(int port)
        {
            if (_buffer.Count == 0)
            {
                return UnitConverter.ByteToBinary(0);
            }

            return _buffer.Dequeue();
        }

        public bool Reset()
        {
            _buffer.Clear();

            return true;
        }

        public bool WriteInPort(int port, string contentInHex)
        {
            throw new InvalidOperationException("This port is reserved for read-only operations");
        }

        public void KeyPress(string hexChar)
        {
            if (_buffer.Count < 4)
            {
                _buffer.Enqueue(UnitConverter.HexToBinary(hexChar + "1"));
            }
        }

        public override string ToString()
        {
            return $"ASCII_Display[reserved memory: {String.Join(", ",reserved_addresses)}]";
        }

    }
}
