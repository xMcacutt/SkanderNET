# Saving Changes

Changes made to figures using this library are not reflected on the figure until the save function is explicitly called.
This function is not part of the base figure since saving is unavailable on <xref:SkanderNET.Figures.MagicItemFigure>s and <xref:SkanderNET.Figures.AdventurePackFigure>s.

Upon saving, the <xref:SkanderNET.PortalComms.Portal> object which owns the figure session will invoke <xref:SkanderNET.PortalComms.Portal.OnFigureSaved> with the figure that was saved.

This API leverages the NFC redundancy block to avoid corruption the same way the game does.

> [!WARNING]
> Making changes to Creation Crystals and Senseis is inherently risky due to some blocks being tied to the figure id using a factory private key
> to encrypt some data. This is irretrievable. Careless edits may cause the figure to be unusable. This library should be safe when working with these figures but proceed with caution.

> [!NOTE]
> Using this API, you agree that any figure corruption or damage is the sole responsibility of the end user, and it is recommended that,
should you use the write capabilities of this API, you pass this message on to the end user.
Figures can almost always be restored by navigating to the settings menu on Giants or later from the main menu and selecting `Reset Broken Toys`.

> [!WARNING]
> Usage of this tool for the purposes of piracy or violating the protections placed on the figures is not permitted.