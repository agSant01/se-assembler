using System;
using System.Collections;
using System.Collections.Generic;

namespace Assembler
{
    /// <summary>
    /// Used to keep a track of the events during assembly of the source code.
    /// </summary>
    public class AssemblyLogger : IWritableObject
    {
        
        /// <summary>
        /// Internal record of all log events
        /// </summary>
        private readonly Queue<LogItem> logs;
        /// <summary>
        /// Internal Enumerator of the log events
        /// </summary>
        private IEnumerator<LogItem> logIterator;

        /// <summary>
        /// Create a new instance of AssemblyLogger
        /// </summary>
        public AssemblyLogger(string asmFileName)
        {
            logs = new Queue<LogItem>();

            Random r = new Random();
            FileName = $"{asmFileName}_AssemblyLog_{r.Next(100, 999)}_{r.Next(1000, 9999)}";

            StatusUpdate($"Started Assembly Log " +
                FileName + $" at {DateTime.Now.ToString()}");

            FileName += ".txt";
        }

        /// <summary>
        /// Add a new STATUS log to the events record.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void StatusUpdate(string message)
        {
            this.logs.Enqueue(new LogItem(message));
            // Everytime a new event is recorded reset the internal Enumerator
            Reset();
        }


        /// <summary>
        /// Used to create a WARNING log object.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="line">Line in source code file that overwritting is being executed.</param>
        /// <param name="address">Address in memory that is been overwritten.</param>
        /// <param name="previousContent">Content being overwritten.</param>
        public void Warning(string message, string line, string address, string previousContent)
        {
            this.logs.Enqueue(new LogItem(message, address, line, previousContent));
            // Everytime a new event is recorded reset the internal Enumerator
            Reset();
        }

        /// <summary>
        /// Used to create an ERROR log object.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="line">Line in source code file that errors ocurrs.</param>
        /// <param name="cause">Cause of the error.</param>
        public void Error(string message, string line, string cause)
        {
            this.logs.Enqueue(new LogItem(message, line, cause));
            // Everytime a new event is recorded reset the internal Enumerator
            Reset();
        }

        /// <summary>
        /// Used to get all the lines that are going to be outputed to a file.
        /// </summary>
        /// <returns>
        /// A string array in which the items represent the lines of a file.
        /// </returns>
        public string[] GetLines()
        {
            Queue<string> rtn = new Queue<string>();

            foreach (LogItem item in logs)
            {
                rtn.Enqueue(item.ToString());
            }

            return rtn.ToArray();
        }

        /// <summary>
        /// Getter for current item in the Enumerator
        /// </summary>
        public string Current {
            get
            {
                if (logIterator == null) Reset();
                return logIterator.Current.ToString();
            }
        }

        /// <summary>
        /// Getter for current item in the iterator
        /// </summary>
        object IEnumerator.Current => Current;

        /// <summary>
        /// Getter for full file path to save the log
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Used to travers the Enumerator
        /// </summary>
        /// <returns>True if there is a next item.</returns>
        public bool MoveNext() {
            if (logIterator == null) Reset();
            return logIterator.MoveNext();
        }

        /// <summary>
        /// Resets the logs Enumerator
        /// </summary>
        public void Reset() => logIterator = logs.GetEnumerator();

        /// <summary>
        /// Dispose the logs Enumerator.
        /// </summary>
        public void Dispose() => logIterator = null;
    }
}
