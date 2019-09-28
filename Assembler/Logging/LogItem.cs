namespace Assembler
{
    class LogItem
    {
        private string message;
        private string type;

        // used for warning and error only
        private string address;
        private string line;
        private string previousContent;
        private string errorCause;

        public LogItem(string message)
        {
            this.type = "STATUS";
            this.message = message;
            this.address = null;
            this.line = null;
        }

        public LogItem(string message, string address, string line, string previousContent)
        {
            this.previousContent = previousContent;
            this.message = message;
            this.address = address;
            this.type = "WARNING";
            this.line = line;
        }

        public LogItem(string message, string line, string errorCause)
        {
            this.type = "ERROR";
            this.message = message;
            this.address = null;
            this.line = line;
            this.errorCause = errorCause;
        }

        public string Message
        {
            get => message;
        }

        public string Type
        {
            get => type;
        }

        public override string ToString()
        {
            if (this.type == "STATUS")
            {
                return $"[{this.type}] {this.message}";
            }

            if (this.type == "WARNING")
            {
                return $"[{this.type}] {this.message}. Address {this.address} overwrite [content: '{this.previousContent}'] at line {this.line}";
            }

            if (this.type == "ERROR")
            {
                return $"[{this.type}] {this.message} at line {this.line}. Cause: {this.errorCause}";
            }

            return "Invalid Log Type";
        }
    }
}
