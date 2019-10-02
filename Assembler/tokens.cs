using System;
using System.Collections;
public class Tokens
{
    private ArrayList tokens;

	public Tokens()
	{
        tokens = new ArrayList();
        this.loadTokens();
	}

    private void loadTokens()
    {
        //The order here is important, do not change
        //I will be indexing these in the same order to provide their
        //correct binary number in the hashtable under operation codes class
        string[] tks = {  
                         @"^LOAD" , @"^LOADIMP",
                        @"^POP", @"^STORE" , @"^PUSH", @"^LOADRIND",
                           @"^STORERIND", @"^ADD", @"^SUB" ,@"^ADDIM" ,
                        @"^SUBIM", @"^AND", @"^OR",
                           @"^XOR",  @"^NOT",@"^NEG", @"^SHIFTR", @"^SHIFTL",
                           @"^ROTAR", @"^ROTAL" , @"^JUMPRIND", @"^JMPADDR",
                           @"^JCONDRIN",@"^JCONDADDR", @"^LOOP",
                           @"^GRT", @"^GRTEQ", @"^EQ",@"^NEQ", @"^NOP",
                           @"^CALL", @"^RETURN"
                        };


        foreach(string t in tks)
        {
            this.tokens.Add(t);
        }
    }

    public ArrayList keywords()
    {
        return this.tokens;
    }

    public string[] register_tokens()
    {
        string[] regs = { @"^R1",@"^R2", @"^R3", @"^R4", @"^R5", @"^R6", @"^R7" };
        return regs;

    }

}
