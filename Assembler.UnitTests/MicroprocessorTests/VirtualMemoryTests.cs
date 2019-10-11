using Assembler.Microprocessor;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.UnitTests.MicroprocessorTests
{
    [TestClass]
    public class VirtualMemoryTests
    {
        //string[] contents;
        //long[] addresses;
        VirtualMemory memory;

        [TestMethod]
        [ExpectedException(typeof(Exception), "Contents of memory greater than address space\n")]
        public void Virtual_Memory_Init_Uneven_Args_Length_Fail()
        {
            long[] addresses = { 0L, 1L, 2L, 3L, 4L };
            string[] contents = { "a", "b", "c","d","e","f" };

            memory = new VirtualMemory(contents, addresses);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "The address provided is invalid\n")]
        public void Virtual_Memory_Invalid_Address_Fail()
        {
            long[] addresses = { 0L, 1L, 2L };
            string[] contents = { "a", "b", "c" };

            memory = new VirtualMemory(contents, addresses);

            memory.GetContents(3L);
        }



        [TestMethod]
        [ExpectedException(typeof(Exception), "Contents provided are not in memory \n")]
        public void Virtual_Memory_Invalid_Contents_Not_In_Memory_Fail()
        {
            long[] addresses = { 0L, 1L, 2L };
            string[] contents = { "a", "b", "c" };

            memory = new VirtualMemory(contents, addresses);

            memory.GetAddress("d");
        }


        [TestMethod]
        public void Virtual_Memory_To_String_Conversion()
        {
            long[] addresses = { 0L, 1L, 2L };
            string[] contents = { "a", "b", "c" };

            memory = new VirtualMemory(contents, addresses);

            string result = memory.ToString();
            string desired = "{\n\t[0,a]\n\t[1,b]\n\t[2,c]\n}\n\n";

            Assert.AreEqual(result, desired);

        }

    }
}
