using Assembler.Microprocessor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Assembler.Microprocessor
{
    
    public  class VirtualMemory : Utils.ICustomIterable<string>
    {

        private Dictionary<long, string> memory;

        public VirtualMemory(string[] contents, long[] addresses)
        {
            this.memory = new Dictionary<long, string>();
            LoadMemory(contents, addresses);
        }
        
        /// <summary>
        /// A private method for initializing the VirtualMemory instance.
        /// Internally creates a dictionary via iteration through both provided arrays.
        /// Throws an exception when contents' length exceeds that of addresses'.
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="addresses"></param>
         private void LoadMemory(string[] contents, long[] addresses)//Either of these args could be null and pose problems
        {
            //Fails if len(contents) > len(addresses)
            if( contents.Length > addresses.Length )
                throw new Exception("Contents of memory greater than address space\n");
          
            for (int i = 0; i < contents.Length; i++)
                this.memory[addresses[i]] = contents[i];
        }
        
        /// <summary>
        /// A method for retrieving the contents from provided address.
        /// Throws an exception if provided address is not found within current VirtualMemory instance.
        /// </summary>
        /// <param name="address">long representing the location in memory to retrieve contents from</param>
        /// <returns>string representation of contents in current VirtualMemory instance (if found).</returns>
        public string GetContents(long address)
        {
            if (memory.ContainsKey(address))
                return this.memory[address];
            else
                throw new Exception("The address provided is invalid\n");
        }

        /// <summary>
        /// A method for retreieving the address in VirtualMemory of provided contents.
        /// Will throw an Exception if said contents  are not found within current instance of VirtualMemory.
        /// </summary>
        /// <param name="contents"> String of contents to be found in VirtualMemory instance.</param>
        /// <returns> long representing the address in VirtualMemory where contents are located (if found).</returns>
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

        /// <summary>
        /// A method for manipulating the contents of current VirtualMemory instance.
        /// </summary>
        /// <param name="address">The address (long) into which to insert the new contents.</param>
        /// <param name="contents"> The new contents (string) to be added into internal memory structure.</param>
        public void SetContents(long address, string contents)
        {
            if(memory.ContainsKey(address))
            {
                this.memory[address] = contents;
            }
            else
            {
                //Maybe add constant to limit what addresses may be added to the internal structure
                //Since we have a limit of 4k lines anyway.
                //we just add both anyway no?
                this.memory[address] = contents;
            }
        }


        /// <summary>
        /// A string representation of the current instance of the VirtualMemory class
        /// </summary>
        /// <returns> string representation of the current instance of the Virtual Memory</returns>
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
