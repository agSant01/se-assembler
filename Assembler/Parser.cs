
using System;
using System.Text.RegularExpressions;

public class Parser{

    private string[] lines;
    private SyntaxAnalyzer syntax_analyzer;
    private OperationCodes codes;
    public Parser(string[] lines)
    {

    }


    public string[] tokens()
    {
        string[] tokens_from_string= { };
        return tokens_from_string;
    }


    public int[] machine_code()
    {
        int[] bits = { };
        return bits;
    }


}