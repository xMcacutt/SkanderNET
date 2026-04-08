# Playing Audio

> [!NOTE]
> Audio may only be played on portals with a speaker (Trap Team portals)

To play audio through the <xref:SkanderNET.PortalComms.Portal>, you must pass a 16bit PCM block to <xref:SkanderNET.PortalComms.Portal.PlayAudio*>.

This can either be done by passing the file path. In this case, the PCM block will be extracted, 
or you can pass the block of data directly as a `byte[]`.

Optionally, a volume can be passed to the call to change how loud the sound will be played through the speaker.

The following snippet is a utility function you are welcome to use which extracts the samples from a sample based audio system like Unity's `AudioClip`
```csharp
var samples = new float[clip.samples];
clip.GetData(samples, 0);
var pcm = new short[samples.Length];
for (var i = 0; i < samples.Length; i++)
    pcm[i] = (short)(samples[i] * 32767f);
var bytes = new byte[pcm.Length * 2];
Buffer.BlockCopy(pcm, 0, bytes, 0, bytes.Length);
```

When calling <xref:SkanderNET.PortalComms.Portal.PlayAudio*> with a raw `byte[]`, note that you must specify the bitrate.
This is purely required for extensibility but should in almost all cases be `16000`.

The resulting object of a call to PlayAudio is a <xref:SkanderNET.PortalComms.PortalSoundClip>.
When called, the sound is queued for playback. This may take some time since the queue must be cleared of other sounds first.
As a result, if you must trigger some code off of either the start or end of a sound, you may subscribe to the events <xref:SkanderNET.PortalComms.PortalSoundClip.OnStarted> and <xref:SkanderNET.PortalComms.PortalSoundClip.OnFinished> immediately after queuing the playback.

If you wish to interrupt playback, you may call either <xref:SkanderNET.PortalComms.PortalSoundClip.Stop>` or to clear all sound <xref:SkanderNET.PortalComms.Portal.ClearAudio>