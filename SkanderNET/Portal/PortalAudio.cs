using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SkanderNET
{
    public class PortalAudio
    {
        private readonly Portal _portal;
        private readonly Queue<byte[]> _audioQueue = new Queue<byte[]>();
        private readonly object _audioLock = new object();

        private readonly Thread _audioThread;
        private volatile bool _cancelRequested;
        private volatile bool _audioStreaming;

        private PortalSoundClip _currentSoundClip;
        private int _soundClipIdCounter;

        internal PortalAudio(Portal portal)
        {
            _portal = portal;
            _audioThread = new Thread(AudioLoop);
            _audioThread.IsBackground = true;
            _audioThread.Start();
        }
        
        internal void HandleAudioResponse(byte[] data)
        {
            lock (_audioLock)
            {
                if (data[1] == 0x01 && _currentSoundClip != null)
                    _audioStreaming = true; 
                else if (data[1] == 0x00)
                    _audioStreaming = false;
            }
        }

        private void AudioLoop()
        {
            bool started = false;

            while (!_cancelRequested)
            {
                byte[] packet = null;
                PortalSoundClip soundToStart = null;
                PortalSoundClip soundToFinish = null;
                
                lock (_audioLock)
                {
                    if (_audioStreaming && _audioQueue.Count > 0)
                    {
                        packet = _audioQueue.Dequeue();
                
                        if (!started)
                        {
                            started = true;
                            soundToStart = _currentSoundClip;
                        }
                    }
                    else if (_audioStreaming && _audioQueue.Count == 0 && started)
                    {
                        _audioStreaming = false;
                        _portal.SendAudioStop();

                        soundToFinish = _currentSoundClip;
                        _currentSoundClip = null;
                        started = false;
                    }
                }

                if (packet != null)
                {
                    try
                    {
                        int written;
                        LibUsb.libusb_interrupt_transfer(_portal.Device, 0x02, packet, 64, out written, 0);
                    }
                    catch (Exception ex)
                    {
                        _portal.Error(new UsbCommunicationsException("Audio USB error: " + ex.Message));
                    }

                    soundToStart?.Started();
                }
                else
                {
                    soundToFinish?.Finished();
                    Thread.Sleep(5);
                }
            }
        }
        
        private static byte[] Downsample(byte[] input, int inRate, int outRate)
        {
            if (inRate == outRate) return input;
            var inSamples = input.Length / 2;
            var outSamples = (int)((long)inSamples * outRate / inRate);
            var output = new byte[outSamples * 2];

            for (var i = 0; i < outSamples; i++)
            {
                var src = (int)((long)i * inRate / outRate);
                Buffer.BlockCopy(input, src * 2, output, i * 2, 2);
            }
            return output;
        }

        internal PortalSoundClip PlayAudio(byte[] pcmData, int sampleRate, float volume = 1.0f)
        {
            if (_portal.Device == IntPtr.Zero)
                return null;

            var soundClip = new PortalSoundClip
            {
                Id = ++_soundClipIdCounter,
                Owner = this
            };

            var pcm = (byte[])pcmData.Clone();

            if (volume < 1.0f && volume > 0.0f)
            {
                for (var i = 0; i < pcm.Length; i += 2)
                {
                    var sample = (short)((pcm[i + 1] << 8) | pcm[i]);
                    var scaled = sample * volume;
                    scaled = Math.Max(short.MinValue, Math.Min(short.MaxValue, scaled));
                    sample = (short)scaled;

                    pcm[i] = (byte)(sample & 0xFF);
                    pcm[i + 1] = (byte)(sample >> 8);
                }
            }

            var processedPcm = Downsample(pcm, sampleRate, 8000);

            lock (_audioLock)
            {
                _currentSoundClip = soundClip;

                var offset = 0;
                while (offset < processedPcm.Length)
                {
                    var packet = new byte[64];
                    var copy = Math.Min(64, processedPcm.Length - offset);
                    Buffer.BlockCopy(processedPcm, offset, packet, 0, copy);
                    _audioQueue.Enqueue(packet);
                    offset += copy;
                }
            }
            
            _portal.SendAudioStart();
            return soundClip;
        }

        internal PortalSoundClip PlayAudio(string path, float volume = 1.0f)
        {
            int sampleRate;
            var pcm = LoadWavPcm(path, out sampleRate);
            return PlayAudio(pcm, sampleRate, volume);
        }
        
        internal void Stop(int id)
        {
            lock (_audioLock)
            {
                if (_currentSoundClip != null && _currentSoundClip.Id == id)
                {
                    _audioQueue.Clear();
                    _audioStreaming = false;
                    _portal.SendAudioStop();

                    _currentSoundClip.Finished();
                    _currentSoundClip = null;
                }
            }
        }

        public static byte[] LoadWavPcm(string path, out int sampleRate)
        {
            byte[] pcmData = null;
            sampleRate = 16000;

            var fs = File.OpenRead(path);
            var br = new BinaryReader(fs);

            br.ReadChars(4); // RIFF
            br.ReadInt32();
            br.ReadChars(4); // WAVE

            short channels = 1;
            short bits = 16;

            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                var chunkId = new string(br.ReadChars(4));
                var chunkSize = br.ReadInt32();

                switch (chunkId)
                {
                    case "fmt ":
                    {
                        br.ReadInt16();
                        channels = br.ReadInt16();
                        sampleRate = br.ReadInt32();
                        br.ReadInt32();
                        br.ReadInt16();
                        bits = br.ReadInt16();
                        if (chunkSize > 16) br.ReadBytes(chunkSize - 16);
                        break;
                    }
                    case "data":
                        pcmData = br.ReadBytes(chunkSize);
                        break;
                    default:
                        br.ReadBytes(chunkSize);
                        break;
                }
            }

            br.Close();
            fs.Close();

            if (pcmData == null || channels != 1 || bits != 16)
                throw new Exception("Only 16-bit mono WAV supported");

            return pcmData;
        }
        
        internal void Clear()
        {
            lock (_audioLock)
            {
                _audioQueue.Clear();
                _audioStreaming = false;
            }
            _portal.SendAudioStop();
        }

        internal void Close()
        {
            _cancelRequested = true;
            if (_audioThread != null && _audioThread.IsAlive)
                _audioThread.Join();
        }
    }
}