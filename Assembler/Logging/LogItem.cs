namespace Assembler
{
    /// <summary>
    /// Class used to represent an assebly log object. Provides the 
    /// message, type of log, line, errorCause (if error), and other
    /// useful information.
    /// </summary>
    class LogItem
    {
        // general items
        private string message;
        private string type;

        // used for warning and error only
        private string address;
        private string line;
        private string previousContent;
        private string errorCause;

        /// <summary>
        /// Used to create a simple Log object.
        /// This object is a 'STATUS'
        /// </summary>
        /// <param name="message">Message to log.</param>
        public LogItem(string message)
        {
            this.type = "STATUS";
            this.message = message;
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
            this.message = message;
            this.address = address;
            this.type = "WARNING";
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
            this.type = "ERROR";
            this.message = message;
            this.address = null;
            this.line = line;
            this.errorCause = errorCause;
        }

        /// <summary>
        /// Getter for message
        /// </summary>
        public string Message
        {
            get => message;
        }

        /// <summary>
        /// Getter for log type
        /// </summary>
        public string Type
        {
            get => type;
        }

        /// <summary>
        /// ToString Override for the LogItem object
        /// </summary>
        /// <returns>Formatted log ready to display.</returns>
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
