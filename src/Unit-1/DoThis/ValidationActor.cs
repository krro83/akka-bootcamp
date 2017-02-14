using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinTail.Messages;

namespace WinTail
{
    class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;
        private readonly string NoInputErrorMessage = "No input was received";
        private readonly string SuccessInputMessage = "Thank you! Message was validated.";
        private readonly string ValidationErrorMessage = "Invalid: Message has an odd number of characters.";

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor; 
        }
        protected override void OnReceive(object message)
        {
            var msg = message as string;

            if (string.IsNullOrEmpty(msg))
            {
                Self.Tell(new Messages.NullInputError(NoInputErrorMessage));
            }
            else
            {
                bool valid = IsValid(msg);
                if (valid)
                {
                    _consoleWriterActor.Tell(new InputSuccess(SuccessInputMessage), Self);
                }
                else
                {
                    _consoleWriterActor.Tell(new ValidationError(ValidationErrorMessage));
                }
            }
            Sender.Tell(new ContinueProcessing());
        }

        private bool IsValid(string message)
        {
            return message.Length % 2 == 0;
        }
    }
}
