using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Assembler
{
    
    class VirtualMemory : IEnumerable
    {
        private string[] memory_contents;
        private string[] memory_addresses;

        public VirtualMemory(string[] contents, string[] addresses)
        {

        }

        public string[] GetContents(string address)
        {
            string[] contents = null;
            return contents;
        }

        public string[] GetAddress(string contents)
        {
            string[] address = null;
            return address;
        }

        public IEnumerable<string> GetEnumerator()
        {
            for(int i=0; i < this.memory_contents.Length; i++)
                yield return this.memory_contents[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return GetEnumerator();
        }


    }
}
