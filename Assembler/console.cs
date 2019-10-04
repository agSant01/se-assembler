using System;
using System.IO;
using System.Collections.Generic;

namespace Assembler.Parsing
{
    //<summary>
    //Class dedicated to presenting the user with temporary input of a file
    //and parser for said file to generate object code for assembler language.
    //</summary>
    public class Shell
    {
        //Parser and lexer for sifting through the file
        private Parser parser;
        private Lexer lexer;

        private string path;

        ///<summary>Constructor that receives a string of the file to be parsed.
        ///</summary>
        /// <param name="filename"> Name of the file from current directory to be parsed.</param>
        public Shell(string filename)
        {
            path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, @filename);
            try
            {
                this.lexer = new Lexer(FileManager.Instance.ToReadFile(path));
                this.parser = new Parser(this.lexer);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Couldn't open file....");
            }

        }

        ///<summary>
        ///Prints the contents of the provided file into stdout.
        ///</summary>

        public void outputFile()
        {
            if (this.lexer.Equals(null))
                throw new FileNotFoundException();


            Console.WriteLine("Provided File's Contents:\n");
            while (this.lexer.MoveNext())
            {
                Console.WriteLine(this.lexer.CurrrentToken);
            }

            Console.WriteLine("End of File...\n");
        }


          public static void Main()
        {
            string val = "";
            Console.Write("File in Current Dir:");

            while(val.Equals(null) || val.Trim().Equals(""))
            {
                val = Console.ReadLine();
            }
                

            Shell shell = new Shell(val);

            try { shell.outputFile(); }
            catch(FileNotFoundException){Console.WriteLine("Couldn't open file....");}
            

        }
    }

}