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
}
