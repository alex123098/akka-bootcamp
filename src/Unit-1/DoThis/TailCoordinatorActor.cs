using System;
using Akka.Actor;

namespace WinTail
{
    public class TailCoordinatorActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case Messages.StartTail startMessage:
                    Context.ActorOf(
                        Props.Create(() => new TailActor(startMessage.ReporterActor, startMessage.FilePath)));
                    break;
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                10, 
                TimeSpan.FromSeconds(30),
                ex =>
                {
                    switch (ex)
                    {
                        case ArithmeticException _: return Directive.Resume;
                        case NotSupportedException _: return Directive.Stop;
                        default: return Directive.Restart;
                    }
                });
        }
    }
}