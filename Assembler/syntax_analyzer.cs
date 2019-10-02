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
        Regex add = new Regex(@"^[ ]*ADD[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*,[ ]*R[1-7][ ]*");
        Regex sub =  new Regex(@"^[ ]*SUB[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*,[ ]*R[1-7][ ]*");
         
        Regex addim = new Regex(@"^[ ]*ADDIM[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*");
        Regex subim = new Regex(@"^[ ]*SUBIM[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*");

        Regex []patterns = { add, sub, addim, subim };

        return patterns;
    }

    public Regex[] control_flow_patterns()
    {
        Regex jumprind = new Regex(@"^[ ]*JUMPRIND[ ]+R[1-7][ ]*");
        Regex jumpaddr = new Regex(@"^[ ]*JUMPADDR[ ]+[0-9][0-9][0-9][0-9][ ]*");//FIX this for 11-bits 2^11

        Regex jcondrin = new Regex(@"^[ ]*JCONDRIN[ ]+R[1-7][ ]*");
        Regex jconaddr = new Regex(@"^[ ]*JCONDADDR[ ]+[0-9][0-9][0-9][0-9][ ]*");//FIX this for 11-bits 2^11

        Regex loop = new Regex(@"^[ ]*LOOP[ ]+[ ]*R[1-7][ ]*,[ ]*[0-9][0-9][0-9][0-9][ ]*");

        Regex greater = new Regex(@"^[ ]*GRT[ ]+R[1-7],[ ]*R[1-7][ ]*");
        Regex grt_eq = new Regex(@"^[ ]*GRTEQ[ ]+R[1-7],[ ]*R[1-7][ ]*");

        Regex equal = new Regex(@"^[ ]*EQ[ ]+R[1-7],[ ]*R[1-7][ ]*");
        Regex n_equal = new Regex(@"^[ ]*NEQ[ ]+R[1-7],[ ]*R[1-7][ ]*");
        Regex[] patterns = { jumprind,jumpaddr, jcondrin, jconaddr,
                            loop, greater, grt_eq, equal, n_equal
           };

        return patterns;
    }


    public Regex[] conditional_patterns()
    {
        Regex and = new Regex(@"^[ ]*AND[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*,[ ]*R[1-7][ ]*");
        Regex or =  new Regex(@"^[ ]*OR[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*, [ ]*R[1-7][ ]*");
        Regex xor = new Regex(@"^[ ]*XOR[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*,[ ]*R[1-7][ ]*");

        Regex not = new Regex( @"^[ ]*NOT[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*");
        Regex neg = new Regex( @"^[ ]*NEG[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*");

        Regex shift_r = new Regex(@"^[ ]*SHIFTR[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*,[ ]*R[1-7][ ]*");
        Regex shift_l = new Regex(@"^[ ]*SHIFTL[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*,[ ]*R[1-7][ ]*");

        Regex rotar= new Regex(@"^[ ]*ROTAR[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*,[ ]*R[1-7][ ]*");
        Regex rotal = new Regex(@"^[ ]*ROTAL[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*,[ ]*R[1-7][ ]*");



        Regex[] patterns = { and, or , xor, not, neg , shift_r, shift_l, rotar,rotal};
        return patterns;
    }

    public Regex[] movement_patterns()
    {
        Regex load = new Regex(@"^[ ]*LOAD[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*");

        Regex load_im = new Regex(@"^[ ]*LOADIM[ ]+R[1-7][ ]*,[ ]*#[0-9][0-9][0-9][0-9][ ]*");
        Regex pop = new Regex(@"^[ ]*POP[ ]+R[1-7]");

        Regex store = new Regex(@"^[ ]*STORE[ ]+#[0-9][0-9][0-9][0-9][ ]*,[ ]*R[1-7][ ]*");
        Regex push = new Regex(@"^[ ]*PUSH[ ]+R[1-7]");
        Regex loadrind = new Regex(@"^[ ]*LOADRIND[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*");
        Regex storerind = new Regex(@"^[ ]*STORERIND[ ]+R[1-7][ ]*,[ ]*R[1-7][ ]*");

        Regex[] movements = {load, load_im, pop, store, push, loadrind,storerind};
        return movements;
    }

    public Regex constant_pattern()
    {
        return new Regex(@"^[ ]*#[0-9]+");
    }

    public Regex ending_pattern()
    {
        return new Regex( @"^[ ]*RETURN[ ]*");
    }


    private void loadPatterns()
    {
        this.patterns.Add(this.ending_pattern());
        this.patterns.Add(this.constant_pattern());

        this.patterns.Add(new Regex(@"^[ ]*NOP[ ]*"));
        this.patterns.Add(new Regex(@"^[ ]*CALL[ ]+[0-9][0-9][0-9][0-9]"));//TODO fix the problem with address detection
        foreach (Regex rg in movement_patterns())
        {
            this.patterns.Add(rg);
        }

        foreach (Regex rg in control_flow_patterns())
        {
            this.patterns.Add(rg);
        }

        
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
