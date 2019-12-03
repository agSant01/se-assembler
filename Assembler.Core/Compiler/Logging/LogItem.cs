namespace Assembler
{
    /// <summary>
    /// Class used to represent an assebly log object. Provides the 
    /// message, type of log, line, errorCause (if error), and other
    /// useful information.
    /// </summary>
    class LogItem
    {

        /// <summary>
        /// Getter for message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Getter for log type
        /// </summary>
        public string Type { get; }

        // used for warning and error only
        private readonly string address;
        private readonly string line;
        private readonly string previousContent;
        private readonly string errorCause;

        /// <summary>
        /// Used to create a simple Log object.
        /// This object is a 'STATUS'
        /// </summary>
        /// <param name="message">Message to log.</param>
        public LogItem(string message)
        {
            this.Type = "STATUS";
            this.Message = message;
            this.address = null;
            this.line = null;
        }

        /// <summary>
        /// Used to create a WARNING log object.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="address">Address in memory that is been overwritten.</param>
        /// <param name="line">Line in source code file that overwritting is being executed.</param>
        /// <param name="previousContent">Content being overwritten.</param>
        public LogItem(string message, string address, string line, string previousContent)
        {
            this.previousContent = previousContent;
            this.Message = message;
            this.address = address;
            this.Type = "WARNING";
            this.line = line;
        }

        /// <summary>
        /// Used to create an ERROR log object.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="line">Line in source code file that errors ocurrs.</param>
        /// <param name="errorCause">Cause of the error.</param>
        public LogItem(string message, string line, string errorCause)
        {
            this.Type = "ERROR";
            this.Message = message;
            this.address = null;
            this.line = line;
            this.errorCause = errorCause;
        }

        /// <summary>
        /// ToString Override for the LogItem object
        /// </summary>
        /// <returns>Formatted log ready to display.</returns>
        public override string ToString()
        {
            if (this.Type == "STATUS")
            {
                return $"[{this.Type}] {this.Message}";
            }

            if (this.Type == "WARNING")
            {
                return $"[{this.Type}] {this.Message}. Address {this.address} overwrite [content: '{this.previousContent}'] in instruction {line}";
            }

            if (this.Type == "ERROR")
            {
                return $"[{this.Type}] {this.Message} on instruction {this.line}. Cause: {this.errorCause}";
            }

            return "Invalid Log Type";
        }
    }
}
