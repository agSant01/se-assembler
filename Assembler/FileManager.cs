using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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

        public string[] ToReadFile(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return null;

                if (!fileName.Contains("."))
                    return null;

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;
                var fileLines = File.ReadAllLines(filePath);

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
        public string ToWriteFile(string fileName, string[] textLines)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return null;

                if (!fileName.Contains("."))
                    return null;

                if (textLines == null)
                    return null;

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;
                File.WriteAllLines(filePath, textLines);
                return filePath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Appends the given log message to a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="message"></param>
        public bool LoggerFile(string fileName, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

                if (!fileName.Contains("."))
                    return false;

                if (message == null)
                    return false;

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;
                File.AppendAllText(filePath, message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
