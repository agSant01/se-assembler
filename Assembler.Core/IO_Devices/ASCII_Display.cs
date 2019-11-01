using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Core.IO_Devices
{
    public class ASCII_Display
    {
        private readonly string[] ascii_chars = {"A","B","C","D","E","F","G","H" };
        private readonly byte DEFAULT = 0;
        private byte[] characters;//these need to be mapped contigously on VirtualMemory
        public ASCII_Display()
        {
            this.characters = new byte[8];
        }

        public void addCharacter(int idx, byte b)
        {
            if (!IsValidIndex(idx))
                return;

            this.characters[idx] = b;
        }

        public void RemoveCharacter(int idx)
        {
            if (!IsValidIndex(idx))
                return;

            this.characters[idx] = DEFAULT;
        }

        private bool IsValidIndex(int idx)
        {
            if (idx > this.characters.Len | idx == this.characters.Len | idx < 0)
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
                return res;
            }
        }


        public byte GetByte(int idx)
        {
            if (!IsValidIndex(idx))
                return DEFAULT;

            return this.characters[idx];
        }
    }
}
