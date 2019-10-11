using Assembler.Microprocessor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Assembler.Microprocessor
{
    
    public  class VirtualMemory : Utils.ICustomIterable<string>
    {

        //private string[] memory_contents;
        //private  long[] memory_addresses;

        private Dictionary<long, string> memory;
        public VirtualMemory(string[] contents, long[] addresses)
        {
            this.memory = new Dictionary<long, string>();
            LoadMemory(contents, addresses);
        }

         private void LoadMemory(string[] contents, long[] addresses)
        {
            //Fails if len(contents) > len(addresses)
            if( contents.Length > addresses.Length )
                throw new Exception("Contents of memory greater than address space\n");
          
            for (int i = 0; i < contents.Length; i++)
                this.memory[addresses[i]] = contents[i];
        }
        
        public string GetContents(long address)
        {
            if (memory.ContainsKey(address))
                return this.memory[address];
            else
                throw new Exception("The address provided is invalid\n");
        }

        public long GetAddress(string contents)
        {
            if (memory.ContainsValue(contents))
            {
                foreach (KeyValuePair<long, string> item in memory)
                {
                    //  Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                    if (item.Value == contents)
                        return item.Key;
                }
            }

            throw new Exception("Contents provided are not in memory \n");
        }

        public void SetContents(long address, string contents)
        {
            if(memory.ContainsKey(address))
            {
                this.memory[address] = contents;
            }
            else
            {
                //we just add both anyway no?
                this.memory[address] = contents;
            }
        }



        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\n");
            foreach (KeyValuePair<long, string> item in this.memory)
            {
                //  Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                builder.Append("\t[" + item.Key + "," + item.Value + "]\n");
            }
            builder.Append("}\n\n");

            return builder.ToString();

        }



    }
}
