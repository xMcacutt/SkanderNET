# Basic Portal Interactions

Once a <xref:SkanderNET.PortalComms.Portal> has been found by the <xref:SkanderNET.PortalComms.PortalFinder>,
the portal must be activated for it to begin sending and receiving data by calling <xref:SkanderNET.PortalComms.Portal.Activate>.

On a successful activation, the portal will invoke its <xref:SkanderNET.PortalComms.Portal.OnReady> event.

There is an additional <xref:SkanderNET.PortalComms.Portal.OnError> event which will be invoked if any error occurs during portal communications.

## Setting the Light Color
<xref:SkanderNET.PortalComms.Portal.SetColor*> has two overloads.
The first, and most useful, allows you to pass a color by its red, green, and blue components as bytes 0-255.
The second allows you to provide a `System.Drawing` color directly.

Example:
```csharp
portal.SetColor(0xFF, 0x00, 0x00);
portal.SetColor(Color.Red);
```
> [!NOTE]
> On Trap Team portals, this changes both lights. 
> No method is currently provided to set these lights separately.

The following code is an example snippet which causes the portal to flash a few times when it successfully activates.

```csharp
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
```

## Syncing the Portal
Sometimes the portal can get out of sync or confused about what is placed.
There are two methods to fix this. The first line of defense is the <xref:SkanderNET.PortalComms.Portal.Sync> method.
This sends a message to the portal to tell it to resynchronise.

If this is still failing under some situations it might be best to reset the portal with <xref:SkanderNET.PortalComms.Portal.Reset>.
This causes the portal object to be destroyed. Assuming the portal search loop has not been closed in the `Portal Finder`,
the portal should be found again. <xref:SkanderNET.PortalComms.PortalFinder.OnPortalFound> will then be reinvoked.