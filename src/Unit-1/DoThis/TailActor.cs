using System.IO;
using System.Net;
using System.Text;
using Akka.Actor;

namespace WinTail
{
    public class TailActor : UntypedActor
    {
        private readonly string _filePath;
        private readonly IActorRef _reporterActor;
        private FileObserver _fileObserver;
        private Stream _fileStream;
        private StreamReader _fileStreamReader;

        public TailActor(IActorRef reporterActor, string filePath)
        {
            _reporterActor = reporterActor;
            _filePath = filePath;
        }

        protected override void PreStart()
        {
            base.PreStart();
            _fileObserver = new FileObserver(Self, Path.GetFullPath(_filePath));
            _fileObserver.Start();

            _fileStream = new FileStream(
                Path.GetFullPath(_filePath), 
                FileMode.Open, 
                FileAccess.Read,
                FileShare.ReadWrite);
            _fileStreamReader = new StreamReader(_fileStream, Encoding.UTF8);

            var initialContent = _fileStreamReader.ReadToEnd();
            Self.Tell(new Messages.InitialRead(_filePath, initialContent));
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case Messages.FileChanged _ :
                    var text = _fileStreamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(text))
                    {
                        _reporterActor.Tell(text);
                    }
                    break;
                case Messages.FileError error:
                    _reporterActor.Tell($"Tail error: {error.Reason}");
                    break;
                case Messages.InitialRead initial:
                    _reporterActor.Tell(initial.Text);
                    break;
            }
        }

        protected override void PostStop()
        {
            _fileObserver.Dispose();
            _fileStreamReader.Close();
            _fileStreamReader.Dispose();
            base.PostStop();
        }
    }
}