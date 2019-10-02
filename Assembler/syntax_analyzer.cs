using System;
using System.Collections;
using System.Text.RegularExpressions;

//Verifies that Syntax is correct and makes sure operations carry proper structure
public class SyntaxAnalyzer
{
    private ArrayList patterns;
	public SyntaxAnalyzer()
	{
        patterns = new ArrayList();
        this.loadPatterns();
	}

    // We use regular expressions to determine if the structure of the OP-Code is correct
    public bool isProperSyntax(string line)
    {
        bool result = false;
        MatchCollection matched_expression ;
        
        foreach( Regex pattern in this.patterns)
        {
            
            matched_expression = pattern.Matches(line);
            //Console.WriteLine(matched_expression.Count);
            if(matched_expression.Count > 0)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    public Regex[] math_patterns()
    {
        Regex add = new Regex(@"^[ ]*ADD[ ]+R[0-7][ ]*,[ ]*R[0-7][ ]*,[ ]*R[0-7][ ]*");
        Regex sub =  new Regex(@"^[ ]*SUB[ ]+R[0-7][ ]*,[ ]*R[0-7][ ]*,[ ]*R[0-7][ ]*");
         
        Regex addim = new Regex(@"^[ ]*ADDIM[ ]+R[0-7][ ]*,[ ]*R[0-7][ ]*");
        Regex subim = new Regex(@"^[ ]*SUBIM[ ]+R[0-7][ ]*,[ ]*R[0-7][ ]*");

        Regex []patterns = { add, sub, addim, subim };

        return patterns;
    }

    public Regex[] conditional_patterns()
    {
        Regex and = new Regex(@"^[ ]*AND[ ]+R[0-7][ ]*,[ ]*R[0-7][ ]*,[ ]*R[0-7][ ]*");
        Regex or =  new Regex(@"^[ ]*OR[ ]+R[0-7][ ]*,[ ]*R[0-7][ ]*, [ ]*R[0-7][ ]*");
        Regex xor = new Regex(@"^[ ]*XOR[ ]+R[0-7][ ]*,[ ]*R[0-7][ ]*,[ ]*R[0-7][ ]*");

        Regex not = new Regex( @"^[ ]*NOT[ ]+R[0-7][ ]*,[ ]*R[0-7][ ]*");
        Regex neg = new Regex( @"^[ ]*NEG[ ]+R[0-7][ ]*,[ ]*R[0-7][ ]*");

        Regex[] patterns = { and, or , xor, not, neg };
        return patterns;
    }

    public Regex ending_pattern()
    {
        return new Regex( @"^[ ]*RETURN[ ]*");
    }


    private void loadPatterns()
    {
        this.patterns.Add(this.ending_pattern());

        foreach (Regex pattern in this.math_patterns())
        {
            this.patterns.Add(pattern);
        }

        foreach (Regex pattern in this.conditional_patterns())
        {
            this.patterns.Add(pattern);
        }
    }

    public ArrayList reg_patterns()
    {
        return this.patterns;
    }
}
