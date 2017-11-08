using Akka.Actor;

namespace WinTail
{
    #region Program
    
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            var consoleWriterProps = Props.Create<ConsoleWriterActor>();
            var consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriter");
            var tailCoordinatorProps = Props.Create<TailCoordinatorActor>();
            var tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinator");
            var fileValidatorProps = Props.Create<FileValidatorActor>(consoleWriterActor, tailCoordinatorActor);
            var fileValidatorActor = MyActorSystem.ActorOf(fileValidatorProps, "fileValidator");
            var consoleReaderProps = Props.Create<ConsoleReaderActor>(fileValidatorActor);
            var consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, "consoleReader");
            
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
    
    #endregion
}
