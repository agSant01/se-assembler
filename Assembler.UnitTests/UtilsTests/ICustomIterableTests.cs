using Assembler.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Assembler.UnitTests.UtilsTests
{
    [TestClass]
    public class ICustomIterableTests
    {
        [TestMethod]
        public void TestDummyOfICustumIterable()
        {
            Dummy dis = new Dummy();

            string[] expected =
            {
                "Test1",
                "Test2",
                "Test3",
                "Test4",
                "Test5"
            };

            dis.AddStr("Test1");
            dis.AddStr("Test2");
            dis.AddStr("Test3");
            dis.AddStr("Test4");
            dis.AddStr("Test5");

            Assert.AreEqual(5, dis.Size);

            Console.WriteLine($"Size: {dis.Size}");

            int index = 0;

            while (dis.MoveNext())
            {
                Console.WriteLine(dis.Current);
                Assert.AreEqual(expected[index++], dis.Current);
            }
        }
    }

    class Dummy : ICustomIterable<string>
    {
        public void AddStr(string str)
        {
            Add(str);
        }
    }
}
