using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Assembler
{
    public interface IWritable : IEnumerator<string>
    {
        string[] GetLines();
    }
}
