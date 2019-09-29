using System;
using System.Text.RegularExpressions;

//Verifies that Syntax is correct and makes sure operations carry proper structure
public class SyntaxAnalyzer
{
	public SyntaxAnalyzer()
	{
	}

    // We use regular expressions to determine if the structure of the OP-Code is correct
    public bool is_Proper_Syntax(string line)
    {
        return false;
    }

    public string math_pattern()
    {
        string add = @"[ ]*[A][D][D][ ]+ [R][0-7][ ]*[,][ ]*[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";
        string sub = @"[ ]*[S][U][B][ ]+ [R][0-7][ ]*[,][ ]*[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";

        string addim = @"[ ]*[A][D][D][I][M][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";
        string subim = @"[ ]*[S][U][B][I][M][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";

        return @"\b[A]\w+";
    }

    public string conditional_pattern()
    {
        string and = @"[ ]*[A][N][D][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";
        string or= @"[ ]*[O][R][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][]*[,] [ ]*[R][0-7][ ]*";
        string xor = @"[ ]*[X][O]R][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";

        string not = @"[ ]*[N][O][T][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";
        string neg = @"[ ]*[N][E][G][ ]+[R][0-7][ ]*[,][ ]*[R][0-7][ ]*";
        return @"";
    }

    public string ending_pattern()
    {
        string retur = @"[ ]*[R][E][T][U][R][N][ ]*";
        return retur;
    }

}
