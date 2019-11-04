using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Utils
{
    public class Comparator
    {

        private int[] indexes;

        public Comparator()
        {
            
        }


        public bool HasChanged(string line, string new_line)
        {


                if (line != new_line)
                    return true;
           
            return false;
        }


        public int[] ChangedIndexes(string[] lines, string[] new_lines)
        {
            int[] indxs = {};
            int i;
            int j = 0;
            for(i = 0; i < lines.Length || i < new_lines.Length; i++)
            {
                if(HasChanged(lines[i] , new_lines[i]))
                {
                    indxs[j++] = i;
                }
            }

            return indxs;
        }
    }
}
