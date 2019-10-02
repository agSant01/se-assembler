using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Assembler.Parser;

namespace Assembler.UnitTests
{

    [TestClass]
    public class OperatorsInfoTests
    {
        string[] ops = { "JMP", "add", "JMPAddR", "LoAd" };

        EInstructionFormat[] opFormats =
        {
            EInstructionFormat.FORMAT_1,
            EInstructionFormat.FORMAT_1,
            EInstructionFormat.FORMAT_3,
            EInstructionFormat.FORMAT_2,
        };

        [TestMethod]
        public void OperatorsInfoTests_GetInstructionFormats_Success()
        {
            int idx = 0;
            foreach (string operation in ops)
            {
                EInstructionFormat f = OperatorsInfo.GetInstructionFormat(operation);

                Assert.AreEqual(opFormats[idx], f);

                idx++;
            }
        }

        [TestMethod]
        public void OperatorsInfoTests_GetInstructionFormats_InvalidFormat()
        {
            int idx = 0;

            Array.Resize(ref ops, 5);
            ops[4] = "I dnt Exist";

            Array.Resize(ref opFormats, 5);
            opFormats[4] = EInstructionFormat.INVALID;



            foreach (string operation in ops)
            {
                EInstructionFormat f = OperatorsInfo.GetInstructionFormat(operation);

                Assert.AreEqual(opFormats[idx], f);

                idx++;
            }
        }
    }
}
