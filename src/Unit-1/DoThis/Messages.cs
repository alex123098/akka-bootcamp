using Akka.Actor;

namespace WinTail
{
    public static class Messages
    {
        #region Neutral/system messages

        /// <summary>
        /// Tells actor to continue processing messages
        /// </summary>
        public class ContinueProcessing { }

        /// <summary>
        /// Start tailing a file at specified path
        /// </summary>
        public class StartTail
        {
            public string FilePath { get; }
            public IActorRef ReporterActor { get; }

            public StartTail(string filePath, IActorRef reporterActor)
            {
                FilePath = filePath;
                ReporterActor = reporterActor;
            }
        }

        /// <summary>
        /// Signal to read an initial content of a file
        /// </summary>
        public class InitialRead
        {
            public string FileName { get; }
            public string Text { get; }

            public InitialRead(string fileName, string text)
            {
                FileName = fileName;
                Text = text;
            }
        }

        /// <summary>
        /// Signals that file has changed its content
        /// </summary>
        public class FileChanged
        {
            public string FileName { get; }

            public FileChanged(string fileName)
            {
                FileName = fileName;
            }
        }

        #endregion

        #region Success messages

        /// <summary>
        /// Base class for signaling of valid user input
        /// </summary>
        public class InputSuccess
        {
            public string Reason { get; }

            public InputSuccess(string reason)
            {
                Reason = reason;
            }
        }

        #endregion

        #region Error messages

        /// <summary>
        /// Indicates an error during accessing a file.
        /// </summary>
        public class FileError
        {
            public string FileName { get; }
            public string Reason { get; }

            public FileError(string fileName, string reason)
            {
                FileName = fileName;
                Reason = reason;
            }
        }
        
        /// <summary>
        /// Base class for signaling of invalid user input
        /// </summary>
        public class InputError
        {
            public string Reason { get; }

            public InputError(string reason)
            {
                Reason = reason;
            }
        }

        /// <summary>
        /// User provided empty input
        /// </summary>
        public class NullInputError : InputError
        {
            public NullInputError(string reason) : base(reason)
            { }
        }
        
        /// <summary>
        /// User provided invalid input (currently w/ odd # of chars)
        /// </summary>
        public class ValidationError : InputError
        {
            public ValidationError(string reason) : base(reason)
            { }
        }

        #endregion
    }
}