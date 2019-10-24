using Assembler.Assembler;
using Assembler.Parsing;
using Assembler.Utils;
using System;
using System.IO;
using System.Linq;
using System.Drawing.Drawing2D;
using Assembler.Core;

namespace Assembler
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"
            ___  ___                                       
            |  \/  (_)                                      
            | .  . |_  ___ _ __ ___                         
            | |\/| | |/ __| '__/ _ \                        
            | |  | | | (__| | | (_) |                       
            \_|  |_/_|\___|_|  \___/                        
                                                
                                                
              ___                         _     _           
             / _ \                       | |   | |          
            / /_\ \___ ___  ___ _ __ ___ | |__ | | ___ _ __ 
            |  _  / __/ __|/ _ \ '_ ` _ \| '_ \| |/ _ \ '__|
            | | | \__ \__ \  __/ | | | | | |_) | |  __/ |   
            \_| |_/___/___/\___|_| |_| |_|_.__/|_|\___|_|  

            by: sin cafe

            Detect: Memory undefined variables, syntax error and memory overwrites
            Generate: object file and log file
            
            ");
            string val = "";
            Console.Write("File (with complete Path): ");

            while (val.Equals(null) || val.Trim().Equals(""))
            {
                val = Console.ReadLine();
            }

            Shell shell = new Shell(val);

            try
            {
                shell.ExportFiles();
            }
            catch (Exception err) { Console.WriteLine($"Unexpected Error during runtime: \n\t'{err}'"); }
        }
    }


}
