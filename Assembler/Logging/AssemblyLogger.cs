using System;
using System.Collections;
using System.Collections.Generic;

namespace Assembler
{
    public class AssemblyLogger : IWritable
    {
        private readonly Queue<LogItem> logs;
        private readonly DateTime startTime;
        private IEnumerator<LogItem> logIterator;

        public AssemblyLogger()
        {
            logs = new Queue<LogItem>();
            startTime = DateTime.Now;
            Random r = new Random();
            StatusUpdate($"Started Assembly Log #{r.Next(100, 999)}_{r.Next(1000, 9999)} at {this.startTime.ToString()}");
        }

        public void StatusUpdate(string message)
        {
            this.logs.Enqueue(new LogItem(message));
            Reset();
        }

        public void Warning(string message, string line, string address, string previousContent)
        {
            this.logs.Enqueue(new LogItem(message, address, line, previousContent));
            Reset();
        }

        public void Error(string message, string line, string cause)
        {
            this.logs.Enqueue(new LogItem(message, line, cause));
            Reset();
        }

        public string[] GetLines()
        {
            Queue<string> rtn = new Queue<string>();

            foreach (LogItem item in logs)
            {
                rtn.Enqueue(item.ToString());
            }

            return rtn.ToArray();
        }

        public string Current {
            get
            {
                if (logIterator == null) Reset();
                return logIterator.Current.ToString();
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext() {
            if (logIterator == null) Reset();
            return logIterator.MoveNext();
        }

        public void Reset() => logIterator = logs.GetEnumerator();

        public void Dispose() => logIterator = null;
    }
}
