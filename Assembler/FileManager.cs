using System;
using System.IO;

namespace Assembler
{
    /// <summary>
    /// This File Manager class has three methods.
    /// Reading and Writing to a file and a Logger File
    /// </summary>
    public class FileManager
    {
        private static FileManager _instance = new FileManager();
        public static FileManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FileManager();
                return _instance;
            }
        }

        private FileManager()
        {

        }

        /// <summary>
        /// Reads all text from a file and returns text seperated in lines.
        /// </summary>
        /// <param name="fileName">Name of file to read. Must include file extension</param>
        /// <returns></returns>

        public string[] ToReadFile(string fullFilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(fullFilePath))
                    return null;

                if (!fullFilePath.Contains("."))
                    return null;

                var fileLines = File.ReadAllLines(fullFilePath);

                return fileLines;
            }
            catch (Exception)
            {
                return null;
            }
        }

       
         /// <summary>
         /// Writes to a file the given text
         /// </summary>
         /// <param name="fileName"></param>
         /// <param name="textLines"></param>
         /// <returns></returns>
        public bool ToWriteFile(string fullFilePath, string[] textLines)
        {
            try
            {
                if (string.IsNullOrEmpty(fullFilePath))
                    return false;

                if (!fullFilePath.Contains("."))
                    return false;

                if (textLines == null)
                    return false;

                File.WriteAllLines(fullFilePath, textLines);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Writes to a file the given text
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="textLines"></param>
        /// <returns></returns>
        public bool ToWriteFile(IWritableObject writeable, string filePath)
        {
            string fullFilePath = Path.Combine(filePath, writeable.FileName);
            try
            {
                if (string.IsNullOrEmpty(fullFilePath))
                    return false;

                if (!fullFilePath.Contains("."))
                    return false;

                if (writeable.GetLines() == null)
                    return false;

                File.WriteAllLines(fullFilePath, writeable.GetLines());

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                return false;
            }
        }
    }
}
