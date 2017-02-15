using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinTail.Messages;

namespace WinTail
{
    public class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;
        private readonly IActorRef _tailCoordinatorActor;

        private readonly string NoInputErrorMessage = "No input was received";

        public FileValidatorActor(IActorRef consoleWriterActor, IActorRef tailCoordinatorActor)
        {
            _consoleWriterActor = consoleWriterActor;
            _tailCoordinatorActor = tailCoordinatorActor; 
        }
        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                _consoleWriterActor.Tell(new NullInputError(NoInputErrorMessage));
                Sender.Tell(new ContinueProcessing());
            }
            else
            {
                var isValid = Validate(msg);
                if (isValid)
                {
                    _consoleWriterActor.Tell(new InputSuccess($"Starting processing for {msg}"));
                    _tailCoordinatorActor.Tell(new StartTail(msg, _consoleWriterActor));
                }
                else
                {
                    _consoleWriterActor.Tell(new ValidationError($"{msg} is not an existing URI on disk"));
                    Sender.Tell(new ContinueProcessing());
                }

            }
        }

        private bool Validate(string url)
        {
            return File.Exists(url); 
        }
    }
}
