
using System;
using System.Text.RegularExpressions;

public class Parser{

    private Tokenizer tokenizer;
    private string[] lines;
    private SyntaxAnalyzer syntax_analyzer;
    private OperationCodes codes;
    public Parser(string[] lines)
    {
        codes = new OperationCodes();
        tokenizer = new Tokenizer();

    }

    //Generate the tokens from the lines provided
    public string[] tokens()
    {
        string[] tokens_from_string= { };
        return tokens_from_string;
    }


    public string[] object_code()
    {
        string[] code= { };
        
        return code;
    }

    
}