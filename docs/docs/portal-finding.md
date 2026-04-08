##  Finding Portals

Before communicating with the portal and figures, the portal must first be discovered.

A static, thread safe, singleton <xref:SkanderNET.PortalComms.PortalFinder> is provided to allow finding portals.

> [!WARNING]
> Xbox portals are not currently supported due to authentication complexity.

```csharp
PortalFinder.OnPortalFound += OnPortalFound;
PortalFinder.InitSearch();
```
This should be closed before PortalFinder is destroyed with
```csharp
PortalFinder.Close();
```

When a portal is found, the <xref:SkanderNET.PortalComms.PortalFinder.OnPortalFound> event is invoked providing a <xref:SkanderNET.PortalComms.Portal> object.
This should be stored for communications.

As long as the `Portal Finder`'s thread is running, any disconnected and reconnected portals will be re-detected.

> [!NOTE]
> Only one portal can be active at a time. The first detected portal is used.

The `Portal Finder` also provides the <xref:SkanderNET.PortalComms.PortalFinder.OnError> event for handling any errors finding portals.
The most common error is a missing libusb-1.0.dll.
You may need to invoke its loading with `Kernel32`
```cs
[DllImport("kernel32")]
private static extern IntPtr LoadLibrary(string path);

static void Main() 
{
    LoadLibrary(@"Path\To\libusb-1.0.dll" );
}
```
This is left to you as the API consumer since the dll may be located in non-standard directories.

<xref:SkanderNET.PortalComms.PortalFinder.InitSearch> is safe to be called in multiple places or even multiple threads.
It will start the search loop if it isn't already running.

Subscribers that attach to <xref:SkanderNET.PortalComms.PortalFinder.OnPortalFound> after a portal has already been discovered will be invoked immediately with the current portal.