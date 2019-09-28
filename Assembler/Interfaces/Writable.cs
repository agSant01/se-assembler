using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Assembler
{
    public interface IWritable : IEnumerable<string>
    {
        string[] GetLines();
    }
}
