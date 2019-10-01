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

        MatchCollection matched_expression = null;
        Regex rg;
        foreach(string pattern in this.reg_patterns())
        {
            rg = new Regex(pattern);
            matched_expression = rg.Matches(line);

            if(matched_expression.Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    public string[] math_patterns()
    {
        string add = @"[ ]*[A][D][D][ ]+ [R][0-7][ ]*[,][ ]*[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";
        string sub = @"[ ]*[S][U][B][ ]+ [R][0-7][ ]*[,][ ]*[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";

        string addim = @"[ ]*[A][D][D][I][M][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";
        string subim = @"[ ]*[S][U][B][I][M][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";

        string []patterns = { add, sub, addim, subim };

        return patterns;
    }

    public string[] conditional_patterns()
    {
        string and = @"[ ]*[A][N][D][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";
        string or =  @"[ ]*[O][R][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][]*[,] [ ]*[R][0-7][ ]*";
        string xor = @"[ ]*[X][O]R][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";

        string not = @"[ ]*[N][O][T][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";
        string neg = @"[ ]*[N][E][G][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";

        string[] patterns = { and, or , xor, not, neg };
        return patterns;
    }

    public string ending_pattern()
    {
        return @"[ ]*[R][E][T][U][R][N][ ]*";
    }


    private void loadPatterns()
    {
        this.patterns.Add(this.ending_pattern());

        foreach (string pattern in this.math_patterns())
        {
            this.patterns.Add(pattern);
        }

        foreach (string pattern in this.conditional_patterns())
        {
            this.patterns.Add(pattern);
        }
    }

    public ArrayList reg_patterns()
    {
        return this.patterns;
    }
}
