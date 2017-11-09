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
            Context.ActorSelection("akka://MyActorSystem/user/fileValidator").Tell(input);
        }

        private string ReadInput()
        {
            return Console.ReadLine();
        }

        private void DoPrintInstructions()
        {
            Console.WriteLine("Provide a path to the file to watch.");
        }

        #endregion
    }
}