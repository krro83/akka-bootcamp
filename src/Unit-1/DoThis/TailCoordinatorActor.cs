using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinTail.Messages;

namespace WinTail
{
    public class TailCoordinatorActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is StartTail)
            {
                var msg = message as StartTail;
                var TailActorProps = Props.Create(() => new TailActor(msg.ReporterActor, msg.FilePath));
                Context.ActorOf(TailActorProps);
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            var maxRetries = 10;
            var timeRange = TimeSpan.FromSeconds(30); 

            return new OneForOneStrategy(
               maxRetries,
               timeRange,
                ex => 
                {
                    if (ex is ArithmeticException) return Directive.Resume;
                    else if (ex is NotSupportedException) return Directive.Stop;
                    else return Directive.Restart; 
                }); 
        }
    }
}
