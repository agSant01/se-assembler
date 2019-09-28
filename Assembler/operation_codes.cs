using System;
using System.Collections;

public class OperationCodes
{
    private Hashtable lookup_table;
	public OperationCodes()
	{
        lookup_table = new Hashtable();
        this.setup_table();
	}

    public int[] machine_language(string token)
    {
        int[] bits = { };

        return bits;
    }

    private void setup_table()
    {
        
        //this.lookup_table.Add("JMP", 20);

        this.lookup_table.Add("LOAD", 0);
        this.lookup_table.Add("LOADIMP", 1);
        this.lookup_table.Add("POP", 2);
        this.lookup_table.Add("STORE", 3);
        this.lookup_table.Add("PUSH", 4);
        this.lookup_table.Add("LOADRIND", 5);

        this.lookup_table.Add("STORERIND", 6);
        this.lookup_table.Add("ADD", 7);
        this.lookup_table.Add("SUB", 8);
        this.lookup_table.Add("ADDIM", 9);
        this.lookup_table.Add("SUBIM", 10);

        this.lookup_table.Add("AND", 11);
        this.lookup_table.Add("OR", 12);
        this.lookup_table.Add("XOR", 13);
        this.lookup_table.Add("NOT", 14);
        this.lookup_table.Add("NEG", 15);
        this.lookup_table.Add("SHIFTR", 16);
        this.lookup_table.Add("SHIFTL", 17);
        this.lookup_table.Add("ROTAR", 18);
        this.lookup_table.Add("ROTAL", 19);


        this.lookup_table.Add("JMPRIND", 20);
        this.lookup_table.Add("JMPADDR", 21);
        this.lookup_table.Add("JCONDRIN", 22);
        this.lookup_table.Add("JCONDADDR", 23);
        this.lookup_table.Add("LOOP", 24);
        this.lookup_table.Add("GRT", 25);
        this.lookup_table.Add("GRTEQ", 26);
        this.lookup_table.Add("EQ",27 );
        this.lookup_table.Add("NEQ", 28);
        this.lookup_table.Add("NOP", 29);
        this.lookup_table.Add("CALL",30 );
        this.lookup_table.Add("RETURN",31 );



    }
}
