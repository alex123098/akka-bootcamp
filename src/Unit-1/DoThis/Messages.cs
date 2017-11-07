namespace WinTail
{
    public static class Messages
    {
        #region Neutral/system messages

        /// <summary>
        /// Tells actor to continue processing messages
        /// </summary>
        public class ContinueProcessing { }

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