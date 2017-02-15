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
    class FileObserver : IDisposable
    {
        private readonly IActorRef _tailActor;
        private readonly string _absoluteFilePath;
        private FileSystemWatcher _watcher;
        private readonly string _fileDir;
        private readonly string _filenameOnly;

        public FileObserver(IActorRef tailActor, string absoluteFilePath)
        {
            _tailActor = tailActor;
            _absoluteFilePath = absoluteFilePath;
            _fileDir = Path.GetDirectoryName(_absoluteFilePath);
            _filenameOnly = Path.GetFileName(_absoluteFilePath); 
        }  
        public void Start()
        {
            _watcher = new FileSystemWatcher(_fileDir, _filenameOnly);
            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

            _watcher.Changed += OnFileChanged;
            _watcher.Error += OnError;

            _watcher.EnableRaisingEvents = true; 
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            _tailActor.Tell(new FileError(_filenameOnly, e.GetException().Message), ActorRefs.NoSender); 
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            _tailActor.Tell(new FileWrite(e.Name), ActorRefs.NoSender); 
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
