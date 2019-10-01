using System;
using System.Collections;
using System.Text.RegularExpressions;

//Will receive the lines from a File and turn them into respective Tokens
public class Tokenizer
{
    private OperationCodes codes;
    private SyntaxAnalyzer grammar_analyzer;
    private Tokens tokens;
	public Tokenizer()
	{
        codes = new OperationCodes();
        grammar_analyzer = new SyntaxAnalyzer();
	}

    //TODO: Fix when we dont find any, currently returns empty string
    public string findPattern(string line)
    {
        MatchCollection matched_expression = null;
        Regex rg;
        foreach (string pattern in this.grammar_analyzer.reg_patterns())
        {
            rg = new Regex(pattern);
            matched_expression = rg.Matches(line);

            if (matched_expression.Count > 0)//we found the pattern
            {
                return pattern;
            }
        }
        return "";//return an empty string if none is found
    }

    public bool isKeyword(string line)
    {
        if(line.Trim().Length == 0)
        {
            return false;
        }

        foreach(string t in this.tokens.keywords())
        {
            if (t == line)
            {
                return true;
            }
        }

        return false;
    }


    public bool isRegister(string line)
    {
        if (line.Trim().Length == 0)
        {
            return false;
        }

        foreach (string t in this.tokens.register_tokens())
        {
            if (t == line)
            {
                return true;
            }
        }

        return false;
    }

    public bool hasToken(string token, string line)
    {
        //Will purposely ignore whitespaces
        if(token.Trim().Length == 0 || line.Trim().Length == 0)
        {
            return false;
        }

        //if(isKeyword(token))
        //{
        //    return matchingResults(line, token).Count > 0;
        //}

        //if (isRegister(token))
        //{
        //    //foreach (string t in this.tokens.register_tokens())
        //    //{
        //    //    if (t == line)
        //    //    {
        //    //        return true;
        //    //    }
        //    //}
        //    return matchingResults(line, token).Count > 0;
        //}

        return matchingResults(line, token).Count >  0;

    }

    public MatchCollection matchingResults(string line, string pattern)
    {
        Regex rg = new Regex(pattern);
        return rg.Matches(pattern);
    }

   //Extracts the tokens of provided line
    public ArrayList tokensOf(string line)
    {
        if (line.Trim().Length == 0 || !grammar_analyzer.isProperSyntax(line))
            return new ArrayList();

        ArrayList tks = new ArrayList();

        foreach(string t in this.tokens.keywords())
        {
            if (hasToken(t, line))
                tks.Add(t);
        }

        foreach (string t in this.tokens.register_tokens())
        {
            if (hasToken(t, line))
                tks.Add(t);
        }

        return tks;
    }

    //Incorrect name
    //public ArrayList extractTokens(string line)//Returns an arraylist of all matching lines
    //{
    //    ArrayList tokens = new ArrayList();
    //    if (!grammar_analyzer.isProperSyntax(line))
    //    {
    //        return tokens;//return an empty ArrayList
    //    }

    //    string regex_pattern = this.findPattern(line);
    //    MatchCollection matches;
    //    if(regex_pattern.Length == 0 || regex_pattern.Trim().Length == 0)
    //    {
           
    //        return tokens;
    //    }

    //    matches = new Regex(regex_pattern).Matches(line);
    //    foreach (Match match in matches)
    //    {
    //        GroupCollection groups = match.Groups;
    //        tokens.Add(groups["word"].Value);
    //    }

    //    return tokens;
    //}

   
}
