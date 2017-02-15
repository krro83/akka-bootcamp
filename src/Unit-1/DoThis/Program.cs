using System;
using Akka.Actor;
using WinTail.Messages;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;
        public static string ConsoleWriterActorName = "consoleWriterActor";
        public static string ConsoleReaderActorName = "consoleReaderActor";
        public static string ValidationActorName = "validationActor";
        public static string TailCoordinatorActorName = "tailCoordinatorActor";
         

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            var consoleWriterProps = Props.Create(() => new ConsoleWriterActor());
            var consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, ConsoleWriterActorName);

            var tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            var tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, TailCoordinatorActorName); 

            var fileValidationActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor, tailCoordinatorActor));
            var validationActor = MyActorSystem.ActorOf(fileValidationActorProps, ValidationActorName); 

            var consoleReaderProps = Props.Create(() => new ConsoleReaderActor(validationActor));
            var consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, ConsoleReaderActorName); 

            // tell console reader to begin
            consoleReaderActor.Tell(Commands.StartCommand); 

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();

            //todo: påbörja lesson 5
        }
    }
    #endregion
}
