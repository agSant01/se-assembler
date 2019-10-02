using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

namespace Assembler
{
    [TestClass]
    public class SyntaxTest
    {
        private SyntaxAnalyzer syntax;

        [TestInitialize]
        public void TestInit()
        {
            syntax = new SyntaxAnalyzer();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            syntax = null;
        }

        [TestMethod]
        public void Verify_Syntax()
        {
            Tokenizer tokenizer = new Tokenizer();
            string add = "ADD R1 , R2 , R3 ";
            ArrayList tokens = new ArrayList();
            tokens.Add("ADD");
            tokens.Add("R1");
            tokens.Add("R2");
            tokens.Add("R3");

            Assert.IsTrue(syntax.isProperSyntax(add));
            //Assert.AreEqual(tokens.Count, tokenizer.tokensOf(add).Count);
            foreach(String s in tokenizer.tokensOf(add))
            {
                Console.WriteLine(s);
            }
            
           // Assert.IsNotNull(data[0]);

        }

        [TestMethod]
        public void Verify_Patterns()
        {

            string add = "ADD R1,R2,R3";
            foreach (Regex pat in syntax.reg_patterns())
            {
                Console.WriteLine(pat);
                
                Console.WriteLine($"Is correct grammar:{syntax.isProperSyntax(add)}");
            }

            // Assert.IsNotNull(data[0]);

        }


    }
}
