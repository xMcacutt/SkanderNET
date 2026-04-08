# The Figure Base

As discussed in the previous section, when a figure is processed, it produces a <xref:SkanderNET.Figures.Figure> object.
This class is the base of each of the more specific figure types.

All figures have some common data that can be accessed even before the figure is fully processed.

Most of the properties are self-explanatory but here's some of the quirks

The <xref:SkanderNET.Figures.Figure.SerialNumber> of the figure can be usually used as a unique identifier for the figure though collisions are possible.
Combined with the <xref:SkanderNET.Figures.Figure.TradingCardId> or <xref:SkanderNET.Figures.Figure.WebCode>, the resulting id should be completely unique.

The <xref:SkanderNET.Figures.Figure.ToyName> property is specifically the name given to the toy, not the nickname applied to the in game character.
If you'd rather have a predefined identifier for the toy, the <xref:SkanderNET.Figures.Figure.Toy> property is preferred. This property is an enum type containing every possible toy.

If the figure is a special variant, for example: lightcore, then it will have some special flags in <xref:SkanderNET.Figures.Figure.Variant>.
It is your responsibility to construct the names of figures using variant info should you wish to display it.

> [!TIP]
> The <xref:SkanderNET.Figures.Figure.ToString> method is overriden to display the toy type followed by the toy name

All of this data is readonly.

 