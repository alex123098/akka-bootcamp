using System;
using System.IO;
using Akka.Actor;

namespace WinTail
{
    public class FileObserver : IDisposable
    {
        private readonly IActorRef _tailActor;
        private readonly string _absoluteFilePath;
        private FileSystemWatcher _fileSystemWatcher;
        private readonly string _fileDir;
        private readonly string _fileName;

        public FileObserver(IActorRef tailActor, string absoluteFilePath)
        {
            _tailActor = tailActor;
            _absoluteFilePath = absoluteFilePath;
            _fileDir = Path.GetDirectoryName(absoluteFilePath);
            _fileName = Path.GetFileName(absoluteFilePath);
        }

        public void Start()
        {
            Environment.SetEnvironmentVariable("MONO_MANAGED_WATCHER", "enabled");
            
            _fileSystemWatcher = new FileSystemWatcher(_fileDir, _fileName)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            _fileSystemWatcher.Changed += OnFileChanged;
            _fileSystemWatcher.Error += OnError;
            
            _fileSystemWatcher.EnableRaisingEvents = true;
        }
        
        public void Dispose()
        {
            if (_fileSystemWatcher != null)
            {
                _fileSystemWatcher.Changed -= OnFileChanged;
                _fileSystemWatcher.Error -= OnError;
                _fileSystemWatcher.Dispose();
            }
        }
        
        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                _tailActor.Tell(new Messages.FileChanged(_fileName), ActorRefs.NoSender);
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            _tailActor.Tell(
                new Messages.FileError(_fileName, e.GetException().Message), 
                ActorRefs.NoSender);
        }
    }
}