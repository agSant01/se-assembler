using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Assembler
{
    
    class VirtualMemory : Utils.ICustomIterable<string>
    {
        //private string[] memory_contents;
        //private  long[] memory_addresses;

        private Dictionary<long, string> memory;
        public VirtualMemory(string[] contents, long[] addresses)
        {
            memory = new Dictionary<long, string>();
            LoadMemory(contents, addresses);
        }

         private void LoadMemory(string[] contents, long[] addresses)
        {
            for (int i = 0; i < contents.Length; i++)
                memory[addresses[i]] = contents[i];
        }
        
        public string GetContents(long address)
        {
            if (memory.ContainsKey(address))
                return memory[address];
            else
                return null;//need to throw error here
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
       
                return 0;//need to throw error here
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\n");
            foreach (KeyValuePair<long, string> item in memory)
            {
                //  Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                builder.Append("\t[" + item.Key + "," + item.Value + "]\n");
            }
            builder.Append("}\n\n");

            return builder.ToString();

        }
        //public IEnumerable<string> GetEnumerator()
        //{
        //    for(int i=0; i < this.memory_contents.Length; i++)
        //        yield return this.memory_contents[i];
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    yield return GetEnumerator();
        //}


    }
}
