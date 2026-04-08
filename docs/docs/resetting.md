# Resetting Figures

All <xref:SkanderNET.Figures.Figure> types that can be saved can also be reset using this library through the corresponding figure class' reset function.

When a figure is reset, its data is not cleared. Instead, its header is removed marking it for formatting.
The next time the figure is placed on a portal, its data will then be cleared.

When reset is called with this library, the <xref:SkanderNET.PortalComms.Portal> object will be destroyed so that the figure can be re-processed.
SkanderNET will apply the formatting to a figure when placed after being marked for formatting.

> [!WARNING]
> Reset will fail on traps with pre-trapped villains