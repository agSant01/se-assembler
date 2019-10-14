using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Utils
{
    public static class HexOperations
    {
        public static string OperationToHex(string hex1, string hex2, string operation)
        {
            int element1 = Convert.ToInt32(hex1, 16);
            int element2 = Convert.ToInt32(hex2, 16);

            switch (operation)
            {
                case "ADD":  break;
                case "SUB":  break;
                case "DIV":  break;
                case "MULT":  break;

            }
            return "";
        }
    }
}
