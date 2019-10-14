using Assembler.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Assembler.UnitTests.CompilerTests
{
    [TestClass]
    public class LexicalAnalyzerTests
    {
        Lexer lexer;

        private readonly string testFileSuccess =  Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\assembly_test.txt");
        private readonly string testFileErrorToken = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            @"CompilerTests\AssemblyFiles\assembly_test_syntax_error.txt");

        private readonly Token[] testSuccess =
        {
            new Token(TokenType.LINE_COMMENT,"//"),
            ////new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"First"),
            ////new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"program"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.ORIGIN,"org"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"0"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"JMPADDR"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"start"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"valor1"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.VARIABLE_ASSIGN,"db"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"5"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"valor2"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.VARIABLE_ASSIGN,"db"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"7"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"mayor"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.VARIABLE_ASSIGN,"db"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"0"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.CONSTANT_ASSIGN,"const"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"ten"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"0A"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"start"),
            new Token(TokenType.COLON,":"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"LOAD"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER,"R1"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"valor1"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"LOAD"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER,"R2"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"valor2"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"GRT"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER,"R1"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER,"R2"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"JMPADDR"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER, "R1esMayor"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"STORE"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"mayor"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER,"R2"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"JMPADDR"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"fin"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER, "R1esMayor"),
            new Token(TokenType.COLON,":"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"STORE"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"mayor"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER,"R1"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"LOADIM"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER,"R3"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"#8"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"fin"),
            new Token(TokenType.COLON,":"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"JMPADDR"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"fin"),
            new Token(TokenType.NEW_LINE,"\\n"),
        };

        private Token[] testErrorToken =
        {
            new Token(TokenType.LINE_COMMENT,"//"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"First"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"program"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.ORIGIN,"org"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"0"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"JMPADDR"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"start"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"valor1"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.VARIABLE_ASSIGN,"db"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"5"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"valor2"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.VARIABLE_ASSIGN,"db"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"7"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"mayor"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.VARIABLE_ASSIGN,"db"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"0"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"cst"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"ten"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"0A"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"start"),
            new Token(TokenType.COLON,":"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"LOAD"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER, "R1"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"valor1"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"LOAD"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER, "R2"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"valor2"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"GRT"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER, "R1"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER, "R2"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.IDENTIFIER,"JMPADR"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER, "R1esMayor"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"STORE"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"mayor"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER, "R2"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"JMPADDR"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"fin"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER, "R1esMayor"),
            new Token(TokenType.COLON,":"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.IDENTIFIER,"STOE"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"mayor"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER, "R1"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"LOADIM"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.REGISTER, "R3"),
            new Token(TokenType.COMMA,","),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"#8"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.IDENTIFIER,"fin"),
            new Token(TokenType.COLON,":"),
            new Token(TokenType.NEW_LINE,"\\n"),
            new Token(TokenType.TAB,"	"),
            new Token(TokenType.OPERATOR,"JMPADDR"),
            //new Token(TokenType.WHITE_SPACE," "),
            new Token(TokenType.IDENTIFIER,"fin"),
            new Token(TokenType.NEW_LINE,"\\n"),
        };

        [TestCleanup]
        public void TestCleanup()
        {
            lexer = null;  
        }

        [TestMethod]
        public void LexicalAnalyzer_TokenizeAssemblyFile_Success()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileSuccess);

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines)
            {
                SkipWhiteSpaces = true
            };

            int counter = 0;
            while (lexer.MoveNext())
            {
                Console.WriteLine(lexer.CurrrentToken);
                Assert.AreEqual<Token>(testSuccess[counter], lexer.CurrrentToken);
                counter++;
            }
        }

        [TestMethod]
        public void LexicalAnalyzer_TokenizeAssemblyFile_WithErrorTokens()
        {
            string[] lines = FileManager.Instance.ToReadFile(testFileErrorToken);

            Assert.IsNotNull(lines, "File Not Found.");

            lexer = new Lexer(lines)
            {
                SkipWhiteSpaces = true
            };

            int counter = 0;
            while (lexer.MoveNext())
            {
                Console.WriteLine(lexer.CurrrentToken);
                Assert.AreEqual<Token>(testErrorToken[counter], lexer.CurrrentToken);
                counter++;
            }
        }
    }
}
