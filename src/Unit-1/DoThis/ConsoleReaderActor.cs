using System;
using Akka.Actor;
using WinTail.Messages;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {

        protected override void OnReceive(object message)
        {
            if (message.Equals(Commands.StartCommand))
            {
                DoPrintInstructions(); 
            }

            GetAndValidateInput();
        }

        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (!string.IsNullOrEmpty(message) && string.Equals(message,Commands.ExitCommand,StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate(); 
            }
            else
            {
                Context.ActorSelection("akka://MyActorSystem/user/validationActor").Tell(message);
            }
        }

        private void DoPrintInstructions()
        {
            Console.WriteLine("Please provide a valid URI to a file on disk"); 
        }
    }
}