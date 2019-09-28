using System;
using System.Collections;
using System.Collections.Generic;

namespace Assembler
{
    public class AssemblyLogger : IWritable
    {
        private readonly Queue<LogItem> logs;
        private readonly DateTime startTime;

        public AssemblyLogger()
        {
            this.logs = new Queue<LogItem>();
            this.startTime = DateTime.Now;
            Random r = new Random();
            StatusUpdate($"Started Assembly Log #{r.Next(100, 999)}_{r.Next(1000,9999)} at {this.startTime.ToString()}");
        }

        public void StatusUpdate(string message)
        {
            this.logs.Enqueue(new LogItem(message));
        }

        public void Warning(string message, string line, string address, string previousContent)
        {
            this.logs.Enqueue(new LogItem(message, address, line, previousContent));
        }

        public void Error(string message, string line, string cause)
        {
            this.logs.Enqueue(new LogItem(message, line, cause));
        }

        public string[] GetLines()
        {
            Queue<string> rtn = new Queue<string>();

            foreach (LogItem item in this.logs)
            {
                rtn.Enqueue(item.ToString());
            }

            return rtn.ToArray();
        }

        public IEnumerator<string> GetEnumerator()
        {
            Queue<string> rtn = new Queue<string>();

            foreach (LogItem item in this.logs)
            {
                rtn.Enqueue(item.ToString());
            }

            return rtn.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Queue<string> rtn = new Queue<string>();

            foreach (LogItem item in this.logs)
            {
                rtn.Enqueue(item.ToString());
            }

            return rtn.GetEnumerator();
        }
    }
}
