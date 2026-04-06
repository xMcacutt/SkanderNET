# SkanderNET
![](./SocialImage.png)
A multiframework compatible library for connecting to Skylanders portals.

---

## Usage

This library depends on libusb. You must manually provide this library. Be aware that bundling this library with libusb as
a single library is not permitted as discussed in [Licensing](#licensing).

Use the correct libusb dll for your build system and architecture and provide it as a dll with your library or application.
Avoid statically linking against libusb when using SkanderNET.
[The libusb project can be found here](https://github.com/libusb/libusb/releases)

Before communicating with the portal and figures, the portal must first be discovered.
```cs
PortalFinder.OnPortalFound += OnPortalFound;
PortalFinder.InitSearch();
```
This should be closed before PortalFinder is destroyed with
```cs
PortalFinder.Close();
```

When a portal is found, the `OnPortalFound` event is invoked providing a portal object.
This should be stored for communications.

As long as the `PortalFinder` thread is running, any disconnected and reconnected portals will be re-detected.
Only one portal may be active at a time.

The `PortalFinder` also provides an OnError event for handling any errors finding portals.
The most common error is a missing libusb-1.0.dll.
This library is discussed in more detail later but you may need to invoke its loading with `Kernel32`
```cs
	[DllImport("kernel32")]
	private static extern IntPtr LoadLibrary(string path);
    
    static void Main() 
    {
        LoadLibrary(
			@"Path\To\libusb-1.0.dll"
		);
    }
```
This is left to you as the API consumer since the dll may be located in non-standard directories.

The Portal object provides the following events for subscription:
```cs
// Invoked when a figure is initially placed and its first sector containing character info is processed.
public event Action<int, Figure> OnFigurePlaced;

// Invoked once the figure is fully parsed and data can be read/written.
public event Action<int, Figure> OnFigureProcessed;

// Invoked when a figure is removed from the portal.
public event Action<int, Figure> OnFigureRemoved;

// Invoked when Save is called on a figure.
public event Action<int, Figure> OnFigureSaved;

// Invoked when the portal is activated and ready to receive queries.
public event Action OnReady;

// Invoked when the portal throws an exception.
public event Action<Exception> OnError;
```
After subscribing to the events, you can call
```cs
portal.Activate();
```
To initialize the portal and allow it to start sending messages back and forth.

When a figure is placed on the portal and once the first few blocks of data are received, portal will invoke 
`OnFigurePlaced` with a `Figure` object and its virtual slot index.

The figure, at this point, only has some of its data initialized. 
The object returned is therefore of the `Figure` base type which contains information about the toy type, toy name, serial number, trading card id, and various other elements.

```cs
portal.OnFigurePlaced += (slot, figure) =>
{
    Console.WriteLine($"{figure.ToyName} was placed.");
};
```

Once the figure is processed, the `OnFigureProcessed` event is invoked with the created Figure.
This time, the `Figure` object has a more specific type but the base type is returned.
The `ToyType` property helps to determine the type of the `Figure` object.

| ToyType             | Figure Type           |
|---------------------|-----------------------|
| AdventurePack       | AdventurePackFigure   |
| MagicItem           | MagicItemFigure       |
| Skylander           | SkylanderFigure       |
| Trap                | TrapFigure            |
| Vehicle             | VehicleFigure         |
| RacingPack (Trophy) | RacePackFigure        |
| CreationCrystal     | CreationCrystalFigure |

You can therefore use the following code to select what should happen based on the ToyType:
```cs
portal.OnFigureProcessed += (slot, figure) =>
{
    Console.WriteLine($"{figure.ToyName} Processed.");
    if (figure.ToyType == ToyType.Skylander)
    {
        var skylander = figure as SkylanderFigure;
        Console.WriteLine($"{skylander.Name} has {skylander.Money} money.");
    }
    if (figure.ToyType == ToyType.Trap)
    {
        var trap = figure as TrapFigure;
        VillainMetaData primaryVillainMetaData;
        VillainIndex.Villains.TryGetValue(trap.PrimaryVillain.VillainType, out primaryVillainMetaData);
        var primaryVillainName = primaryVillainMetaData?.Name ?? "Unknown";
        Console.WriteLine($"{primaryVillainName} is trapped.");
    }
};
```

---

### Figures
The following section covers the properties that warrant further explanation.
Anything not listed is assumed to be self-explanatory.

#### Skylanders
Skylanders cover any playable character other than those made from creation crystals.
This includes core, giants, swappers, trap masters, sidekicks, superchargers, and senseis.

Swappers are two different figures. The `ToyName` returned from accessing the property on a SkylanderFigure is
therefore the half of the name that is applicable to the half of the swapper. This makes forming the name of swapper combos easier.

##### Hats

Setting and getting hats on figures is complicated since the method for retrieving a hat depends on the game.
Therefore, the game you intend to pull from must be specified when getting the hat through
```cs
skylander.GetHat(SkylandersGame.SpyrosAdventure)
    or setting the hat through
skylander.SetHat(SkylandersGame.Giants, Hat.CombatHat)
```
When the hat is set, it should be worn in all games for which the hat is supported but does clear all other hats.

##### Skills

Modifying skills can be achieved through the functions
```cs
public void SetUpgrade(int index, bool value)
    and
public void SetUpgradePath(UpgradePath path)
````
`SetUpgrade` specifically sets the upgrades along the path to be unlocked.
Upgrades 6-8 are for the path based skills for the currently selected path. When the path is swapped in games which allow for it,
the upgrades at 11-13 are swapped with those in 6-8.

Calling `SetUpgradePath` mimics this behaviour when switching to the opposite path.

The properties `HasWowPow` and `HasSoulGem` may be used to abstract the need to specify the specific upgrade.

#### Traps
Traps are incredibly simple to modify. Each trap contains a list of up to six villains.
These villains are cached and pushed down the list when a new villain is trapped. 

The first villain in the list is the `PrimaryVillain`. 

The games do not care about the element of the trap matching the element of the villain.
It will load the villain stored in the trap regardless. 
The prevention is done purely when trapping villains where the game prevents the wrong villain from being trapped into the wrong element trap in the first place.

Two methods are provided for modifying the traps
##### Method 1
It is possible to get and set the cached villains through the methods
```cs
public bool TryGetCachedVillain(int villainIndex, out Villain villain) 
    and
public bool TrySetCachedVillain(int villainIndex, Villain value)
```
The primary villain can be modified through
`trap.PrimaryVillain`

In both of these cases, the `Villain` struct that is returned can be modified before being written back to the corresponding villain.

##### Method 2
A higher level wrapper is provided as part of the `TrapFigure` class which automatically moves the cached villains around by calling
```cs
public void TrapVillain(VillainType villainType)
```

#### Vehicles
TBC

#### Racing Packs
TBC

#### Creation Crystals
TBC

---

### Saving

None of the data you modify is saved to the figure until `Save` is explicitly called.
The `Save` function is dependent on the `Figure` object type. Some figure types cannot call save. 
This is intentional to avoid corrupting "readonly" figure types.

Some figure types also feature a `Reset` function. This marks the figure for formatting in the same way `reset broken toys` does in the games.
Formatting itself is handled on loading the figures. If a figure is seen as fully corrupt or marked for formatting, it will be formatted.

Note that variant traps will not be marked for reset by the reset function to avoid the loss of the variant.
However, if the trap becomes fully corrupt, it will cause the trap to be formatted. This is largely unavoidable.

This API leverages the NFC redundancy block to avoid corruption the same way the game does.

Using this API, you agree that any figure corruption or damage is the sole responsibility of the end user, and it is recommended that,
should you use the write capabilities of this API, you pass this message on to the end user.
Figures can almost always be restored by navigating to the settings menu on Giants or later from the main menu and selecting `Reset Broken Toys`.

Usage of this tool for the purposes of piracy or violating the protections placed on the figures is not permitted.

---

### The Portal Sound Engine
Trap Team portals have a built-in speaker. Sound can be sent to the speaker using one of the two following methods.

By path
```cs
PortalSoundClip clip = portal.PlayAudio(@"Path\To\My\Sound.wav", volume: 0.5f);
```
or by raw data. (The PortalAudio class provides a static helper to load PCM data from a path though this is not necessary thanks to the above method.)
```cs
PortalSoundClip clip portal.PlayAudio(pcmData, sampleRate: 16000, volume: 0.5f);
```
The portal is designed to work with 16bit PCM. Sending non PCM or different bit rate packets will potentially fail though everything is downsampled anyway.

The resulting object of a call to PlayAudio is a `PortalSoundClip`.
When a call is made to PlayAudio, the sound is queued for playback. This may take some time since the queue must be cleared of other sounds first.
As a result, if you must trigger some code off of either the start or end of a sound, you may subscribe to the events `clip.OnStarted` or `clip.OnFinished` immediately after queuing the playback.

If you wish to interrupt playback, you may call either `clip.Stop()` or to clear all sound `portal.ClearAudio()`

---

## Licensing

The full license for this project can be found in the License file provided but please note the following:
This project is licensed under Polyform Non-Commercial 1.0.0. 
Any modification of this library is permitted but must be redistributed under Polyform Non-Commercial 1.0.0.

This library also interfaces with libusb, which is licensed under LGPL3.
As a result, any distribution of this library must maintain all libusb as a separate library.
libusb therefore remains licensed under LGPL and SkanderNET under Polyform.
Statically linking both libraries into a single library or binary is prohibited as it forms a license conflict.

Any work distributed with this library may not be used for any commercial gains as expressed in the full Polyform license.
