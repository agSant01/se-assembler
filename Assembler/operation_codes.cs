using System;
using System.Collections;

public class OperationCodes
{
    //To be used when converting the text into object code
    //the table will store a value in integer type but that value must be converted to binary
    private Hashtable code_lookup_table, register_table;
	public OperationCodes()
	{
        code_lookup_table = new Hashtable();
        register_table = new Hashtable();
        this.setup_code_table();
        this.setup_register_table();
	}

    //receives the tokens and translates to object code
    public int[] object_code(string token)
    {
        int[] bits = { };

        return bits;
    }

    private void setup_register_table()
    {
        this.register_table.Add("R1", 0);
        this.register_table.Add("R2", 1);
        this.register_table.Add("R3", 2);
        this.register_table.Add("R4", 3);
        this.register_table.Add("R5", 4);
        this.register_table.Add("R6", 5);
        this.register_table.Add("R7", 6);

    }

    private void setup_code_table()
    {
        
        //Add all the operations and their respective representation in DECIMAL

        this.code_lookup_table.Add("LOAD", 0);
        this.code_lookup_table.Add("LOADIMP", 1);
        this.code_lookup_table.Add("POP", 2);
        this.code_lookup_table.Add("STORE", 3);
        this.code_lookup_table.Add("PUSH", 4);
        this.code_lookup_table.Add("LOADRIND", 5);

        this.code_lookup_table.Add("STORERIND", 6);
        this.code_lookup_table.Add("ADD", 7);
        this.code_lookup_table.Add("SUB", 8);
        this.code_lookup_table.Add("ADDIM", 9);
        this.code_lookup_table.Add("SUBIM", 10);

        this.code_lookup_table.Add("AND", 11);
        this.code_lookup_table.Add("OR", 12);
        this.code_lookup_table.Add("XOR", 13);
        this.code_lookup_table.Add("NOT", 14);
        this.code_lookup_table.Add("NEG", 15);
        this.code_lookup_table.Add("SHIFTR", 16);
        this.code_lookup_table.Add("SHIFTL", 17);
        this.code_lookup_table.Add("ROTAR", 18);
        this.code_lookup_table.Add("ROTAL", 19);


        this.code_lookup_table.Add("JMPRIND", 20);
        this.code_lookup_table.Add("JMPADDR", 21);
        this.code_lookup_table.Add("JCONDRIN", 22);
        this.code_lookup_table.Add("JCONDADDR", 23);
        this.code_lookup_table.Add("LOOP", 24);
        this.code_lookup_table.Add("GRT", 25);
        this.code_lookup_table.Add("GRTEQ", 26);
        this.code_lookup_table.Add("EQ",27 );
        this.code_lookup_table.Add("NEQ", 28);
        this.code_lookup_table.Add("NOP", 29);
        this.code_lookup_table.Add("CALL",30 );
        this.code_lookup_table.Add("RETURN",31 );

    }
}
