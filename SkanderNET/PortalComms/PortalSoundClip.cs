using System;

namespace SkanderNET.PortalComms
{
    public class PortalSoundClip
    {
        internal int Id;
        internal PortalAudio Owner;
        /// <summary>
        /// Invoked when the sound clip starts playing
        /// </summary>
        public event Action OnStarted;
        /// <summary>
        /// Invoked when the sound clip finishes playing
        /// </summary>
        public event Action OnFinished;
        
        internal void Started() => OnStarted?.Invoke();
        internal void Finished() => OnFinished?.Invoke();

        /// <summary>
        /// Stops this sound clip from playing if it is currently playing
        /// </summary>
        public void Stop()
        {
            Owner?.Stop(Id);
        }
    }
}