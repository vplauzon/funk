namespace Funk.Runtime
{
    /// <summary>Represents an error that occurs during Funk execution.</summary>
    public class FunkException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunkException"/> class.
        /// </summary>
        public FunkException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunkException"/> class with a
        /// specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FunkException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunkException"/> class with a
        /// specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="scriptText">The script that caused the error.</param>
        public FunkException(string message, string scriptText)
            : base(message)
        {
            ScriptText = scriptText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunkException"/> class with a
        /// specified error message
        /// and a reference to the exception that caused this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public FunkException(string message, FunkException innerException)
            : base(message, innerException)
        {
        }

        public string ScriptText { get; } = string.Empty;
    }
}
