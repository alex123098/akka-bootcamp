using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for serializing message writes to the console.
    /// (write one message at a time, champ :)
    /// </summary>
    public class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case Messages.InputError error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(error.Reason);
                    Console.ResetColor();
                    break;

                case Messages.InputSuccess successMessage:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(successMessage.Reason);
                    Console.ResetColor();
                    break;

                default:
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
