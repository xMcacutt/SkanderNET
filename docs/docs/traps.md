# The Trap Figure

> [!NOTE]
> Many properties and methods from <xref:SkanderNET.Figures.TrapFigure> are omitted in this document.
> They are self-explanatory. More info in the API section.

Each <xref:SkanderNET.Figures.TrapFigure> contains a list of up to six villains.
These villains are cached and pushed down the list when a new villain is trapped.

The first villain in the list is the `PrimaryVillain`.

The games do not care about the element of the trap matching the element of the villain.
It will load the villain stored in the trap regardless.
The prevention is done purely when trapping villains where the game prevents the wrong villain from being trapped into the wrong element trap in the first place.

It is possible to get and set the cached villains through <xref:SkanderNET.Figures.TrapFigure.TryGetCachedVillain(System.Int32,SkanderNET.Figures.Villain@)> and <xref:SkanderNET.Figures.TrapFigure.TrySetCachedVillain(System.Int32,SkanderNET.Figures.Villain)>.
The primary villain (the one currently trapped) can by directly setting <xref:SkanderNET.Figures.TrapFigure.PrimaryVillain>

In both of these cases, the `Villain` struct that is returned can be modified before being written back to the corresponding villain.

A higher level wrapper, <xref:SkanderNET.Figures.TrapFigure.TrapVillain(SkanderNET.Data.VillainType)>, is also provided to move the specified villain into the primary slot and shift all other stored villains down.

> [!WARNING]
> Some traps (variant traps) have special villain variants preloaded into them. 
> This library will refuse to reset a figure which has one of these since that would result in losing the variant.
> Please avoid using these traps with this tool.

