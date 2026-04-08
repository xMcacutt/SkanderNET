# The Race Pack Figure (Trophies)

Before discussing vehicles, it's worth touching on the <xref:SkanderNET.Figures.RacePackFigure> which is the <xref:SkanderNET.Figures.Figure> type 
for the trophies that unlock races in Superchargers.

There's only one piece of unique data stored in these figures: the captured villains from the racing mode.
This is stored as a bitfield so <xref:SkanderNET.Figures.RacePackFigure.CapturedVillains> is of type <xref:SkanderNET.Data.TrophyVillain> which is a flags enum.

# The Vehicle Figure

> [!NOTE]
> Many properties and methods from <xref:SkanderNET.Figures.VehicleFigure> are omitted in this document.
> They are self-explanatory. More info in the API section.

Each vehicle has a different set of unique modifications across three categories (<xref:SkanderNET.Data.Vehicle.VehicleModType>).
There are four modifications per category per vehicle. The <xref:SkanderNET.Figures.VehicleFigure> has special methods for getting and setting the modifications.

To keep the vehicle data ambivalent to the vehicle type, <xref:SkanderNET.Figures.VehicleFigure.GetVehicleMod(SkanderNET.Data.Vehicle.VehicleModType)> 
and <xref:SkanderNET.Figures.VehicleFigure.SetVehicleMod(SkanderNET.Data.Vehicle.VehicleModType,System.Int32)> accept the modification type and return or accept an index 0-3 respectively. 
GetVehicleMod then returns a <xref:SkanderNET.Data.Vehicle.VehicleMod> which contains a name and index.

