using System;

namespace SkanderNET
{
    public class PortalSoundClip
    {
        internal int Id;
        internal PortalAudio Owner;
        public event Action OnStarted;
        public event Action OnFinished;
        
        internal void Started() => OnStarted?.Invoke();
        internal void Finished() => OnFinished?.Invoke();

        public void Stop()
        {
            Owner?.Stop(Id);
        }
    }
}