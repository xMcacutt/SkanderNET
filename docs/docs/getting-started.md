# Getting Started

This library is best installed through [nuget](https://www.nuget.org/packages/SkanderNET).

It has a single dependency, [libusb](https://github.com/libusb/libusb/releases). 
This library is not packaged with the SkanderNET nuget package. 
You must therefore add the library, and its license, to your project manually.

Ensure you add the library as a reference and copy it to your output directory without statically linking or compiling into your binary.

## Quick Start

> [!WARNING]
> The following information is a condensed guide for communicating with the portal and modifying a figure. 
> Please continue to the following pages for a more comprehensive walkthrough of the library and its features.

To begin discovering portals, first subscribe to the <xref:SkanderNET.PortalComms.PortalFinder>'s <xref:SkanderNET.PortalComms.PortalFinder.OnPortalFound> event 
and begin the portal search loop with <xref:SkanderNET.PortalComms.PortalFinder.InitSearch>
```csharp
public static void Main(string[] args)
{
    PortalFinder.OnError += Console.WriteLine;
    PortalFinder.OnPortalFound += OnPortalFound;
    PortalFinder.InitSearch();
    while (true)
    {
        var key = Console.ReadKey();
    }
}
```

Next, define the OnPortalFound delegate, subscribing to the figure events from the discovered portal.
We also save the portal that's found and activate the portal.

```csharp
static void OnPortalFound(Portal portal)
{
    _currentPortal = portal;
    
    portal.OnError += e => { 
        Console.WriteLine($"[ERROR] {e.Message}\n{e.StackTrace}");
        portal.SetColor(Color.Red);
    };
    
    portal.OnReady += () =>
    {
        new Thread(() =>
        {
            for (int flashIndex = 0; flashIndex < 2; flashIndex++)
            {
                portal.SetColor(255, 255, 255);
                Thread.Sleep(200);
                portal.SetColor(0, 0, 0);
                Thread.Sleep(200);
            }
            portal.SetColor(0, 255, 0);
        }).Start();
    };

    portal.OnFigurePlaced += (slot, figure) =>
    {
        Console.WriteLine($"{figure} was placed.");
        SetColorFromElement(portal, figure.Element);
    };
    
    portal.OnFigureProcessed += OnFigureProcessed

    portal.OnFigureRemoved += (slot, figure) => {
        Console.WriteLine($"{figure} was removed.");
    };
    
    portal.Activate();
}
```

> [!TIP]
> The SetColorFromElement function used above is a custom function you may want to use which sets the color of the portal
> depending on the element of the most recent figure placed.
> ```csharp
> static void SetColorFromElement(Portal portal, Element element)
> {
>     switch (element)
>     {
>         case Element.None:
>             portal.SetColor(Color.FromArgb(255, 255, 255));
>             break;
>         case Element.Air:
>             portal.SetColor(Color.FromArgb(0x6c, 0xbc, 0xf5));
>             break;
>         case Element.Earth:
>             portal.SetColor(Color.FromArgb(0xdf, 0x90, 0x30));
>             break;
>         case Element.Fire:
>             portal.SetColor(Color.FromArgb(0xff, 0x00, 0x00));
>             break;
>         case Element.Water: 
>             portal.SetColor(Color.FromArgb(0x00, 0x00, 0xff));
>             break;
>         case Element.Magic:
>             portal.SetColor(Color.FromArgb(0xb7, 0x59, 0xf3)); 
>             break;
>         case Element.Tech:
>             portal.SetColor(Color.FromArgb(0xff, 0x70, 0x30));
>             break;
>         case Element.Life:
>             portal.SetColor(Color.FromArgb(0x00, 0xff, 0x00));
>             break;
>         case Element.Undead:
>             portal.SetColor(Color.FromArgb(0xd0, 0x90, 0xd0));
>             break;
>         case Element.Light:
>             portal.SetColor(Color.FromArgb(0xff, 0xcf, 0x80));
>             break;
>         case Element.Dark:
>             portal.SetColor(Color.FromArgb(0x23, 0x3e, 0x5b));
>             break;
>         case Element.Kaos:
>             portal.SetColor(Color.FromArgb(0x72, 0x66, 0x9b));
>             break;
>         default:
>             throw new ArgumentOutOfRangeException(nameof(element), element, null);
>     }
> ```

Then, we define the OnFigureProcessed delegate.
Here, we'll filter for the figure type and call a different function for each figure type.
For brevity, we'll only filter for Skylanders and Creation Crystals.

```csharp
void OnFigureProcessed(int slot, Figure figure) 
{
    switch (figure.ToyType)
    {
        case ToyType.Skylander:
            HandleSkylander(figure as SkylanderFigure);
            break;
        case ToyType.Crystal:
            HandleCrystal(figure as CreationCrystalFigure);
            break;
    }
}
```

In the handler method, we'll change the money of the skylander and set its Soul Gem to collected.
Then, we'll save changes to the Skylander.

```csharp
void HandleSkylander(SkylanderFigure figure) 
{
    figure.Money += 200;
    figure.SetUpgrade(Upgrade.SoulGem, true);
    figure.Save();
}
```

This should be enough to start experimenting but the following pages provide a much more in depth overview of the features
this library has to offer.