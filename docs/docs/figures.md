# Handling Placed Figures

When a <xref:SkanderNET.Figures.Figure>
is placed onto an active <xref:SkanderNET.PortalComms.Portal>, 
the portal raises a series of events.

<xref:SkanderNET.PortalComms.Portal.OnFigurePlaced> is invoked when a figure is placed on the portal.

<xref:SkanderNET.PortalComms.Portal.OnFigureRemoved> is invoked when a figure is removed from the portal.

<xref:SkanderNET.PortalComms.Portal.OnFigureSaved> is invoked when a figure is saved (see base Figure class). 

<xref:SkanderNET.PortalComms.Portal.OnFigureProcessed> is invoked when a figure has finished processing.

Each event provides a Figure instance associated with the affected slot.

> [!NOTE]
> New subscribers to the processed event will immediately receive callbacks for any figures already present on the portal.

## Processing Lifecycle

When a figure is first placed, the provided Figure instance is not yet ready for full interaction.
It must first complete an internal processing step.

Once processing is complete:
- The <xref:SkanderNET.PortalComms.Portal.OnFigureProcessed> event is invoked
- The Figure instance can be safely read and modified
- The instance now represents its derived type

Once the <xref:SkanderNET.Figures.Figure> is processed, it takes on a derived <xref:SkanderNET.Figures.Figure> type.

The <xref:SkanderNET.Figures.Figure> base class will be described more in the next section but for now, note that it contains some metadata properties about the toy.

One of these properties is the <xref:SkanderNET.Data.ToyType>. 
To get more specific data about the figure, it can be cast directly to its corresponding type.
This leads to the following pattern:

```csharp
portal.OnFigureProcessed += (slot, figure) =>
{
    if (figure.ToyType == ToyType.RacingPack)
    {
        var racePack = figure as RacePackFigure;
    }
    if (figure.ToyType == ToyType.Vehicle)
    {
        var vehicle = figure as VehicleFigure;
    }
    if (figure.ToyType == ToyType.Crystal)
    {
        var crystal = figure as CreationCrystalFigure;
    }
    if (figure.ToyType == ToyType.Skylander)
    {
        var skylander = figure as SkylanderFigure;
    }
    else if (figure.ToyType == ToyType.Trap)
    {
        var trap = figure as TrapFigure;
    }
};
```
<xref:SkanderNET.Figures.MagicItemFigure> and <xref:SkanderNET.Figures.AdventurePackFigure> are excluded from the above code.
This is because they have no data which can be accessed nor modified beyond their base figure data.

This is your main control flow from which everything related to modifying or reading specific properties from the figure should happen.

> [!WARNING]
> Event invocation always happens on a separate thread. This can cause issues especially in unity.
> If you are intending for data processed from figure events to modify unity elements, you may need to set flags in the event and catch them in the update loop.

Each <xref:SkanderNET.Data.ToyType> has a corresponding subclass deriving from the [base Figure class](figure-base.md).
The unique properties of each type are discussed in their section in the following sections.

| ToyType                                            | Figure Type                                     |
|----------------------------------------------------|-------------------------------------------------|
| <xref:SkanderNET.Data.ToyType.AdventurePack>       | <xref:SkanderNET.Figures.AdventurePackFigure>   |
| <xref:SkanderNET.Data.ToyType.MagicItem>           | <xref:SkanderNET.Figures.MagicItemFigure>       |
| <xref:SkanderNET.Data.ToyType.Skylander>           | <xref:SkanderNET.Figures.SkylanderFigure>       |
| <xref:SkanderNET.Data.ToyType.Trap>                | <xref:SkanderNET.Figures.TrapFigure>            |
| <xref:SkanderNET.Data.ToyType.Vehicle>             | <xref:SkanderNET.Figures.VehicleFigure>         |
| <xref:SkanderNET.Data.ToyType.RacingPack> (Trophy) | <xref:SkanderNET.Figures.RacePackFigure>        |
| <xref:SkanderNET.Data.ToyType.Crystal>             | <xref:SkanderNET.Figures.CreationCrystalFigure> |
