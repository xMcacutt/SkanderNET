# The Skylander Figure

> [!NOTE]
> Many properties and methods from <xref:SkanderNET.Figures.SkylanderFigure> are omitted in this document.
> They are self-explanatory. More info in the API section.

Skylanders cover any playable character other than those made from creation crystals.
This includes core, giants, swappers, trap masters, sidekicks, superchargers, and senseis.

Swappers are two different figures. The <xref:SkanderNET.Figures.Figure.ToyName> on a `Swapper` is
therefore the half of the name that is applicable to the half of the swapper. This makes forming the name of swapper combos easier.

## Hats

Setting and getting hats on figures is complicated since the method for retrieving a hat depends on the game.
Therefore, the game you intend to pull from must be specified when getting the hat through
```csharp
skylander.GetHat(SkylandersGame.SpyrosAdventure) 
``` 
or setting the hat through
```csharp
skylander.SetHat(SkylandersGame.Giants, Hat.CombatHat)
```
When the hat is set, it should be worn in all games for which the hat is supported but does clear all other hats.

## Skills

Modifying skills can be achieved through <xref:SkanderNET.Figures.SkylanderFigure.SetUpgrade(SkanderNET.Data.Upgrade,System.Boolean)> and <xref:SkanderNET.Figures.SkylanderFigure.SetUpgradePath(SkanderNET.Data.UpgradePath)>

`SetUpgrade` specifically sets the upgrades along the path to be unlocked.
Upgrades with index 6-8 are for the path based skills for the currently selected path. When the path is swapped in games which allow for it,
the upgrades at 11-13 are swapped with those in 6-8.

Calling `SetUpgradePath` mimics this behaviour when switching to the opposite path.

## Experience

To make setting and getting the experience easier, the wrapper property, <xref:SkanderNET.Figures.SkylanderFigure.Level>, is provided which allows you to specify a level instead of a raw experience value.

