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
        switch (line)
        {
            //case this.patterns

            default:
                break;
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
        string or= @"[ ]*[O][R][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][]*[,] [ ]*[R][0-7][ ]*";
        string xor = @"[ ]*[X][O]R][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";

        string not = @"[ ]*[N][O][T][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";
        string neg = @"[ ]*[N][E][G][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";

        string[] patterns = { and, or , xor, not, neg };
        return patterns;
    }

    public string ending_pattern()
    {
        string retur = @"[ ]*[R][E][T][U][R][N][ ]*";
        return retur;
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
}
