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
        private const string ExitCommand = "exit";
        public const string StartCommand = "start";

        private readonly IActorRef _validationActor;

        public ConsoleReaderActor(IActorRef validationActor)
        {
            _validationActor = validationActor;
        }
        
        
        protected override void OnReceive(object message)
        {
            if (StartCommand.Equals(message))
            {
                DoPrintInstructions();
            }
            var input = ReadInput();
            
            ProcessInput(input);
        }

        
        #region Internal methods
        
        private void ProcessInput(string input)
        {
            if (string.Equals(input, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate();
                return;
            }
            _validationActor.Tell(input);
        }

        private string ReadInput()
        {
            return Console.ReadLine();
        }

        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }

        #endregion
    }
}