using Akka.Actor;

namespace WinTail
{
    public class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }
        
        protected override void OnReceive(object message)
        {
            var msg = message as string;
            switch (msg)
            {
                case null:
                case string s when string.IsNullOrEmpty(s):
                    _consoleWriterActor.Tell(new Messages.NullInputError("No input received."));
                    break;

                case string s when IsValid(s):
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Message was valid."));
                    break;

                default:
                    _consoleWriterActor.Tell(new Messages.ValidationError("Invalid: input had odd number of characters."));
                    break;
            }
            
            Sender.Tell(new Messages.ContinueProcessing());
        }
        
        private bool IsValid(string message) => message.Length % 2 == 0; 
    }
}