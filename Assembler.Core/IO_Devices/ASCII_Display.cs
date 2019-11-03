using Assembler.Microprocessor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.IO_Devices
{
    public class ASCII_Display
    {
        //private readonly string[] ascii_chars = {"A","B","C","D","E","F","G","H" };
        private bool[] active;
        private readonly int[] reserved_addresses;
        private readonly byte DEFAULT = 0;
        private byte[] characters;//these need to be mapped contigously on VirtualMemory
        public ASCII_Display(VirtualMemory mem)
        {
            //We need to find 8 memory locations in virutal memory to reserve the bytes...
            reserved_addresses = ReserveMemory(mem);
            this.characters = new byte[8];
            this.active = new bool[8];
        }


        private int[] ReserveMemory(VirtualMemory mem)
        {
            int[] addresses_to_use = new int[8];
            ArrayList addresses = new ArrayList();
            int curr = 0;
            for(int i =1; i <= mem.LastAddress(); i++)//avoid using first address... may have a bug here cause we reach Last -1
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

        public ArrayList ActiveCharactersIndexes(VirtualMemory mem)
        {
            ArrayList active = new ArrayList();
            int[] reserved = this.ReservedAddresses();
            for(int i=0; i <reserved.Length ; i++)
            {
                if(mem.IsInUse(reserved[i]))//checks if reserved memory is now in use
                    active.Add(i);//add to active index list
            }
            

            return active;
        }

        public ArrayList InactiveCharactersIndexes(VirtualMemory mem)
        {
            ArrayList inactive = new ArrayList();
            int[] reserved = this.ReservedAddresses();
            for (int i = 0; i < reserved.Length; i++)
            {
                if (!mem.IsInUse(reserved[i]))//checks if reserved memory is now in use
                    inactive.Add(i);//add to active index list
            }


            return inactive;
        }
    }
}
