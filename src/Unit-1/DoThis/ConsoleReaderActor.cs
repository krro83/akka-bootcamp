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
        
        private IActorRef _consoleWriterActor;

        private readonly string NoInputErrorMessage = "No input was received";
        private readonly string SuccessInputMessage = "Thank you! Message was validated.";
        private readonly string ValidationErrorMessage = "Invalid: Message has an odd number of characters.";


        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(Commands.StartCommand))
            {
                DoPrintInstructions(); 
            }
            else if (message is InputError)
            {
                _consoleWriterActor.Tell(message as InputError, Self); 
            }

            GetAndValidateInput();
        }

        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (string.IsNullOrEmpty(message))
            {
                Self.Tell(new Messages.NullInputError(NoInputErrorMessage)); 
            }
            else if (string.Equals(message,Commands.ExitCommand,StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate(); 
            }
            else
            {
                bool valid = IsValid(message);
                if (valid)
                {
                    _consoleWriterActor.Tell(new InputSuccess(SuccessInputMessage), Self);
                    Self.Tell(new ContinueProcessing()); 
                }
                else
                {
                    Self.Tell(new ValidationError(ValidationErrorMessage)); 
                }
            }
        }

        private bool IsValid(string message)
        {
            return message.Length % 2 == 0; 
        }

        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }
    }
}