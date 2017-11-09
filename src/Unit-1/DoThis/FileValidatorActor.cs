using System;
using System.IO;
using Akka.Actor;

namespace WinTail
{
    public class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public FileValidatorActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }
        
        protected override void OnReceive(object message)
        {
            var msg = message as string;
            switch (msg)
            {
                case string s when string.IsNullOrEmpty(s):
                    _consoleWriterActor.Tell(
                        new Messages.NullInputError("Input was blank. Please try again."));
                       
                    Sender.Tell(new Messages.ContinueProcessing());
                    break;

                case string s when IsValid(s):
                    _consoleWriterActor.Tell(new Messages.InputSuccess($"Starting processing a content of {s}..."));
                    
                    Context.ActorSelection("akka://MyActorSystem/user/tailCoordinator")
                        .Tell(new Messages.StartTail(s, _consoleWriterActor));
                    break;

                default:
                    _consoleWriterActor.Tell(new Messages.ValidationError($"The file at {msg} does not exist"));
                    Sender.Tell(new Messages.ContinueProcessing());
                    break;
            }            
        }
        
        private bool IsValid(string message) => File.Exists(message);
    }
}