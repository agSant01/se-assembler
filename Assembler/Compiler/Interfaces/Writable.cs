using System.Collections.Generic;

namespace Assembler
{
    /// <summary>
    /// Interface that makes a class usable by the FileManeger. So that 
    /// FileManager can extract contents and write them to a file.
    /// </summary>
    public interface IWritableObject : IEnumerator<string>
    {
        /// <summary>
        /// Used to get all the lines that are going to be outputed to a file.
        /// </summary>
        /// <returns>
        /// A string array in which the items represent the lines of a file.
        /// </returns>

        string FileName { get; }

        string[] GetLines();
    }
}
