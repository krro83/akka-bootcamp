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
        
        private IActorRef _validationActor;

        public ConsoleReaderActor(IActorRef validationActor)
        {
            _validationActor = validationActor;
        }

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
                _validationActor.Tell(message); 
            }
        }

        private void DoPrintInstructions()
        {
            Console.WriteLine("Please provide a valid URI to a file on disk"); 
        }
    }
}