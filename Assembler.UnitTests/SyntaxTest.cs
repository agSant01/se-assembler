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
        private Tokenizer tokenizer;

        [TestInitialize]
        public void TestInit()
        {
            syntax = new SyntaxAnalyzer();
            tokenizer = new Tokenizer();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            syntax = null;
        }

        [TestMethod]
        public void Verify_Syntax()
        {
           
            string add = "ADD R1 , R2 , R3 ";

            Assert.IsTrue(syntax.isProperSyntax(add));

            string sub = "SUB R1, R2, R3";
            Assert.IsTrue(syntax.isProperSyntax(sub));
        }

        [TestMethod]
        public void VerifyAddTokenExtraction()
        {
            
            string add = "ADD R1 , R2 , R3 ";
            ArrayList tokens = new ArrayList();
            tokens.Add("ADD");
            tokens.Add("R1");
            tokens.Add("R2");
            tokens.Add("R3");
            Assert.AreEqual(tokenizer.tokensOf(add).Count, tokens.Count);
            //Assert.AreEqual(tokens.Count, tokenizer.tokensOf(add).Count);
            //foreach (String s in tokenizer.tokensOf(add))
            //{
            //    Console.WriteLine(s);
            //}

            // Assert.IsNotNull(data[0]);

        }

        [TestMethod]
        public void VerifySubTokenExtraction()
        {
            string add = "SUB R1 , R2 , R3 ";
            ArrayList tokens = new ArrayList();
            tokens.Add("SUB");
            tokens.Add("R1");
            tokens.Add("R2");
            tokens.Add("R3");
            Assert.AreEqual(tokenizer.tokensOf(add).Count, tokens.Count);
            //Assert.AreEqual(tokens.Count, tokenizer.tokensOf(add).Count);
            //foreach (String s in tokenizer.tokensOf(add))
            //{
            //    Console.WriteLine(s);
            //}

            // Assert.IsNotNull(data[0]);

        }





        //[TestMethod]
        //public void Verify_Patterns()
        //{

        //    string add = "ADD R1,R2,R3";
        //    foreach (Regex pat in syntax.reg_patterns())
        //    {
        //        Console.WriteLine(pat);
                
        //        Console.WriteLine($"Is correct grammar:{syntax.isProperSyntax(add)}");
        //    }

        //    // Assert.IsNotNull(data[0]);

        //}


    }
}
