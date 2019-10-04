using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Assembler.Parsing;

namespace Assembler.UnitTests
{

    [TestClass]
    public class OperatorsInfoTests
    {
        string[] ops = { "JMPRIND", "add", "JMPAddR", "LoAd" };
<<<<<<< HEAD
        
=======
>>>>>>> master

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
<<<<<<< HEAD
            Token[] opTkn = new Token[ops.Length];
            for (int i = 0; i < opTkn.Length; i++)
            {
                opTkn[i] = new Token(TokenType.OPERATOR, ops[i]);
            }
            int idx = 0;
            foreach (Token operation in opTkn)
=======
            int idx = 0;
            foreach (string operation in ops)
>>>>>>> master
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

<<<<<<< HEAD
            Token[] opTkn = new Token[ops.Length];
            for (int i = 0; i < opTkn.Length; i++)
            {
                opTkn[i] = new Token(TokenType.OPERATOR, ops[i]);
            }

            foreach (Token operation in opTkn)
=======


            foreach (string operation in ops)
>>>>>>> master
            {
                EInstructionFormat f = OperatorsInfo.GetInstructionFormat(operation);

                Assert.AreEqual(opFormats[idx], f);

                idx++;
            }
        }
    }
}
