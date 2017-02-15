using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTail.Messages
{
    public class Commands
    {
        public static readonly string ContinueCommand = "continue";
        public static readonly string StartCommand = "start";
        public const string ExitCommand = "exit";
    }

    #region Neutral and System Messages
    public class ContinueProcessing{ }
    #endregion

    #region Success Messages
    public class InputSuccess
    {
        public string Reason { get; }
        public InputSuccess(string reason)
        {
            Reason = reason;
        }
    }
    #endregion

    #region Error Messages
    public class InputError
    {
        public string Reason { get; }

        public InputError(string reason)
        {
            Reason = reason; 
        }
    }

    public class NullInputError : InputError
    {
        public NullInputError(string reason) : base(reason) { }
    }

    public class ValidationError : InputError
    {
        public ValidationError(string reason) : base(reason) { }
    }
    #endregion

    #region Tail Messages
    public class StartTail
    {
        public StartTail(string filePath, IActorRef reporterActor)
        {
            FilePath = filePath;
            ReporterActor = reporterActor;
        }

        public string FilePath { get; private set; }

        public IActorRef ReporterActor { get; private set; }
    }

    /// <summary>
    /// Stop tailing the file at user-specified path.
    /// </summary>
    public class StopTail
    {
        public StopTail(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; private set; }
    }
    #endregion

    #region File Messages
    public class FileWrite
    {
        public FileWrite(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; private set; }
    }

    /// <summary>
    /// Signal that the OS had an error accessing the file.
    /// </summary>
    public class FileError
    {
        public FileError(string fileName, string reason)
        {
            FileName = fileName;
            Reason = reason;
        }

        public string FileName { get; private set; }

        public string Reason { get; private set; }
    }

    /// <summary>
    /// Signal to read the initial contents of the file at actor startup.
    /// </summary>
    public class InitialRead
    {
        public InitialRead(string fileName, string text)
        {
            FileName = fileName;
            Text = text;
        }

        public string FileName { get; private set; }
        public string Text { get; private set; }
    }

    #endregion
}
