using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
    /// </summary>
    public class ConsoleReaderActor : UntypedActor
    {
        public const string StartCommand = "start";
        public const string ExitCommand = "exit";
        
        private readonly IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case StartCommand:
                    DoPrintInstructions();
                    break;

                case Messages.InputError error:
                    _consoleWriterActor.Tell(error);
                    break;
            }
            GetAndValidateInput();
        }

        #region Internal methods

        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            switch (message)
            {
                case null:
                case string s when string.IsNullOrEmpty(s):
                    Self.Tell(new Messages.NullInputError("No input received."));
                    break;

                case ExitCommand:
                    Context.System.Terminate();
                    break;

                case string msg when IsValid(msg):
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Message was valid."));

                    // continue reading messages from console
                    Self.Tell(new Messages.ContinueProcessing());
                    break;

                default:
                    Self.Tell(new Messages.ValidationError("Invalid: input had odd number of characters."));
                    break;
            }
        }

        private bool IsValid(string message) => message.Length % 2 == 0; 

        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }

        #endregion
    }
}